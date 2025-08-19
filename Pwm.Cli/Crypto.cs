using System;
using System.Text;
using System.Security.Cryptography;

namespace Pwm;

public static class Crypto
{
    //Create KDF parameters
    public static (byte[] salt, int iterations) CreateKdf(int iterations = 300_000)
        => (RandomNumberGenerator.GetBytes(16), iterations);
    
    //derive 256 bit key from master password
    public static byte[] DeriveKey(string masterPassword, byte[] salt, int iterations)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(masterPassword, salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(32);
    }

    //Encrypt with AES-GCM-256
    public static (byte[] nonce, byte[] ciphertext, byte[] tag) AesGcmEncrypt(byte[] key, byte[] plainText, byte[]? aad = null)
    {
        var nonce = RandomNumberGenerator.GetBytes(12);
        var ciphertext = new byte[plainText.Length];
        var tag = new byte[16];

        using var gcm = new AesGcm(key);
        gcm.Encrypt(nonce, plainText, ciphertext, tag, aad ?? Array.Empty<byte>());
        return (nonce, ciphertext, tag);
    }

    //Decrytp to plain text
    public static byte[] AesGcmDecrypt(byte[] key, byte[] nonce, byte[] ciphertext, byte[] tag, byte[]? aad = null)
    {
        var plaintext = new byte[ciphertext.Length];
        using var gcm = new AesGcm(key);
        gcm.Decrypt(nonce, ciphertext, tag, plaintext, aad ?? Array.Empty<byte>());
        return plaintext;
    }

}