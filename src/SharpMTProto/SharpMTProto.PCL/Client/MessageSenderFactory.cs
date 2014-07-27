using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Connection;

namespace SharpMTProto.Client
{
    internal class MessageSenderFactory : IMessageSenderFactory
    {
        private DefaultContainer _container;

        #region IMessageSenderFactory Members

        public IMessageSender CreateSender(MTProtoConnection mtProtoConnection)
        {
            return new MessageSender(
                mtProtoConnection,
                new MessageProcessorDependencies(
                    this._container.AuthenticationManager,
                    this._container.EncryptionServices,
                    this._container.HashServices,
                    this._container.MessageDispatcher
                    ), this._container.TLRig,
                this._container.MessageIdGenerator,
                this._container.SessionManager
                );
        }

        public void Initialize(DefaultContainer container)
        {
            this._container = container;
        }

        #endregion
    }
}