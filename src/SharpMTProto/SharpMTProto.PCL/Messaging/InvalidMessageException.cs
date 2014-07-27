using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Messaging
{
    public class InvalidMessageException : MTProtoException
    {
        public InvalidMessageException()
        {
        }

        public InvalidMessageException(string message) : base(message)
        {
        }

        public InvalidMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}