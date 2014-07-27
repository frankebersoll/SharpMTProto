using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Messaging;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Connection
{
    public class MessageSender : IMessageSender
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly IMTProtoConnection _connection;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IHashServices _hashServices;
        private readonly IMessageIdGenerator _messageIdGenerator;
        private readonly IMessageDispatcher _messageReceiver;
        private readonly SessionManager _sessionManager;
        private readonly TLRig _tlRig;

        private uint _messageSeqNumber;

        public TimeSpan DefaultRpcTimeout { get; set; }

        public MessageSender(
            IMTProtoConnection connection,
            MessageProcessorDependencies dependencies,
            TLRig tlRig,
            IMessageIdGenerator messageIdGenerator,
            SessionManager sessionManager)
        {
            this._authenticationManager = dependencies.AuthenticationManager;
            this._encryptionServices = dependencies.EncryptionServices;
            this._messageReceiver = dependencies.Receiver;
            this._hashServices = dependencies.HashServices;

            this._connection = connection;
            this._tlRig = tlRig;
            this._messageIdGenerator = messageIdGenerator;
            this._sessionManager = sessionManager;

            this.DefaultRpcTimeout = Defaults.RpcTimeout;
        }

        public Task<T> SendMessage<T>(object request, TimeSpan timeout, MessageType messageType)
        {
            if (messageType == MessageType.Encrypted && !this._authenticationManager.IsEncryptionSupported)
            {
                throw new InvalidOperationException(
                    "Encryption is not supported. Setup encryption first by calling SetupEncryption() method.");                
            }

            this.ThrowIfDisconnected();

            byte[] messageData = this._tlRig.Serialize(request);
            var messageId = this.GetNextMessageId();
            var message = this.CreateMessage(messageType, messageId, messageData);

            Action reSendAction = () =>
            {
                var newMessageId = this.GetNextMessageId();
                var newMessage = this.CreateMessage(messageType, newMessageId, messageData);
                this._messageReceiver.ReRegister(newMessageId, messageId);
                this._connection.Send(newMessage);
            };

            var task = this._messageReceiver.Register<T>(message.MessageId, timeout, reSendAction);
            this._connection.Send(message);

            return task;
        }

        private IMessage CreateMessage(MessageType messageType, ulong messageId, byte[] messageData)
        {
            IMessage message;
            switch (messageType)
            {
                case MessageType.Plain:
                    message = new PlainMessage(messageId, messageData);
                    break;
                case MessageType.Encrypted:
                    message = new EncryptedMessage(this._authenticationManager.Info, this._sessionManager.SessionId,
                                                   messageId,
                                                   this.GetNextSeqNo(true), messageData, SenderType.Client,
                                                   this._hashServices,
                                                   this._encryptionServices);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("messageType");
            }
            return message;
        }

        private ulong GetNextMessageId()
        {
            return this._messageIdGenerator.GetNextMessageId();
        }

        private uint GetNextSeqNo(bool isContentRelated)
        {
            uint x = (isContentRelated ? 1u : 0);
            uint result = this._messageSeqNumber*2 + x;
            this._messageSeqNumber += x;
            return result;
        }

        private void ThrowIfDisconnected()
        {
            if (!this._connection.IsConnected)
            {
                throw new InvalidOperationException("Not allowed when disconnected.");
            }
        }
    }
}