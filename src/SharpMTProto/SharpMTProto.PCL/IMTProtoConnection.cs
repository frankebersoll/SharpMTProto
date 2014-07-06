// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMTProtoConnection.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto
{
    public interface IMTProtoConnection : IDisposable, ITLAsyncMethods
    {
        IObservable<IMessage> InMessagesHistory { get; }

        IObservable<IMessage> OutMessagesHistory { get; }

        MTProtoConnectionState State { get; }

        bool IsConnected { get; }

        TimeSpan DefaultRpcTimeout { get; set; }

        TimeSpan DefaultConnectTimeout { get; set; }

        bool IsEncryptionSupported { get; }

        /// <summary>
        /// Connect.
        /// </summary>
        Task<MTProtoConnectResult> Connect();

        /// <summary>
        /// Connect.
        /// </summary>
        Task<MTProtoConnectResult> Connect(CancellationToken cancellationToken);

        /// <summary>
        /// Diconnect.
        /// </summary>
        Task Disconnect();

        void SendEncryptedMessage(byte[] messageData, bool isContentRelated = true);

        /// <summary>
        /// Sends encrypted message and waits for a response.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response which will be awaited.</typeparam>
        /// <param name="requestMessageDataObject">Request message data.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns>Response.</returns>
        /// <exception cref="TimeoutException">When response is not captured within a specified timeout.</exception>
        Task<TResponse> SendEncryptedMessage<TResponse>(object requestMessageDataObject, TimeSpan timeout)
            where TResponse : class;

        void SendMessage(IMessage message);

        Task<TResponse> SendMessage<TResponse>(
            object requestMessageDataObject,
            TimeSpan timeout,
            MessageType messageType) where TResponse : class;

        void SendPlainMessage(byte[] messageData);

        /// <summary>
        /// Sends plain (unencrypted) message and waits for a response.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response which will be awaited.</typeparam>
        /// <param name="requestMessageDataObject">Request message data.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns>Response.</returns>
        /// <exception cref="TimeoutException">When response is not captured within a specified timeout.</exception>
        Task<TResponse> SendPlainMessage<TResponse>(object requestMessageDataObject, TimeSpan timeout)
            where TResponse : class;

        void SetupEncryption(byte[] authKey, ulong salt);
    }

    public enum MTProtoConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2
    }

    public enum MTProtoConnectResult
    {
        Success,
        Timeout,
        Other
    }
}