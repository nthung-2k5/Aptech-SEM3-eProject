using System.Security.Cryptography;
using System.Text;
using GiveAID.Services.Abstractions;
using Konscious.Security.Cryptography;

namespace GiveAID.Services;

public class PasswordService : IPasswordService
{
    // Configuration constants matching OWASP / robust baseline profiles
    private const int SaltSize = 16; // 128-bit salt
    private const int HashSize = 32; // 256-bit output hash
    private const int Iterations = 3; // Time cost
    private const int MemorySize = 65536; // 64 MB memory cost
    private const int Parallelism = 4; // Number of threads to use

    public string HashPassword(string password)
    {
        // 1. Generate a cryptographically secure random salt
        byte[] salt = new byte[SaltSize];

        using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(salt); }

        // 2. Initialize and configure Argon2id
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (var argon2 = new Argon2id(passwordBytes))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = Parallelism;
            argon2.Iterations = Iterations;
            argon2.MemorySize = MemorySize;

            // 3. Get the computed hash bytes
            byte[] hash = argon2.GetBytes(HashSize);

            // 4. Combine Salt and Hash into a single array for easier storage
            byte[] combinedBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, combinedBytes, 0, SaltSize);
            Array.Copy(hash, 0, combinedBytes, SaltSize, HashSize);

            // 5. Convert to Base64 to store safely as a string in the database
            return Convert.ToBase64String(combinedBytes);
        }
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            // 1. Decode the stored combined string back to bytes
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            // 2. Extract the salt from the combined array
            byte[] salt = new byte[SaltSize];
            Array.Copy(combinedBytes, 0, salt, 0, SaltSize);

            // 3. Extract the original hash from the combined array
            byte[] originalHash = new byte[HashSize];
            Array.Copy(combinedBytes, SaltSize, originalHash, 0, HashSize);

            // 4. Re-hash the provided password attempt using the extracted salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            using var argon2 = new Argon2id(passwordBytes);

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = Parallelism;
            argon2.Iterations = Iterations;
            argon2.MemorySize = MemorySize;

            byte[] verificationHash = argon2.GetBytes(HashSize);

            // 5. Compare the hashes using a fixed-time comparison to prevent side-channel attacks
            return CryptographicOperations.FixedTimeEquals(originalHash, verificationHash);
        }
        catch { return false; }
    }
}
