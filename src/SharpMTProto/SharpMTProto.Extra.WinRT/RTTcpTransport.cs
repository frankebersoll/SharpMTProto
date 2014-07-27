using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using BigMath.Utils;
using Catel.Logging;
using Nito.AsyncEx;
using SharpMTProto.Transport;
using SharpTL;
using Buffer = System.Buffer;

namespace SharpMTProto.Extra.WinRT
{
    public class RTTcpTransport : ITransport
    {
        private const int PacketLengthBytesCount = 4;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly TcpTransportConfig _config;
        private readonly Subject<byte[]> _in = new Subject<byte[]>();
        private readonly IBuffer _readerBuffer;
        private readonly AsyncLock _stateAsyncLock = new AsyncLock();
        private readonly byte[] _tempLengthBuffer = new byte[PacketLengthBytesCount];
        private CancellationTokenSource _connectionCancellationTokenSource;
        private int _nextPacketBytesCountLeft;
        private byte[] _nextPacketDataBuffer;
        private TLStreamer _nextPacketStreamer;
        private int _packetNumber;
        private Task _receiverTask;
        private StreamSocket _streamSocket;
        private int _tempLengthBufferFill;

        public RTTcpTransport(TcpTransportConfig config)
        {
            if (config.Port <= 0 || config.Port > ushort.MaxValue)
            {
                throw new ArgumentException("Port is incorrect.");
            }

            if (!Regex.IsMatch(config.IPAddress, @"^\d{1,3}(?:\.\d{1,3}){3}$"))
            {
                throw new ArgumentException("IP address is incorrect.");
            }

            this._config = config;
            this._readerBuffer = new byte[config.MaxBufferSize].AsBuffer();
        }

        private async Task ProcessReceivedData(ArraySegment<byte> buffer)
        {
            try
            {
                int bytesRead = 0;
                while (bytesRead < buffer.Count)
                {
                    int startIndex = buffer.Offset + bytesRead;
                    int bytesToRead = buffer.Count - bytesRead;

                    if (this._nextPacketBytesCountLeft == 0)
                    {
                        int tempLengthBytesToRead = PacketLengthBytesCount - this._tempLengthBufferFill;
                        tempLengthBytesToRead = (bytesToRead < tempLengthBytesToRead)
                                                    ? bytesToRead
                                                    : tempLengthBytesToRead;
                        Buffer.BlockCopy(buffer.Array, startIndex, this._tempLengthBuffer, this._tempLengthBufferFill,
                                         tempLengthBytesToRead);

                        this._tempLengthBufferFill += tempLengthBytesToRead;
                        if (this._tempLengthBufferFill < PacketLengthBytesCount)
                        {
                            break;
                        }

                        startIndex += tempLengthBytesToRead;
                        bytesToRead -= tempLengthBytesToRead;

                        this._tempLengthBufferFill = 0;
                        this._nextPacketBytesCountLeft = this._tempLengthBuffer.ToInt32();

                        if (this._nextPacketDataBuffer == null
                            || this._nextPacketDataBuffer.Length < this._nextPacketBytesCountLeft
                            || this._nextPacketStreamer == null)
                        {
                            this._nextPacketDataBuffer = new byte[this._nextPacketBytesCountLeft];
                            this._nextPacketStreamer = new TLStreamer(this._nextPacketDataBuffer);
                        }

                        // Writing packet length.
                        this._nextPacketStreamer.Write(this._tempLengthBuffer);
                        this._nextPacketBytesCountLeft -= PacketLengthBytesCount;
                        bytesRead += PacketLengthBytesCount;
                    }

                    bytesToRead = bytesToRead > this._nextPacketBytesCountLeft
                                      ? this._nextPacketBytesCountLeft
                                      : bytesToRead;

                    this._nextPacketStreamer.Write(buffer.Array, startIndex, bytesToRead);

                    bytesRead += bytesToRead;
                    this._nextPacketBytesCountLeft -= bytesToRead;

                    if (this._nextPacketBytesCountLeft > 0)
                    {
                        break;
                    }

                    var packet = new TcpTransportPacket(this._nextPacketDataBuffer, 0,
                                                        (int) this._nextPacketStreamer.Position);

                    await this.ProcessReceivedPacket(packet);

                    this._nextPacketBytesCountLeft = 0;
                    this._nextPacketStreamer.Position = 0;
                }
            }
            catch (Exception)
            {
                if (this._nextPacketStreamer != null)
                {
                    this._nextPacketStreamer.Dispose();
                    this._nextPacketStreamer = null;
                }
                this._nextPacketDataBuffer = null;
                this._nextPacketBytesCountLeft = 0;

                throw;
            }
        }

