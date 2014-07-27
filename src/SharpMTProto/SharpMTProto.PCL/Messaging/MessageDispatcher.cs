using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Catel.Logging;
using SharpMTProto.Connection;
using SharpMTProto.Messaging.Handlers;
using SharpMTProto.Schema.MTProto;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly AuthenticationManager _authenticationManager;
        private readonly IGZipService _gZipService;
        private readonly HandlerRegistry _handlers;
        private readonly RequestManager _requestManager;
        private readonly TLRig _tlRig;

        private readonly Subject<IMessage> _inMessages = new Subject<IMessage>();

        public MessageDispatcher(
            TLRig tlRig, 
            AuthenticationManager authenticationManager, 
            IGZipService gZipService,
            HandlerRegistry handlers,
            RequestManager requestManager
            )
        {
            this._tlRig = tlRig;
            this._authenticationManager = authenticationManager;
            this._gZipService = gZipService;
            this._handlers = handlers;
            this._requestManager = requestManager;

            this._inMessages
                .ObserveOn(DefaultScheduler.Instance)
                .Subscribe(this.ProcessIncomingMessage);
        }

        public void Receive(IMessage logicalMessage)
        {
            this._inMessages.OnNext(logicalMessage);
        }

        private void ProcessIncomingMessage(IMessage message)
        {
            try
            {
                object response = this._tlRig.Deserialize(message.MessageData);
                if (response != null)
                {
                    this.ProcessIncomingObject(response);
                }
            }
            catch (Exception e)
            {
                Log.Debug(e, "Error on message deserialization.");
            }
        }

        public Task<T> Register<T>(ulong messageId, TimeSpan timeout, Action reSendAction)
        {
            var completionSource = new TaskCompletionSource<T>();
            var cancellation = new CancellationTokenSource(timeout);
            cancellation.Token.Register(() => completionSource.TrySetException(new TimeoutException()));

            var task = completionSource.Task;
            task.ContinueWith(t => this._requestManager.Remove(messageId));

            this._requestManager.Add(messageId, new Request<T>(completionSource, this._gZipService, reSendAction));

            return task;
        }

        public void ReRegister(ulong newMessageId, ulong oldMessageId)
        {
            this._requestManager.Change(newMessageId, oldMessageId);
        }

        public void Receive(Schema.MTProto.IMessage mtProtoMessage)
        {
            this._handlers.Handle(mtProtoMessage.Body);
        }

        private void ProcessIncomingObject(object message)
        {
            this._handlers.Handle(message);
        }
    }
}