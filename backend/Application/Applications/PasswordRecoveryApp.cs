using Application.DTO;
using Domain.Config;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Repository.Context;

namespace Application;

public class PasswordRecoveryApp : IPasswordRecoveryApp
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordRecoveryRepo _passwordRecoveryRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailApp _emailApp;
    private readonly CRMContext _context;
    private readonly EmailSettings _emailSettings;

    public PasswordRecoveryApp(
        IUserRepo userRepo,
        IPasswordRecoveryRepo passwordRecoveryRepo,
        IPasswordHasher passwordHasher,
        IEmailApp emailApp,
        CRMContext context,
        IOptions<EmailSettings> emailOptions)
    {
        _userRepo = userRepo;
        _passwordRecoveryRepo = passwordRecoveryRepo;
        _passwordHasher = passwordHasher;
        _emailApp = emailApp;
        _context = context;
        _emailSettings = emailOptions.Value;
    }

    public async Task RequestPasswordRecoveryAsync(ForgotPasswordRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email é obrigatório.");

        var normalizedEmail = request.Email.Trim();
        ValidateEmail(normalizedEmail);

        var user = await _userRepo.GetByEmailAsync(normalizedEmail);
        if (user == null || !user.IsActive)
            throw new KeyNotFoundException("Usuário não localizado.");

        var latestRequest = await _passwordRecoveryRepo.GetLatestByEmailAsync(normalizedEmail);
        if (latestRequest != null && latestRequest.CreatedAt > DateTime.UtcNow.AddMinutes(-1))
            throw new InvalidOperationException("Aguarde 1 minuto antes de solicitar nova recuperação de senha.");

        var token = Guid.NewGuid().ToString();
        var passwordRecovery = new PasswordRecovery
        {
            UserId = user.ID,
            Email = normalizedEmail,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        await _passwordRecoveryRepo.AddAsync(passwordRecovery);

        var resetUrl = BuildResetUrl(token);
        var subject = "Recuperação de senha";
        var body = $"Olá {user.Name},\n\nUse o link a seguir para redefinir sua senha:\n{resetUrl}\n\nO link expira em 10 minutos.";

        try
        {
            await _emailApp.SendEmailAsync(normalizedEmail, subject, body);
        }
        catch
        {
            await _passwordRecoveryRepo.DeleteAsync(passwordRecovery);
            throw;
        }
    }

    public async Task<PasswordRecovery> ValidateTokenAsync(ValidateTokenRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Token))
            throw new ArgumentException("Token é obrigatório.");

        var passwordRecovery = await _passwordRecoveryRepo.GetByTokenAsync(request.Token.Trim());
        if (passwordRecovery == null || !passwordRecovery.IsActive)
            throw new InvalidOperationException("Token inválido.");

        if (passwordRecovery.IsExpired())
            throw new InvalidOperationException("Token expirado.");

        return passwordRecovery;
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (request == null)
            throw new ArgumentException("Requisição inválida.");

        if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            throw new ArgumentException("Nova senha e confirmação são obrigatórias.");

        if (!string.Equals(request.NewPassword, request.ConfirmPassword, StringComparison.Ordinal))
            throw new ArgumentException("As senhas não coincidem.");

        ValidatePassword(request.NewPassword);

        var passwordRecovery = await ValidateTokenAsync(new ValidateTokenRequest { Token = request.Token });
        var user = await _userRepo.GetByIdAsync(passwordRecovery.UserId);
        if (user == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            user.SetPassword(request.NewPassword, _passwordHasher);
            passwordRecovery.MarkAsUsed();

            await _userRepo.UpdateAsync(user);
            await _passwordRecoveryRepo.UpdateAsync(passwordRecovery);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private string BuildResetUrl(string token)
    {
        if (string.IsNullOrWhiteSpace(_emailSettings.UrlFrontend))
            throw new InvalidOperationException("EmailSettings:UrlFrontend não foi configurado.");

        return $"{_emailSettings.UrlFrontend.TrimEnd('/')}/reset-password?token={token}";
    }

    private static void ValidateEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            if (!string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Email inválido.");
        }
        catch (FormatException)
        {
            throw new ArgumentException("Email inválido.");
        }
    }

    private static void ValidatePassword(string password)
    {
        if (password.Length < 8)
            throw new ArgumentException("A senha deve conter ao menos 8 caracteres.");

        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSpecial = password.Any(character => char.IsPunctuation(character) || char.IsSymbol(character));

        if (!hasUpper || !hasLower || !hasDigit || !hasSpecial)
            throw new ArgumentException("A senha deve conter letra maiúscula, letra minúscula, número e caractere especial.");
    }
}