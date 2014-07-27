using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Authentication;
using SharpMTProto.Connection;
using SharpMTProto.Messaging;
using SharpMTProto.Messaging.Handlers;
using SharpMTProto.Services;
using SharpMTProto.Transport;
using SharpTL;

namespace SharpMTProto.Client
{
    public class DefaultContainer
    {
        public HandlerRegistry HandlerRegistry { get; private set; }

        public IGZipService GZipService { get; private set; }

        public IMessageReaderFactory MessageReaderFactory { get; private set; }

        public IMessageSenderFactory MessageSenderFactory { get; private set; }

        public IMessageDispatcher MessageDispatcher { get; private set; }

        public ITransportFactory TransportFactory { get; private set; }

        public AuthenticationManager AuthenticationManager { get; private set; }

        public ITransportConfigProvider TransportConfigProvider { get; private set; }

        public IEncryptionServices EncryptionServices { get; private set; }

        public HashServices HashServices { get; private set; }

        public KeyChain KeyChain { get; private set; }

        public TLRig TLRig { get; private set; }

        public INonceGenerator NonceGenerator { get; private set; }

        public MTProtoConnectionFactory ConnectionFactory { get; private set; }

        public IMessageIdGenerator MessageIdGenerator { get; private set; }

        public SessionManager SessionManager { get; private set; }

        public DefaultContainer(ITransportFactory transportFactory)
        {
            this.TransportFactory = transportFactory;
            this.TLRig = TLRig.Default;
            this.HashServices = new HashServices();
            this.KeyChain = new KeyChain(this.TLRig, this.HashServices);
            this.NonceGenerator = new NonceGenerator();
            this.GZipService = new GZipService();
            this.TransportConfigProvider = new TransportConfigProvider();
            this.AuthenticationManager = new AuthenticationManager();
            this.EncryptionServices = new EncryptionServices();
            this.MessageIdGenerator = new MessageIdGenerator();
            this.SessionManager = new SessionManager();
            this.RequestManager = new RequestManager();

            this.MessageReaderFactory = new MessageReaderFactory();
            this.MessageSenderFactory = new MessageSenderFactory();
            this.HandlerRegistry = new HandlerRegistry();

            this.MessageDispatcher = new MessageDispatcher(
                this.TLRig, 
                this.AuthenticationManager, 
                this.GZipService,
                this.HandlerRegistry,
                this.RequestManager);

            this.ConnectionFactory = new MTProtoConnectionFactory(
                this.TransportConfigProvider,
                this.TransportFactory,
                this.MessageReaderFactory,
                this.MessageSenderFactory,
                this.MessageDispatcher
                );

            this.MessageReaderFactory.Initialize(this);
            this.MessageSenderFactory.Initialize(this);
            this.HandlerRegistry.Initialize(this);
        }

        public RequestManager RequestManager { get; private set; }
    }
}