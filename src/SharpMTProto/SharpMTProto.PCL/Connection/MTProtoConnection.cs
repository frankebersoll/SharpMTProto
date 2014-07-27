// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MTProtoConnection.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using BigMath.Utils;
using Catel;
using Catel.IoC;
using Catel.Logging;
using SharpMTProto.Messaging;
using SharpMTProto.Properties;
using SharpMTProto.Transport;
using AsyncLock = Nito.AsyncEx.AsyncLock;

namespace SharpMTProto.Connection
{
    /// <summary>
    /// MTProto connection.
    /// </summary>
    public class MTProtoConnection : IMTProtoConnection
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly AsyncLock _lock = new AsyncLock();
        private readonly IMessageReader _reader;
        private readonly IMessageDispatcher _receiver;
        private readonly IMessageSender _sender;
        private readonly ITransport _transport;

        private CancellationToken _connectionCancellationToken;
        private CancellationTokenSource _connectionCts;
        private bool _isDisposed;
        private Subject<IMessage> _outMessages = new Subject<IMessage>();
        private readonly ReplaySubject<IMessage> _outMessageHistory = new ReplaySubject<IMessage>(100); 

        private volatile MTProtoConnectionState _state = MTProtoConnectionState.Disconnected;

        public IMessageSender MessageSender
        {
            get { return this._sender; }
        }

        public ISubject<IMessage> OutMessagesHistory { get { return this._outMessageHistory; } } 

        public MTProtoConnection(
            [NotNull] TransportConfig transportConfig,
            [NotNull] ITransportFactory transportFactory,
            [NotNull] IMessageReaderFactory readerFactory,
            [NotNull] IMessageSenderFactory senderFactory,
            [NotNull] IMessageDispatcher receiver
            )
        {
            Argument.IsNotNull(() => transportFactory);

            this.DefaultConnectTimeout = Defaults.ConnectTimeout;

            this._receiver = receiver;
            this._reader = readerFactory.CreateReader();
            this._sender = senderFactory.CreateSender(this);

            // Init transport.
            this._transport = transportFactory.CreateTransport(transportConfig);

            // Connector in/out.
            this._transport.ObserveOn(DefaultScheduler.Instance)
                .Do(bytes => LogMessageInOut(bytes, "IN"))
                .Subscribe(OnIncomingBytes);

            this._outMessages.ObserveOn(DefaultScheduler.Instance)
                .Do(message => LogMessageInOut(message.MessageBytes, "OUT"))
                .Subscribe(OnOutgoingMessage);

            this._outMessages.ObserveOn(DefaultScheduler.Instance).Subscribe(_outMessageHistory);
        }

        private void OnOutgoingMessage(IMessage message)
        {
            this._transport.Send(message.MessageBytes);
        }

        private void OnIncomingBytes(byte[] bytes)
        {
            this._reader.ProcessIncomingMessageBytes(bytes);
        }

        private static void LogMessageInOut(byte[] messageBytes, string inOrOut)
        {
            Log.Debug(string.Format("{0} ({1} bytes): {2}", inOrOut, messageBytes.Length, messageBytes.ToHexString()));
        }

        private void ThrowIfDisconnected()
        {
            if (!this.IsConnected)
            {
                throw new InvalidOperationException("Not allowed when disconnected.");
            }
        }

        #region Disposable

        protected virtual void Dispose(bool isDisposing)
        {
            if (this._isDisposed)
            {
                return;
            }
            this._isDisposed = true;

            if (isDisposing)
            {
                this.Disconnect().Wait(5000);

                this._outMessages.Dispose();
                this._outMessages = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region IMTProtoConnection Members

        public TimeSpan DefaultConnectTimeout { get; set; }

        public MTProtoConnectionState State
        {
            get { return this._state; }
        }

        public bool IsConnected
        {
            get { return this._state == MTProtoConnectionState.Connected; }
        }

        public TimeSpan DefaultRpcTimeout
        {
            get { return this._sender.DefaultRpcTimeout; }
            set { this._sender.DefaultRpcTimeout = value; }
        }

        public void Send(IMessage message)
        {
            this.ThrowIfDisconnected();

            this._outMessages.OnNext(message);
        }

        public Task<T> Send<T>(object request, TimeSpan timeout, MessageType messageType)
        {
            return this._sender.SendMessage<T>(request, timeout, messageType);
        }

        /// <summary>
        /// Start sender and receiver tasks.
        /// </summary>
        public async Task<MTProtoConnectResult> Connect()
        {
            return await this.Connect(CancellationToken.None);
        }

        /// <summary>
        /// Connect.
        /// </summary>
        public async Task<MTProtoConnectResult> Connect(CancellationToken cancellationToken)
        {
            var result = MTProtoConnectResult.Other;

            await Task.Run(async () =>
            {
                using (await this._lock.LockAsync(cancellationToken))
                {
                    if (this._state == MTProtoConnectionState.Connected)
                    {
                        result = MTProtoConnectResult.Success;
                        return;
                    }
                    Debug.Assert(this._state == MTProtoConnectionState.Disconnected);
                    try
                    {
                        this._state = MTProtoConnectionState.Connecting;
                        Log.Debug("Connecting...");

                        await
                            this._transport.ConnectAsync(cancellationToken)
                                .ToObservable()
                                .Timeout(this.DefaultConnectTimeout);

                        this._connectionCts = new CancellationTokenSource();
                        this._connectionCancellationToken = this._connectionCts.Token;

                        Log.Debug("Connected.");
                        result = MTProtoConnectResult.Success;
                    }
                    catch (TimeoutException)
                    {
                        result = MTProtoConnectResult.Timeout;
                        Log.Debug(string.Format("Failed to connect due to timeout ({0}s).",
                                                this.DefaultConnectTimeout.TotalSeconds));
                    }
                    catch (Exception e)
                    {
                        result = MTProtoConnectResult.Other;
                        Log.Debug(e, "Failed to connect.");
                    }
                    finally
                    {
                        switch (result)
                        {
                            case MTProtoConnectResult.Success:
                                this._state = MTProtoConnectionState.Connected;
                                break;
                            case MTProtoConnectResult.Timeout:
                            case MTProtoConnectResult.Other:
                                this._state = MTProtoConnectionState.Disconnected;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }, cancellationToken).ConfigureAwait(false);

            return result;
        }

        public async Task Disconnect()
        {
            await Task.Run(async () =>
            {
                using (await this._lock.LockAsync(CancellationToken.None))
                {
                    if (this._state == MTProtoConnectionState.Disconnected)
                    {
                        return;
                    }
                    this._state = MTProtoConnectionState.Disconnected;

                    if (this._connectionCts != null)
                    {
                        this._connectionCts.Cancel();
                        this._connectionCts.Dispose();
                        this._connectionCts = null;
                    }

                    await this._transport.DisconnectAsync(CancellationToken.None);
                }
            }, CancellationToken.None).ConfigureAwait(false);
        }

        #endregion
    }
}