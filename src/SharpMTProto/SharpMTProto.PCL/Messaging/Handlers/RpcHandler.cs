using Catel.Logging;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class RpcHandler : IHandle<IRpcResult>
    {
        private readonly RequestManager _requestManager;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public RpcHandler(RequestManager requestManager)
        {
            this._requestManager = requestManager;
        }

        public void Handle(IRpcResult message)
        {
            IRequest handler = _requestManager.Get(message.ReqMsgId);
            if (handler != null)
            {
                handler.Receive(message.Result);
            }
            else
            {
                Log.Debug("Couldn't match incoming message.");
            }
        }
    }
}