        private async Task ProcessReceivedPacket(TcpTransportPacket packet)
        {
            await Task.Run(() => this._in.OnNext(packet.GetPayloadCopy()));
        }

        private async Task StartReceiver(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    IBuffer bytesRead =
                        await this._streamSocket.InputStream
                                  .ReadAsync(
                                             this._readerBuffer,
                                             this._readerBuffer.Capacity,
                                             InputStreamOptions.Partial);

                    if (bytesRead.Length <= 0)
                    {
                        break;
                    }

                    try
                    {
                        byte[] readBytes = this._readerBuffer.ToArray();
                        await this.ProcessReceivedData(new ArraySegment<byte>(readBytes, 0, readBytes.Length));
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Critical error while precessing received data.");
                        break;
                    }
                }

                await this.DisconnectAsync(token);
            }, token).ConfigureAwait(false);
        }

        #region ITransport Members

        public IDisposable Subscribe(IObserver<byte[]> observer)
        {
            return this._in.Subscribe(observer);
        }

        public void Dispose()
        {
            if (this._streamSocket != null)
            {
                this._streamSocket.Dispose();
                this._streamSocket = null;
            }
        }

        public bool IsConnected
        {
            get { return this.State == TransportState.Connected; }
        }

        public TransportState State
        {
            get
            {
                if (this._streamSocket != null)
                {
                    return TransportState.Connected;
                }

                return TransportState.Disconnected;
            }
        }

        public void Connect()
        {
            this.ConnectAsync().Wait();
        }

        public async Task ConnectAsync()
        {
            await this.ConnectAsync(CancellationToken.None);
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            using (await this._stateAsyncLock.LockAsync(token))
            {
                if (this.State == TransportState.Connected)
                {
                    return;
                }

                this._streamSocket = new StreamSocket();

                var hostname = new HostName(this._config.IPAddress);
                string serviceName = this._config.Port.ToString();
                await this._streamSocket.ConnectAsync(hostname, serviceName);

                this._connectionCancellationTokenSource = new CancellationTokenSource();
                this._receiverTask = this.StartReceiver(this._connectionCancellationTokenSource.Token);
            }
        }

        public void Disconnect()
        {
            if (this._streamSocket != null)
            {
                this._streamSocket.Dispose();
                this._streamSocket = null;
            }
        }

        public Task DisconnectAsync()
        {
            this.Disconnect();
            return Task.FromResult(true);
        }

        public Task DisconnectAsync(CancellationToken token)
        {
            this.Disconnect();
            return Task.FromResult(true);
        }

        public void Send(byte[] payload)
        {
            this.SendAsync(payload).Wait();
        }

        public Task SendAsync(byte[] payload)
        {
            return this.SendAsync(payload, CancellationToken.None);
        }

        public async Task SendAsync(byte[] payload, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                var packet = new TcpTransportPacket(this._packetNumber++, payload);
                IBuffer buffer = packet.Data.AsBuffer();
                this._streamSocket.OutputStream.WriteAsync(buffer);
            }, token).ConfigureAwait(false);
        }

        #endregion
    }
}