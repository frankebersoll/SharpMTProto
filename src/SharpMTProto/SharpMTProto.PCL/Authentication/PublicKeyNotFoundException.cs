using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMTProto.Authentication
{
    public class PublicKeyNotFoundException : MTProtoException
    {
        public PublicKeyNotFoundException(ulong fingerprint) : base(string.Format("Public key with fingerprint {0:X16} not found.", fingerprint))
        {
        }

        public PublicKeyNotFoundException(IEnumerable<ulong> fingerprints) : base(GetMessage(fingerprints))
        {
        }

        private static string GetMessage(IEnumerable<ulong> fingerprints)
        {
            var sb = new StringBuilder();
            sb.Append("There are no keys found with corresponding fingerprints: ");
            foreach (var fingerprint in fingerprints)
            {
                sb.Append(string.Format("0x{0:X16}", fingerprint));
            }
            sb.Append(".");
            return sb.ToString();
        }
    }
}