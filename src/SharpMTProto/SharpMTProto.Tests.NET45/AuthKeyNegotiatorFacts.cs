// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthKeyNegotiatorFacts.cs">
//   Copyright (c) 2014 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using BigMath.Utils;
using Catel.IoC;
using Catel.Logging;
using Catel.Reflection;
using FluentAssertions;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.NSubstitute;
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
    public abstract class AutoMockingTestBase
    {
        [SetUp]
        public void SetUp()
        {
            LogManager.AddDebugListener(true);

            this._kernel = new NSubstituteMockingKernel();
        }

        protected IKernel Kernel
        {
            get { return this._kernel; }
        }

        private NSubstituteMockingKernel _kernel;

        protected ITransport ConfigureConnectionDependenciesWithMockTransport(IObservable<byte[]> inConnector)
        {
            this.Kernel.Bind<IMTProtoConnection>().To<MTProtoConnection>();
            this.Kernel.Bind<TLRig>().ToConstant(TLRig.Default);
            this.Kernel.Bind<IEncryptionServices>().To<EncryptionServices>();
            this.Kernel.Bind<IHashServices>().To<HashServices>();

            this.Kernel.Bind<IKeyChain>().To<KeyChain>().InSingletonScope();
            this.Kernel.Bind<IMessageReader>().To<MessageReader>().InSingletonScope();
            this.Kernel.Bind<IMessageSender>().To<MessageSender>().InSingletonScope();

            this.Kernel.Get<IMTProtoConnectionFactory>()
                .Create(Arg.Any<TransportConfig>())
                .Returns(a => this.Kernel.Get<IMTProtoConnection>(new ConstructorArgument("transportConfig", a[0])));

            this.Kernel.Get<IMessageIdGenerator>()
                .GetNextMessageId().Returns(0x51e57ac42770964aUL, 0x51e57ac917717a27UL, 0x51e57acd2aa32c6dUL);

            this.Kernel.Get<IMessageReaderFactory>()
                .CreateReader().Returns(this.Kernel.Get<IMessageReader>());

            this.Kernel.Get<IMessageSenderFactory>()
                .CreateSender(Arg.Any<MTProtoConnection>())
                .Returns(a => Kernel.Get<IMessageSender>(new ConstructorArgument("connection", a[0])));

            var transport = this.Kernel.Get<ITransport>();
            transport.Subscribe(Arg.Do<IObserver<byte[]>>(o => inConnector.Subscribe(o)));

            this.Kernel.Get<ITransportFactory>()
                .CreateTransport(Arg.Any<TransportConfig>())
                .Returns(transport);

            return transport;
        }

        protected void ConfigureDispatcher()
        {
            this.Kernel.Bind<IMessageDispatcher>().To<MessageDispatcher>().InSingletonScope();
        }
    }

    public static class ArgEnum
    {
        public static T[] Is<T>(IEnumerable<T> enumerable)
        {
            return Arg.Is<T[]>(a => a.SequenceEqual(enumerable));
        }
    }

    [TestFixture]
    public class AuthKeyNegotiatorFacts : AutoMockingTestBase
    {
        [Test]
        public async Task Should_create_auth_key()
        {
            TLRig.Default.PrepareSerializersForAllTLObjectsInAssembly(typeof(MTProtoConnection).Assembly);

            var inConnector = new Subject<byte[]>();
            this.ConfigureDispatcher();
            var transport = this.ConfigureConnectionDependenciesWithMockTransport(inConnector);

            transport.When(t => t.Send(Arg.Is<byte[]>(a => a.SequenceEqual(TestData.ReqPQ))))
                .Do(o => inConnector.OnNext(TestData.ResPQ));
            transport.When(t => t.Send(Arg.Is<byte[]>(a => a.SequenceEqual(TestData.ReqDHParams))))
                .Do(o => inConnector.OnNext(TestData.ServerDHParams));
            transport.When(t => t.Send(Arg.Is<byte[]>(a => a.SequenceEqual(TestData.SetClientDHParams))))
                .Do(o => inConnector.OnNext(TestData.DhGenOk));

            var mockEncryptionServices = Substitute.For<IEncryptionServices>();
            mockEncryptionServices.RSAEncrypt(Arg.Any<byte[]>(), Arg.Any<PublicKey>())
                                  .Returns(TestData.EncryptedData);
            mockEncryptionServices
                .Aes256IgeDecrypt(ArgEnum.Is(TestData.ServerDHParamsOkEncryptedAnswer),
                                  ArgEnum.Is(TestData.TmpAesKey), 
                                  ArgEnum.Is(TestData.TmpAesIV))
                .Returns(TestData.ServerDHInnerDataWithHash);
            mockEncryptionServices
                .Aes256IgeEncrypt(Arg.Is<byte[]>(bytes =>
                                                 bytes.RewriteWithValue(0, bytes.Length - 12, 12)
                                                      .SequenceEqual(TestData.ClientDHInnerDataWithHash)),
                                  ArgEnum.Is(TestData.TmpAesKey), 
                                  ArgEnum.Is(TestData.TmpAesIV))
                .Returns(TestData.SetClientDHParamsEncryptedData);

            mockEncryptionServices.DH(ArgEnum.Is(TestData.B),
                                      ArgEnum.Is(TestData.G),
                                      ArgEnum.Is(TestData.GA),
                                      ArgEnum.Is(TestData.P))
                                  .Returns(new DHOutParams(TestData.GB, TestData.AuthKey));

            this.Kernel.Bind<INonceGenerator>().ToConstant(new TestNonceGenerator());
            this.Kernel.Rebind<IEncryptionServices>().ToConstant(mockEncryptionServices);

            var keyChain = this.Kernel.Get<IKeyChain>();
            keyChain.AddKeys(TestData.TestPublicKeys);

            var connectionFactory = Kernel.Get<IMTProtoConnectionFactory>();
            connectionFactory.DefaultRpcTimeout = TimeSpan.FromSeconds(5);
            connectionFactory.DefaultConnectTimeout = TimeSpan.FromSeconds(5);

            var authKeyNegotiator = Kernel.Get<AuthKeyNegotiator>();

            var authInfo = await authKeyNegotiator.CreateAuthKey();

            authInfo.AuthKey.ShouldAllBeEquivalentTo(TestData.AuthKey);
            authInfo.Salt.Should().Be(TestData.InitialSalt);
        }
    }
}
