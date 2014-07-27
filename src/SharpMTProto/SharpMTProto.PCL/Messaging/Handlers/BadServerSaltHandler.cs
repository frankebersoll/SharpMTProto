using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Connection;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging.Handlers
{
    public class BadServerSaltHandler : IHandle<BadServerSalt>
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly RequestManager _requestManager;

        public BadServerSaltHandler(AuthenticationManager authenticationManager, RequestManager requestManager)
        {
            this._authenticationManager = authenticationManager;
            this._requestManager = requestManager;
        }

        #region IHandle<BadServerSalt> Members

        public void Handle(BadServerSalt message)
        {
            this._authenticationManager.SetSalt(message.NewServerSalt);

            IRequest request = this._requestManager.Get(message.BadMsgId);
            if (request != null)
            {
                request.Resend();
            }
        }

        #endregion
    }
}