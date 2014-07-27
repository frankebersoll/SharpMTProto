// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpTransportFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using BigMath.Utils;
using Catel.Logging;
using FluentAssertions;
using Nito.AsyncEx;
using NUnit.Framework;
using SharpMTProto.Extra.WinRT;
using SharpMTProto.Transport;

namespace SharpMTProto.Tests.WinRT
{
    [TestFixture]
    public class RTTcpTransportFacts
    {
        [SetUp]
        public void SetUp()
        {
            LogManager.AddDebugListener(true);

            this._serverSocket = new StreamSocketListener();
            this._serverSocket.ConnectionReceived += OnConnectionReceived;
            this._serverSocket.BindServiceNameAsync("0").AsTask().Wait();
            this._serverEndPoint = int.Parse(this._serverSocket.Information.LocalPort);

            Debug.WriteLine(this._serverEndPoint);
        }

        private StreamSocket _clientSocket;

        private void OnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            this._clientSocket = args.Socket;
        }

        [TearDown]
        public void TearDown()
        {
            if (this._serverSocket != null)
            {
                this._serverSocket.Dispose();
                this._serverSocket = null;
            }

            this._serverEndPoint = -1;
        }

        private StreamSocketListener _serverSocket;
        private int _serverEndPoint = -1;

        private RTTcpTransport CreateTcpTransport()
        {
            var config = new TcpTransportConfig("127.0.0.1", this._serverEndPoint) { MaxBufferSize = 0xFF };
            var transport = new RTTcpTransport(config);
            return transport;
        }

        [Test]
        public async Task Should_connect_and_disconnect()
        {
            var transport = this.CreateTcpTransport();

            transport.State.Should().Be(TransportState.Disconnected);
            transport.IsConnected.Should().BeFalse();

            await transport.ConnectAsync();

            var clientSocket = this._clientSocket;

            clientSocket.Should().NotBeNull();

            transport.State.Should().Be(TransportState.Connected);
            transport.IsConnected.Should().BeTrue();

            await transport.DisconnectAsync();
            transport.Dispose();

            transport.State.Should().Be(TransportState.Disconnected);
            transport.IsConnected.Should().BeFalse();
        }

        [Test]
        public async Task Should_process_lost_connection()
        {
            var transport = this.CreateTcpTransport();

            transport.State.Should().Be(TransportState.Disconnected);

            await transport.ConnectAsync();

            var clientSocket = this._clientSocket;

            clientSocket.Should().NotBeNull();

            transport.State.Should().Be(TransportState.Connected);

            clientSocket.Dispose();

            await Task.Delay(200);

            transport.State.Should().Be(TransportState.Disconnected);

            await transport.DisconnectAsync();

            transport.State.Should().Be(TransportState.Disconnected);
        }

        [Test]
        public async Task Should_receive()
        {
            var transport = this.CreateTcpTransport();

            Task<byte[]> receiveTask =
                transport.FirstAsync(b => true)
                         .Timeout(TimeSpan.FromSeconds(1))
                         .ToTask(CancellationToken.None);

            await transport.ConnectAsync();
            var clientSocket = this._clientSocket;

            byte[] payload = "010203040506070809".HexToBytes();

            var packet = new TcpTransportPacket(0x0FA0B1C2, payload);

            var stream = clientSocket.OutputStream.AsStreamForWrite();
            await stream.WriteAsync(packet.Data, 0, packet.Length);
            await stream.FlushAsync();

            byte[] receivedData = await receiveTask;
            receivedData.Should().BeEquivalentTo(payload);

            await transport.DisconnectAsync();
            clientSocket.Dispose();
        }

        [Test]
        public async Task Should_receive_big_payload()
        {
            var transport = this.CreateTcpTransport();

            Task<byte[]> receiveTask = transport
                .FirstAsync(b => true)
                .Timeout(TimeSpan.FromSeconds(1))
                .ToTask(CancellationToken.None);

            await transport.ConnectAsync();
            var clientSocket = this._clientSocket;

            byte[] payload = Enumerable.Range(0, 255)
                .Select(i => (byte)i).ToArray();

            var packet = new TcpTransportPacket(0x0FA0B1C2, payload);
            var stream = clientSocket.OutputStream.AsStreamForWrite();
            await stream.WriteAsync(packet.Data, 0, packet.Length);
            await stream.FlushAsync();

            byte[] receivedData = await receiveTask;
            receivedData.Should().BeEquivalentTo(payload);

            await transport.DisconnectAsync();
            clientSocket.Dispose();
        }

