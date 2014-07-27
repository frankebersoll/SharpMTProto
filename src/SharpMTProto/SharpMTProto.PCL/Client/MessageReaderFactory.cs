using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Connection;

namespace SharpMTProto.Client
{
    internal class MessageReaderFactory : IMessageReaderFactory
    {
        private DefaultContainer _container;

        #region IMessageReaderFactory Members

        public IMessageReader CreateReader()
        {
            return new MessageReader(new MessageProcessorDependencies(
                                         this._container.AuthenticationManager,
                                         this._container.EncryptionServices,
                                         this._container.HashServices,
                                         this._container.MessageDispatcher
                                         ));
        }

        public void Initialize(DefaultContainer container)
        {
            this._container = container;
        }

        #endregion
    }
}