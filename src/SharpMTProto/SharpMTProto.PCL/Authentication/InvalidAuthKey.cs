using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Authentication
{
    public class InvalidAuthKey : MTProtoException
    {
        public InvalidAuthKey()
        {
        }

        public InvalidAuthKey(string message) : base(message)
        {
        }

        public InvalidAuthKey(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}