        [Test]
        public async Task Should_receive_multiple_packets()
        {
            var transport = this.CreateTcpTransport();

            var receivedMessages = new AsyncProducerConsumerQueue<byte[]>();
            transport.Subscribe(receivedMessages.Enqueue);

            await transport.ConnectAsync();
            var clientSocket = this._clientSocket;
            var stream = clientSocket.OutputStream.AsStreamForWrite();

            byte[] payload1 = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            var packet1 = new TcpTransportPacket(0, payload1);

            byte[] payload2 = Enumerable.Range(11, 40).Select(i => (byte)i).ToArray();
            var packet2 = new TcpTransportPacket(1, payload2);

            byte[] payload3 = Enumerable.Range(51, 205).Select(i => (byte)i).ToArray();
            var packet3 = new TcpTransportPacket(2, payload3);

            byte[] allData = ArrayUtils.Combine(packet1.Data, packet2.Data, packet3.Data);

            byte[] dataPart1;
            byte[] dataPart2;
            allData.Split(50, out dataPart1, out dataPart2);

            await stream.WriteAsync(dataPart1, 0, dataPart1.Length);
            await stream.FlushAsync();
            await Task.Delay(100);

            await stream.WriteAsync(dataPart2, 0, dataPart2.Length);
            await stream.FlushAsync();
            await Task.Delay(100);

            byte[] receivedData1 = await receivedMessages.DequeueAsync(CancellationTokenHelpers.Timeout(1000).Token);
            receivedData1.Should().BeEquivalentTo(payload1);

            byte[] receivedData2 = await receivedMessages.DequeueAsync(CancellationTokenHelpers.Timeout(1000).Token);
            receivedData2.Should().BeEquivalentTo(payload2);

            byte[] receivedData3 = await receivedMessages.DequeueAsync(CancellationTokenHelpers.Timeout(1000).Token);
            receivedData3.Should().BeEquivalentTo(payload3);

            await transport.DisconnectAsync();
            clientSocket.Dispose();
        }

        [Test]
        public async Task Should_receive_small_parts_less_than_4_bytes()
        {
            var transport = this.CreateTcpTransport();

            Task<byte[]> receiveTask = transport
                .FirstAsync(b => true)
                .Timeout(TimeSpan.FromSeconds(3))
                .ToTask(CancellationToken.None);

            await transport.ConnectAsync();
            var clientSocket = this._clientSocket;

            byte[] payload = "010203040506070809".HexToBytes();

            var packet = new TcpTransportPacket(0x0ABBCCDD, payload);
            byte[] part1 = packet.Data.Take(1).ToArray();
            byte[] part2 = packet.Data.Skip(part1.Length).Take(2).ToArray();
            byte[] part3 = packet.Data.Skip(part1.Length + part2.Length).Take(3).ToArray();
            byte[] part4 = packet.Data.Skip(part1.Length + part2.Length + part3.Length).ToArray();

            var stream = clientSocket.OutputStream.AsStreamForWrite();
            {
                await stream.WriteAsync(part1, 0, part1.Length);
                await stream.FlushAsync();
                await Task.Delay(100);
                await stream.WriteAsync(part2, 0, part2.Length);
                await stream.FlushAsync();
                await Task.Delay(200);
                await stream.WriteAsync(part3, 0, part3.Length);
                await stream.FlushAsync();
                await Task.Delay(50);
                await stream.WriteAsync(part4, 0, part4.Length);
                await stream.FlushAsync();
                await Task.Delay(50);
            }

            byte[] receivedData = await receiveTask;
            receivedData.Should().BeEquivalentTo(payload);

            await transport.DisconnectAsync();
            clientSocket.Dispose();
        }

        [Test]
        public async Task Should_send()
        {
            var transport = this.CreateTcpTransport();

            await transport.ConnectAsync();

            var clientSocket = this._clientSocket;

            byte[] payload = "010203040506070809".HexToBytes();

            await transport.SendAsync(payload);

            var buffer = new byte[30];
            var stream = clientSocket.InputStream.AsStreamForRead();
            var received = await stream.ReadAsync(buffer, 0, buffer.Length);

            var packet = new TcpTransportPacket(buffer, 0, received);
            var receivedPayload = packet.GetPayloadCopy();

            receivedPayload.ShouldAllBeEquivalentTo(payload);
        }
    }
}
