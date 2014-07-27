using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SharpMTProto.Services
{
    public class GZipService : IGZipService
    {
        public byte[] Unpack(byte[] packed)
        {
            using (var input = new MemoryStream(packed))
            using (var gzipStream = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzipStream.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}