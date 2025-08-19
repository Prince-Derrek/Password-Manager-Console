using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwm;
public class VaultFileEnvelope
{
    public string Version { get; set; } = "1";
    public string Aead { get; set; } = "AES-GCM-256";
    public string Kdf { get; set; } = "PBKDF2-SHA256";
    public string KdfSaltB64 { get; set; } = "";
    public string KdfIter { get; set; }
    public string NonceB64 { get; set; } = "";
    public string TagB64 { get; set; } = "";
    public string CiphertextB64 { get; set; } = "";
}
