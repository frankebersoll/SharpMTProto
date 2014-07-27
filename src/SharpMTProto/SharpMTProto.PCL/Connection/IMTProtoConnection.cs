// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMTProtoConnection.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using SharpMTProto.Messaging;
using IMessage = SharpMTProto.Messaging.IMessage;

namespace SharpMTProto.Connection
{
    public interface IMTProtoConnection : IDisposable
    {
        MTProtoConnectionState State { get; }

        bool IsConnected { get; }

        TimeSpan DefaultRpcTimeout { get; set; }

        TimeSpan DefaultConnectTimeout { get; set; }

        ISubject<IMessage> OutMessagesHistory { get; }

        Task<T> Send<T>(object request, TimeSpan timeout, MessageType messageType);

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

        void Send(IMessage message);
    }
}