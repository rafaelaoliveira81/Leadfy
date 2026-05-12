using Domain.Entities;

namespace Application;

public interface IJwtApp
{
    string GenerateToken(User user, IReadOnlyCollection<string>? permissions = null);
}