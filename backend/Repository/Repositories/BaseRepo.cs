using System.Data.Common;
using Microsoft.Data.SqlClient;
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
        var connectionString = _context.Database.GetConnectionString();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ConnectionString não foi inicializada no CRMContext.");

        return new SqlConnection(connectionString);
    }
}