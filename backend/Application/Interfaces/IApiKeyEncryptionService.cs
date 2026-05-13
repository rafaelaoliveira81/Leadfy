namespace Application;

public interface IApiKeyEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherBase64);
}
