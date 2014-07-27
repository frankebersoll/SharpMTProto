using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Services
{
    public class DHOutParams
    {
        public DHOutParams(byte[] gb, byte[] s)
        {
            this.GB = gb;
            this.S = s;
        }

        public byte[] GB { get; set; }
        public byte[] S { get; set; }
    }
}