using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

public abstract class BaseRepo
{
    protected readonly CRMContext _context;

    protected BaseRepo(CRMContext context)
    {
        _context = context;
    }

    protected DbConnection GetConnection()
    {
        return _context.Database.GetDbConnection();
    }
}