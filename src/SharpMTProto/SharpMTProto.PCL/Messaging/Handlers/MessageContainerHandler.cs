using System;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class MessageContainerHandler : IHandle<IMessageContainer>
    {
        private readonly IMessageDispatcher _dispatcher;

        public MessageContainerHandler(IMessageDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Handle(IMessageContainer message)
        {
            foreach (var childMessage in message.Messages)
            {
                _dispatcher.Receive(childMessage);
            }      
        }
    }
}