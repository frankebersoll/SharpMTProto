using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Authentication
{
    /// <summary>
    ///     Key chain interface.
    /// </summary>
    public interface IKeyChain : IEnumerable<PublicKey>
    {
        PublicKey this[ulong keyFingerprint] { get; }
        void Add(PublicKey publicKey);
        void AddKeys(params PublicKey[] publicKeys);
        void AddKeys(IEnumerable<PublicKey> keys);
        void Remove(ulong keyFingerprint);
        bool Contains(ulong keyFingerprint);
        PublicKey GetFirst(IEnumerable<ulong> fingerprints);
    }
}