using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Messaging;
using SharpMTProto.Services;

namespace SharpMTProto.Connection
{
    public class MessageProcessorDependencies
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IHashServices _hashServices;
        private readonly IMessageDispatcher _receiver;

        public AuthenticationManager AuthenticationManager
        {
            get { return this._authenticationManager; }
        }

        public IEncryptionServices EncryptionServices
        {
            get { return this._encryptionServices; }
        }

        public IHashServices HashServices
        {
            get { return this._hashServices; }
        }

        public IMessageDispatcher Receiver
        {
            get { return this._receiver; }
        }

        public MessageProcessorDependencies(
            AuthenticationManager authenticationManager,
            IEncryptionServices encryptionServices,
            IHashServices hashServices,
            IMessageDispatcher receiver)
        {
            this._authenticationManager = authenticationManager;
            this._encryptionServices = encryptionServices;
            this._hashServices = hashServices;
            this._receiver = receiver;
        }
    }
}