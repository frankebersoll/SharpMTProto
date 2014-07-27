// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MTProtoConnectionFactory.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Catel.IoC;
using SharpMTProto.Messaging;
using SharpMTProto.Transport;

namespace SharpMTProto.Connection
{
    public class MTProtoConnectionFactory : IMTProtoConnectionFactory
    {
        private readonly ITransportConfigProvider _configProvider;
        private readonly ITransportFactory _transportFactory;
        private readonly IMessageReaderFactory _messageReaderFactory;
        private readonly IMessageSenderFactory _messageSenderFactory;
        private readonly IMessageDispatcher _messageDispatcher;

        public MTProtoConnectionFactory(
            ITransportConfigProvider configProvider,
            ITransportFactory transportFactory,
            IMessageReaderFactory messageReaderFactory,
            IMessageSenderFactory messageSenderFactory,
            IMessageDispatcher messageDispatcher
            )
        {
            this.DefaultRpcTimeout = Defaults.RpcTimeout;
            this.DefaultConnectTimeout = Defaults.ConnectTimeout;
            
            this._configProvider = configProvider;
            this._transportFactory = transportFactory;
            this._messageReaderFactory = messageReaderFactory;
            this._messageSenderFactory = messageSenderFactory;
            this._messageDispatcher = messageDispatcher;
        }

        public IMTProtoConnection Create()
        {
            var config = this._configProvider.DefaultTransportConfig;
            return this.Create(config);
        }

        public IMTProtoConnection Create(TransportConfig transportConfig)
        {
            var connection = new MTProtoConnection(
                transportConfig, 
                _transportFactory, 
                _messageReaderFactory,
                _messageSenderFactory,
                _messageDispatcher
                );

            connection.DefaultRpcTimeout = this.DefaultRpcTimeout;
            connection.DefaultConnectTimeout = this.DefaultConnectTimeout;

            return connection;
        }

        public TimeSpan DefaultRpcTimeout { get; set; }
        public TimeSpan DefaultConnectTimeout { get; set; }
    }
}
