// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MTProtoConnectionFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using BigMath.Utils;
using Catel.IoC;
using FluentAssertions;
using Ninject;
using Ninject.Parameters;
using NSubstitute;
using NUnit.Framework;
using SharpMTProto.Authentication;
using SharpMTProto.Connection;
using SharpMTProto.Messaging;
using SharpMTProto.Services;
using SharpMTProto.Transport;
using SharpTL;

namespace SharpMTProto.Tests
{
    [TestFixture]
    public class MTProtoConnectionFacts : AutoMockingTestBase
    {
        [Test]
        public async Task Should_send_and_receive_plain_message()
        {
            var inConnector = new Subject<byte[]>();
            var transport = this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            byte[] messageData = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
            byte[] expectedMessageBytes = "00000000000000000807060504030201FF000000".HexToBytes().Concat(messageData).ToArray();
            IMessage actual = null;

            this.Kernel.Get<IMessageDispatcher>()
                .Receive(Arg.Do<IMessage>(m => actual = m));

            using (var connection = this.Kernel.Get<MTProtoConnection>())
            {
                await connection.Connect();

                // Testing sending.
                var message = new PlainMessage(0x0102030405060708UL, messageData);
                connection.Send(message);

                await Task.Delay(100); // Wait while internal sender processes the message.
                transport.Received(1).Send(Arg.Do<byte[]>(b => b.ShouldBeEquivalentTo(expectedMessageBytes)));

                // Testing receiving.
                transport.Received().Subscribe(Arg.Any<IObserver<byte[]>>());

                inConnector.OnNext(expectedMessageBytes);

                await Task.Delay(100); // Wait while internal receiver processes the message.
                actual.MessageBytes.ShouldAllBeEquivalentTo(expectedMessageBytes);

                await connection.Disconnect();
            }
        }

        [Test]
        public async Task Should_send_plain_message_and_wait_for_response()
        {
            var inConnector = new Subject<byte[]>();
            this.ConfigureDispatcher();
            var transport = this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            var request = new TestRequest { TestId = 9 };
            var expectedResponse = new TestResponse { TestId = 9, TestText = "Number 1" };
            var expectedResponseMessage = new PlainMessage(0x0102030405060708, TLRig.Default.Serialize(expectedResponse));

            transport.Send(Arg.Do<byte[]>(a => inConnector.OnNext(expectedResponseMessage.MessageBytes)));

            using (var connection = this.Kernel.Get<MTProtoConnection>())
            {
                await connection.Connect();

                // Testing sending a plain message.
                TestResponse response = await connection.Send<TestResponse>(request, TimeSpan.FromSeconds(5), MessageType.Plain);
                response.Should().NotBeNull();
                response.ShouldBeEquivalentTo(expectedResponse);

                await Task.Delay(100); // Wait while internal sender processes the message.
                IMessage inMessageTask = await connection.OutMessagesHistory.FirstAsync().ToTask();
                transport.Received(1).Send(inMessageTask.MessageBytes);

                await connection.Disconnect();
            }
        }

        [Test]
        public async Task Should_send_encrypted_message_and_wait_for_response()
        {
            var inConnector = new Subject<byte[]>();

            this.ConfigureDispatcher();
            var transport = this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            byte[] authKey = (
                "752BC8FC163832CB2606F7F3DC444D39A6D725761CA2FC984958E20" +
                "EB7FDCE2AA1A65EB92D224CEC47EE8339AA44DF3906D79A01148CB6" +
                "AACF70D53F98767EBD7EADA5A63C4229117EFBDB50DA4399C9E1A5D" +
                "8B2550F263F3D43B936EF9259289647E7AAC8737C4E007C0C910863" +
                "1E2B53C8900C372AD3CCA25E314FBD99AFFD1B5BCB29C5E40BB8366" +
                "F1DFD07B053F1FBBBE0AA302EEEE5CF69C5A6EA7DEECDD965E0411E" +
                "3F00FE112428330EBD432F228149FD2EC9B5775050F079C69CED280" +
                "FE7E13B968783E3582B9C58CEAC2149039B3EF5A4265905D661879A" +
                "41AF81098FBCA6D0B91D5B595E1E27E166867C155A3496CACA9FD6C" +
                "F5D16DB2ADEBB2D3E").HexToBytes();

            const ulong salt = 100500;

            var request = new TestRequest { TestId = 9 };
            var expectedResponse = new TestResponse { TestId = 9, TestText = "Number 1" };
            var expectedResponseMessage = new EncryptedMessage(
                new AuthenticationInfo(authKey, salt), 2, 0x0102030405060708, 3, 
                TLRig.Default.Serialize(expectedResponse), 
                SenderType.Server,
                this.Kernel.Get<IHashServices>(), 
                this.Kernel.Get<IEncryptionServices>());

            transport.Send(Arg.Do<byte[]>(a => inConnector.OnNext(expectedResponseMessage.MessageBytes)));

            this.Kernel.Get<AuthenticationManager>().SetInfo(new AuthenticationInfo(authKey, salt));

            using (var connection = this.Kernel.Get<MTProtoConnection>())
            {
                await connection.Connect();

                // Testing sending an encrypted message.
                TestResponse response = await connection.Send<TestResponse>(request, TimeSpan.FromSeconds(5), MessageType.Encrypted);
                response.Should().NotBeNull();
                response.ShouldBeEquivalentTo(expectedResponse);

                await Task.Delay(100); // Wait while internal sender processes the message.
                IMessage inMessageTask = await connection.OutMessagesHistory.FirstAsync().ToTask();
                transport.Received(1).Send(inMessageTask.MessageBytes);

                await connection.Disconnect();
            }
        }

        [Test]
        public void Should_throw_on_response_timeout()
        {
            var inConnector = new Subject<byte[]>();
            this.ConfigureDispatcher();
            this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            var callingSend = new Func<Task>(async () =>
            {
                using (var connection = this.Kernel.Get<MTProtoConnection>())
                {
                    await connection.Connect();
                    await connection.Send<TestResponse>(new TestRequest(), TimeSpan.FromSeconds(1), MessageType.Plain);
                }
            });

            callingSend.ShouldThrow<TimeoutException>();
        }

        [Test]
        public async Task Should_timeout_on_connect()
        {
            var inConnector = new Subject<byte[]>();
            this.ConfigureDispatcher();
            var transport = this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            transport.ConnectAsync(Arg.Any<CancellationToken>()).Returns(c => Task.Delay(1000));

            using (var connection = this.Kernel.Get<MTProtoConnection>())
            {
                connection.DefaultConnectTimeout = TimeSpan.FromMilliseconds(100);
                MTProtoConnectResult result = await connection.Connect();
                result.ShouldBeEquivalentTo(MTProtoConnectResult.Timeout);
            }
        }
    }
}
