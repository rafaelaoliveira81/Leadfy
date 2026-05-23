using Domain.Entities;

public interface IJwtApp
{
    string GenerateToken(User user);
}