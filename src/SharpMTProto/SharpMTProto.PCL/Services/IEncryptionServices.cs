// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEncryptionServices.cs">
//   Copyright (c) 2014 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using SharpMTProto.Authentication;

namespace SharpMTProto.Services
{
    public interface IEncryptionServices
    {
        byte[] RSAEncrypt(byte[] data, PublicKey publicKey);

        byte[] Aes256IgeDecrypt(byte[] encryptedData, byte[] key, byte[] iv);
        DHOutParams DH(byte[] b, byte[] g, byte[] ga, byte[] p);
        byte[] Aes256IgeEncrypt(byte[] data, byte[] key, byte[] iv);
    }
}
