using System.Security.Cryptography;
using System.Text;

namespace School.Infrastructure.Security;

/// <summary>
/// SHA256 hashing helper, matching AgentPortal.Infrastructure.Security.Hash.
/// </summary>
public static class Hash
{
    public static string GenerateHash(string plainText)
    {
        string hashString = string.Empty;

        if (!string.IsNullOrEmpty(plainText))
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            hashString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        return hashString;
    }
}
