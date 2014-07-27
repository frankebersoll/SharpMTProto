using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class AcknowledgeHandler : IHandle<IMsgsAck>
    {
        private readonly RequestManager _requestManager;

        public AcknowledgeHandler(RequestManager requestManager)
        {
            this._requestManager = requestManager;
        }

        public void Handle(IMsgsAck message)
        {
            foreach (var id in message.MsgIds)
            {
                var request = _requestManager.Get(id);
                if (request != null)
                {
                    request.Acknowledge();
                }
            }
        }
    }
}