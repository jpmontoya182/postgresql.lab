using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEF.Models;

namespace MinimalAPIPostgreSQLEF.Data;

public class OfficeDBContext: DbContext
{
    public OfficeDBContext(DbContextOptions<OfficeDBContext> options) : base(options)
    {
        
    }
    public DbSet<Employee> Employees => Set<Employee>();
}
