// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessage.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Messaging
{
    public interface IMessage
    {
        int Length { get; }
        byte[] MessageBytes { get; }

        /// <summary>
        ///     Plain inner message data.
        /// </summary>
        byte[] MessageData { get; }

        ulong MessageId { get; }
    }
}
