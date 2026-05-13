using Domain.Entities;

namespace Application;

public interface IJwtApp
{
    string GenerateToken(User user);
}