using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Catel.Logging;
using SharpMTProto.Client;
using SharpMTProto.Schema.Api;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class HandlerRegistry
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Type, dynamic> _dictionary
            = new Dictionary<Type, dynamic>();

        public void Handle(object message)
        {
            Type type = message.GetType();

            dynamic handler;
            if (!this._dictionary.TryGetValue(type, out handler))
            {
                Log.WarningWithData(string.Format("No handler found for message type '{0}'.", type));
                return;
            }

            handler.Handle((dynamic) message);
        }

        private static readonly string[] MessageNamespaces =
        {
            typeof(ResPQ).Namespace
        };

        private static readonly Type[] AllMessageTypes = typeof(HandlerRegistry)
            .Assembly()
            .ExportedTypes
            .Where(t => MessageNamespaces.Contains(t.Namespace))
            .ToArray();

        private void AddHandler<TMessage>(dynamic handler)
        {
            var derivedTypes = AllMessageTypes
                .Where(type => typeof(TMessage).IsAssignableFrom(type));

            foreach (var derivedType in derivedTypes)
            {
                this._dictionary.Add(derivedType, handler);                
            }
        }

        public void Initialize(DefaultContainer container)
        {
            this.AddHandler<BadServerSalt>(new BadServerSaltHandler(
                                               container.AuthenticationManager,
                                               container.RequestManager));

            var firstRequestHandler = new ForwardToFirstRequestHandler(container.RequestManager);
            this.AddHandler<IResPQ>(firstRequestHandler);
            this.AddHandler<IServerDHParams>(firstRequestHandler);
            this.AddHandler<ISetClientDHParamsAnswer>(firstRequestHandler);

            this.AddHandler<IRpcResult>(new RpcHandler(container.RequestManager));
            this.AddHandler<IMessageContainer>(new MessageContainerHandler(container.MessageDispatcher));
            this.AddHandler<INewSession>(new SessionHandler(container.SessionManager));
            this.AddHandler<IMsgsAck>(new AcknowledgeHandler(container.RequestManager));

            this.AddHandler<IUpdates>(new UpdatesHandler());
        }
    }
}