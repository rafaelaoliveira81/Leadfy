using Repository.Context;

public abstract class BaseRepo
{
    protected readonly CRMContext _context;

    protected BaseRepo(CRMContext context)
    {
        _context = context;
    }
}