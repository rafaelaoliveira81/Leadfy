using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application;

/// <summary>
/// Serviço de criptografia AES-256 para armazenamento seguro de chaves de API.
/// A chave de criptografia deve ser configurada em <c>Encryption:Key</c> como string Base64
/// de exatamente 32 bytes (256 bits).
/// </summary>
public class ApiKeyEncryptionService : IApiKeyEncryptionService
{
    private readonly byte[] _key;

    /// <summary>
    /// Inicializa o serviço lendo a chave AES das configurações da aplicação.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a chave não está configurada ou possui tamanho inválido.
    /// </exception>
    public ApiKeyEncryptionService(IConfiguration config)
    {
        var keyBase64 = config["Encryption:Key"]
            ?? throw new InvalidOperationException("Chave de criptografia não configurada. Defina 'Encryption:Key' no appsettings.");

        _key = Convert.FromBase64String(keyBase64);

        if (_key.Length != 32)
            throw new InvalidOperationException("A chave de criptografia deve ter exatamente 32 bytes (AES-256).");
    }

    /// <summary>
    /// Criptografa o texto plano usando AES-256-CBC.
    /// O IV gerado aleatoriamente é prefixado ao cipher antes de codificar em Base64.
    /// </summary>
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

    /// <summary>
    /// Descriptografa um valor gerado por <see cref="Encrypt"/>.
    /// Os primeiros 16 bytes do payload decodificado são o IV; o restante é o cipher.
    /// </summary>
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
