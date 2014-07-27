using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Messaging.Handlers
{
    public interface IHandle<in TMessage>
    {
        void Handle(TMessage message);
    }
}