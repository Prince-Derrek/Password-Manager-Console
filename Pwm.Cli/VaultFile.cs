using System;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace Pwm;

public sealed class VaultFile
{
    private readonly string _path;
    public VaultFile(string path) => _path = path;
    public bool Exists() => File.Exists(_path);
    public void Init(string masterPassword)
    {
        if (Exists()) throw new InvalidOperationException("Vault already exists!");

        var (salt, iter) = Crypto.CreateKdf();
        var key = Crypto.DeriveKey(masterPassword, salt, iter);

        var vault = new VaultData();
        var json = JsonSerializer.SerializeToUtf8Bytes(vault, new JsonSerializerOptions { WriteIndented = false });

        var (nonce, ct, tag) = Crypto.AesGcmEncrypt(key, json);

        var env = new VaultFileEnvelope
        {
            Version = "1",
            Aead = "AES-GCM-256",
            Kdf = "PBKDF2-SHA256",
            KdfSaltB64 = Convert.ToBase64String(salt),
            KdfIter = iter,
            NonceB64 = Convert.ToBase64String(nonce),
            TagB64 = Convert.ToBase64String(tag),
            CiphertextB64 = Convert.ToBase64String(ct)
        };

        File.WriteAllText(_path, JsonSerializer.Serialize(env, new JsonSerializerOptions { WriteIndented = true }));
        Array.Clear(key, 0, key.Length);
    }

    public VaultData Load(string masterPassword)
    {
        var env = ReadEnvelope();

        var salt = Convert.FromBase64String(env.KdfSaltB64);
        var iter = env.KdfIter;
        var key = Crypto.DeriveKey(masterPassword, salt, iter);

        var nonce = Convert.FromBase64String(env.NonceB64);
        var tag = Convert.FromBase64String(env.TagB64);
        var ct = Convert.FromBase64String(env.CiphertextB64);

        var json = Crypto.AesGcmDecrypt(key, nonce, ct, tag);
        var vault = JsonSerializer.Deserialize<VaultData>(json) ?? new VaultData();

        Array.Clear(key, 0, key.Length);
        Array.Clear(json, 0, json.Length);

        return vault;
    }

    public void Save(string masterPassword, VaultData vault)
    {
        var env = ReadEnvelope();

        var salt = Convert.FromBase64String(env.KdfSaltB64);
        var iter = env.KdfIter;
        var key = Crypto.DeriveKey(masterPassword, salt, iter);

        var json = JsonSerializer.SerializeToUtf8Bytes(vault);
        var (nonce, ct, tag) = Crypto.AesGcmEncrypt(key, json);

        env.NonceB64 = Convert.ToBase64String(nonce);
        env.TagB64 = Convert.ToBase64String(tag);
        env.CiphertextB64 = Convert.ToBase64String(ct);

        File.WriteAllText(_path, JsonSerializer.Serialize(env, new JsonSerializerOptions { WriteIndented = true }));

        Array.Clear(key, 0, key.Length);
        Array.Clear(json, 0, json.Length);
    }

    private VaultFileEnvelope ReadEnvelope()
    {
        if (!Exists()) throw new FileNotFoundException("Vault not found", _path);
        var text = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<VaultFileEnvelope>(text)
               ?? throw new InvalidDataException("Corrupt vault file");
    }
}
