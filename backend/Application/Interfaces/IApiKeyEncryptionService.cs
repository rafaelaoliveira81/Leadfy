namespace Application;

/// <summary>
/// Contrato para criptografia e descriptografia reversível de chaves de API.
/// Utiliza AES-256 com chave configurada em Encryption:Key (appsettings).
/// </summary>
public interface IApiKeyEncryptionService
{
    /// <summary>Criptografa o texto plano e retorna o resultado em Base64.</summary>
    string Encrypt(string plainText);

    /// <summary>Descriptografa um valor previamente criptografado com <see cref="Encrypt"/>.</summary>
    string Decrypt(string cipherBase64);
}
