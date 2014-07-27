using System;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class ForwardToFirstRequestHandler : IHandle<object>
    {
        private readonly RequestManager _requestManager;

        public ForwardToFirstRequestHandler(RequestManager requestManager)
        {
            this._requestManager = requestManager;
        }

        public void Handle(object message)
        {
            var request = _requestManager.GetForResponse(message);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }

            request.Receive(message);
        }
    }
}