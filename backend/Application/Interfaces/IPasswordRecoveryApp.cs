using Application.DTO;
using Domain.Entities;

public interface IPasswordRecoveryApp
{
    Task RequestPasswordRecoveryAsync(ForgotPasswordRequest request);
    Task<PasswordRecovery> ValidateTokenAsync(ValidateTokenRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}