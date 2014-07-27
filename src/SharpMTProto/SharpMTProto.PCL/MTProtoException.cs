// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Exceptions.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto
{
    public class MTProtoException : Exception
    {
        public MTProtoException()
        {
        }

        public MTProtoException(string message) : base(message)
        {
        }

        public MTProtoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
