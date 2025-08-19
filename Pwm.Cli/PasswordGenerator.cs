using System.Text;
using System.Security.Cryptography;

namespace Pwm;
public static class PasswordGenerator
{
    public static string Generate(int length = 20, bool upper = true, bool lower = true, bool digits = true, bool symbols = true)
    {
        const string U = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string L = "abcdefghijklmnopqrstuvwxyz";
        const string D = "01234567890";
        const string S = "!@#$%^&*()_+=-[]{}:;,.?";

        var pool = new StringBuilder();
        if (upper) pool.Append(U);
        if (lower) pool.Append(L);
        if (digits) pool.Append(D);
        if (symbols) pool.Append(S);
        if (pool.Length == 0) throw new ArgumentException("Select at least one character set!");

        var bytes = RandomNumberGenerator.GetBytes(length);
        var chars = new char[length];

        for (int i = 0; i < length; i++)
            chars[i] = pool[bytes[i] % pool.Length];

        return new string(chars);

    }
}
