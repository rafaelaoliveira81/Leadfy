using System.Security.Cryptography;

namespace Application
{
    /// <summary>
    /// Implementação de um gerador e verificador de hashes de senha.
    /// Utiliza o algoritmo PBKDF2 (Rfc2898DeriveBytes) com SHA256 para garantir a segurança.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// O número de iterações a serem executadas pelo algoritmo PBKDF2.
        /// Um valor maior aumenta a segurança contra ataques de força bruta, mas consome mais CPU.
        /// </summary>
        private const int Iterations = 10000;

        /// <summary>
        /// O tamanho, em bytes, do salt gerado aleatoriamente para cada senha.
        /// O salt garante que senhas iguais resultem em hashes diferentes.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// O tamanho, em bytes, da chave derivada (o hash da senha em si).
        /// </summary>
        private const int KeySize = 32;

        /// <summary>
        /// Gera um hash criptográfico seguro a partir de uma senha em texto plano.
        /// </summary>
        /// <param name="password">A senha em texto plano que será submetida ao hash.</param>
        /// <returns>Uma string formatada contendo as iterações, o salt em Base64 e a chave derivada em Base64, separados por pontos.</returns>
        public string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Iterations}.{salt}.{key}";
        }

        /// <summary>
        /// Verifica se uma senha em texto plano corresponde a um hash previamente gerado.
        /// </summary>
        /// <param name="hash">A string do hash gerado pelo método <see cref="Hash(string)"/>.</param>
        /// <param name="password">A senha em texto plano a ser verificada.</param>
        /// <returns>Retorna <c>true</c> se a senha estiver correta; caso contrário, <c>false</c>.</returns>
        public bool Verify(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                return false;
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var keyToCheck = algorithm.GetBytes(KeySize);

            // Compara as chaves de forma segura contra ataques de temporização (timing attacks)
            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }
    }
}