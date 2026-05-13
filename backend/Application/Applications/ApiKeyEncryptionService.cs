using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application;

public class ApiKeyEncryptionService : IApiKeyEncryptionService
{
    private readonly byte[] _key;
    public ApiKeyEncryptionService(IConfiguration config)
    {
        var keyBase64 = config["Encryption:Key"]
            ?? throw new InvalidOperationException("Chave de criptografia não configurada. Defina 'Encryption:Key' no appsettings.");

        _key = Convert.FromBase64String(keyBase64);

        if (_key.Length != 32)
            throw new InvalidOperationException("A chave de criptografia deve ter exatamente 32 bytes (AES-256).");
    }
    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipher = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = new byte[aes.IV.Length + cipher.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(cipher, 0, result, aes.IV.Length, cipher.Length);

        return Convert.ToBase64String(result);
    }
    public string Decrypt(string cipherBase64)
    {
        var fullCipher = Convert.FromBase64String(cipherBase64);

        using var aes = Aes.Create();
        aes.Key = _key;

        var ivLength = aes.BlockSize / 8;
        var iv = new byte[ivLength];
        Buffer.BlockCopy(fullCipher, 0, iv, 0, ivLength);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        var cipher = new byte[fullCipher.Length - ivLength];
        Buffer.BlockCopy(fullCipher, ivLength, cipher, 0, cipher.Length);

        var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
