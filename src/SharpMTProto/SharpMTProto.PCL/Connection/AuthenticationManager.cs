using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Authentication;

namespace SharpMTProto.Connection
{
    public class AuthenticationManager
    {
        private byte[] _authKey;

        private ulong _salt;

        public bool IsEncryptionSupported
        {
            get { return this._authKey != null; }
        }

        public AuthenticationInfo Info
        {
            get
            {
                return this.IsEncryptionSupported
                           ? new AuthenticationInfo(this._authKey, this._salt)
                           : new AuthenticationInfo();
            }
        }

        public void SetInfo(AuthenticationInfo authenticationInfo)
        {
            this._authKey = authenticationInfo.AuthKey;
            this._salt = authenticationInfo.Salt;
        }

        public void SetSalt(ulong salt)
        {
            this._salt = salt;
        }
    }
}