using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Connection
{
    public class CouldNotConnectException : MTProtoException
    {
        public CouldNotConnectException()
        {
        }

        public CouldNotConnectException(string message, MTProtoConnectResult result) : base(message)
        {
        }

        public CouldNotConnectException(string message, MTProtoConnectResult result, Exception innerException) : base(message, innerException)
        {
        }
    }
}