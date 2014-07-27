using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Messaging
{
    public class RequestManager
    {
        private readonly SortedDictionary<ulong, IRequest> _requests
            = new SortedDictionary<ulong, IRequest>();

        public void Remove(ulong messageId)
        {
            this._requests.Remove(messageId);
        }

        public void Add(ulong messageId, IRequest request)
        {
            this._requests.Add(messageId, request);
        }

        public void Change(ulong newMessageId, ulong oldMessageId)
        {
            this._requests.Add(newMessageId, this._requests[oldMessageId]);
            this._requests.Remove(oldMessageId);
        }

        public IRequest Get(ulong messageId)
        {
            IRequest request;
            return this._requests.TryGetValue(messageId, out request) ? request : null;
        }

        public IRequest GetForResponse(object response)
        {
            return this._requests.Values.FirstOrDefault(r => r.CanReceive(response));
        }
    }
}