// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyChain.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BigMath.Utils;
using Catel;
using SharpMTProto.Properties;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Authentication
{
    /// <summary>
    ///     Key chain.
    /// </summary>
    public class KeyChain : IKeyChain
    {
        private readonly IHashServices _hashServices;
        private readonly Dictionary<ulong, PublicKey> _keys = new Dictionary<ulong, PublicKey>();
        private readonly TLRig _tlRig;

        public KeyChain([NotNull] TLRig tlRig, [NotNull] IHashServices hashServices)
        {
            Argument.IsNotNull(() => tlRig);
            Argument.IsNotNull(() => hashServices);

            this._tlRig = tlRig;
            this._hashServices = hashServices;
        }

        public PublicKey this[ulong keyFingerprint]
        {
            get { return this._keys.ContainsKey(keyFingerprint) ? this._keys[keyFingerprint] : null; }
        }

        public IEnumerator<PublicKey> GetEnumerator()
        {
            return this._keys.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(PublicKey publicKey)
        {
            if (!this._keys.ContainsKey(publicKey.Fingerprint))
            {
                this._keys.Add(publicKey.Fingerprint, publicKey);
            }
        }

        public void AddKeys(params PublicKey[] publicKeys)
        {
            this.AddKeys(publicKeys.AsEnumerable());
        }

        public void AddKeys(IEnumerable<PublicKey> keys)
        {
            foreach (PublicKey key in keys)
            {
                this.Add(key);
            }
        }

        public void Remove(ulong keyFingerprint)
        {
            if (!this._keys.ContainsKey(keyFingerprint))
            {
                this._keys.Remove(keyFingerprint);
            }
        }

        public bool Contains(ulong keyFingerprint)
        {
            return this._keys.ContainsKey(keyFingerprint);
        }

        public PublicKey GetFirst(IEnumerable<ulong> fingerprints)
        {
            return (from fingerprint in fingerprints where this.Contains(fingerprint) select this[fingerprint]).FirstOrDefault();
        }

        /// <summary>
        ///     Checks key fingerprint.
        /// </summary>
        /// <param name="publicKey">The key.</param>
        /// <returns>True - fingerprint is OK, False - fingerprint is incorrect.</returns>
        public bool CheckKeyFingerprint(PublicKey publicKey)
        {
            byte[] keyData = this._tlRig.Serialize(publicKey, TLSerializationMode.Bare);
            ulong expectedFingerprint = this.ComputeFingerprint(keyData);
            return publicKey.Fingerprint == expectedFingerprint;
        }

        /// <summary>
        ///     Calculates fingerprint for a public RSA key.
        /// </summary>
        /// <param name="modulus">Modulus bytes.</param>
        /// <param name="exponent">Exponent bytes.</param>
        /// <returns>Returns fingerprint as lower 64 bits of the SHA1(RSAPublicKey).</returns>
        public ulong ComputeFingerprint(byte[] modulus, byte[] exponent)
        {
            var tempKey = new PublicKey(modulus, exponent, 0);
            byte[] keyData = this._tlRig.Serialize(tempKey, TLSerializationMode.Bare);
            return this.ComputeFingerprint(keyData);
        }

        /// <summary>
        ///     Calculates fingerprint for a public RSA key.
        /// </summary>
        /// <param name="keyData">Bare serialized type of a constructor: "rsa_public_key n:string e:string = RSAPublicKey".</param>
        /// <returns>Returns fingerprint as lower 64 bits of the SHA1(RSAPublicKey).</returns>
        public ulong ComputeFingerprint(byte[] keyData)
        {
            byte[] hash = this._hashServices.ComputeSHA1(keyData);
            return hash.ToUInt64(hash.Length - 8);
        }
    }
}
