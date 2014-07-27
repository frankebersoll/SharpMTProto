using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Services
{
    /// <summary>
    ///     Interface for a message ID generator.
    /// </summary>
    public interface IMessageIdGenerator
    {
        ulong GetNextMessageId();
    }
}