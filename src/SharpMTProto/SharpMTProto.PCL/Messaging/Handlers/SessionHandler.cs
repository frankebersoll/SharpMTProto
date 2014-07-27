using SharpMTProto.Connection;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class SessionHandler : IHandle<INewSession>
    {
        private readonly SessionManager _sessionManager;

        public SessionHandler(SessionManager sessionManager)
        {
            this._sessionManager = sessionManager;
        }

        public void Handle(INewSession message)
        {
            // TODO
        }
    }
}