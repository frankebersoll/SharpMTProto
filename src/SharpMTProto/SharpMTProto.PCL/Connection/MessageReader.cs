using System;
using System.Collections.Generic;
using System.Linq;
using Catel.Logging;
using SharpMTProto.Messaging;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Connection
{
    public class MessageReader : IMessageReader
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly AuthenticationManager _authenticationManager;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IHashServices _hashServices;
        private readonly IMessageDispatcher _receiver;

        public MessageReader(MessageProcessorDependencies dependencies)
        {
            this._authenticationManager = dependencies.AuthenticationManager;
            this._encryptionServices = dependencies.EncryptionServices;
            this._hashServices = dependencies.HashServices;
            this._receiver = dependencies.Receiver;
        }

        /// <summary>
        /// Processes incoming message bytes.
        /// </summary>
        /// <param name="messageBytes">Incoming bytes.</param>
        public async void ProcessIncomingMessageBytes(byte[] messageBytes)
        {
            TLStreamer streamer = null;
            try
            {
                Log.Debug("Processing incoming message.");
                streamer = new TLStreamer(messageBytes);
                if (messageBytes.Length == 4)
                {
                    int error = streamer.ReadInt32();
                    Log.Debug("Received error code: {0}.", error);
                    return;
                }
                else if (messageBytes.Length < 20)
                {
                    throw new InvalidMessageException(
                        string.Format(
                                      "Invalid message length: {0} bytes. Expected to be at least 20 bytes for message or 4 bytes for error code.",
                                      messageBytes.Length));
                }

                ulong authKeyId = streamer.ReadUInt64();
                if (authKeyId == 0)
                {
                    // Assume the message bytes has a plain (unencrypted) message.
                    Log.Debug(string.Format("Auth key ID = 0x{0:X16}. Assume this is a plain (unencrypted) message.",
                                            authKeyId));

                    // Reading message ID.
                    ulong messageId = streamer.ReadUInt64();
                    if (!this.IsIncomingMessageIdValid(messageId))
                    {
                        throw new InvalidMessageException(string.Format("Message ID = 0x{0:X16} is invalid.", messageId));
                    }

                    // Reading message data length.
                    int messageDataLength = streamer.ReadInt32();
                    if (messageDataLength <= 0)
                    {
                        throw new InvalidMessageException("Message data length must be greater than zero.");
                    }

                    // Reading message data.
                    var messageData = new byte[messageDataLength]; // TODO: consider reusing of byte arrays.
                    int read =
                        await streamer.ReadAsync(messageData, 0, messageDataLength); // TODO: cancellation
                    // await streamer.ReadAsync(messageData, 0, messageDataLength, this._connectionCancellationToken);
                    if (read != messageDataLength)
                    {
                        throw new InvalidMessageException(
                            string.Format("Actual message data length ({0}) is not as expected ({1}).", read,
                                          messageDataLength));
                        // TODO: read message data if read is less than expected.
                    }

                    // Notify in-messages subject.
                    var message = new PlainMessage(messageId, messageData);

                    Log.Debug(
                              string.Format(
                                            "Received plain message. Message ID = 0x{0:X16}. Message data length: {1} bytes.",
                                            messageId, messageDataLength));

                    this._receiver.Receive(message);
                }
                else
                {
                    // Assume the stream has an encrypted message.
                    Log.Debug(string.Format("Auth key ID = 0x{0:X16}. Assume this is encrypted message.", authKeyId));
                    if (!this._authenticationManager.IsEncryptionSupported)
                    {
                        Log.Debug("Encryption is not supported by this connection.");
                        return;
                    }

                    byte[] authKey = this._authenticationManager.Info.AuthKey;
                    var message = new EncryptedMessage(authKey, messageBytes, SenderType.Server, this._hashServices,
                                                       this._encryptionServices);

                    Log.Debug(
                              string.Format(
                                            "Received encrypted message. Message ID = 0x{0:X16}. Message data length: {1} bytes.",
                                            message.MessageId, message.MessageDataLength));

                    this._receiver.Receive(message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to receive a message.");
            }
            finally
            {
                if (streamer != null)
                {
                    streamer.Dispose();
                }
            }
        }

        private bool IsIncomingMessageIdValid(ulong messageId)
        {
            // TODO: check.
            return true;
        }
    }
}