using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Authentication
{
    public class InvalidResponseException : MTProtoException
    {
        public InvalidResponseException()
        {
        }

        public InvalidResponseException(string message) : base(message)
        {
        }

        public InvalidResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}