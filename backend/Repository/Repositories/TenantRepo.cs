using Domain.Entities;
using Repository.Context;

namespace Repository.Repositories;

public class TenantRepository : BaseRepository<Tenant>, ITenantRepo
{
    public TenantRepository(CRMContext context) : base(context)
    {
    }
}