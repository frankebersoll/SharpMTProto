// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationInfo.cs">
//   Copyright (c) 2014 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Authentication
{
    /// <summary>
    ///     Auth info contains of auth key and initial salt.
    /// </summary>
    public struct AuthenticationInfo
    {
        private readonly byte[] _authKey;
        private readonly UInt64 _salt;

        public AuthenticationInfo(byte[] authKey, ulong salt)
        {
            this._authKey = authKey;
            this._salt = salt;
        }

        public byte[] AuthKey
        {
            get { return this._authKey; }
        }

        public ulong Salt
        {
            get { return this._salt; }
        }
    }
}
