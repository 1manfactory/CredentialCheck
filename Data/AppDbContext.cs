using CredentialCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace CredentialCheck.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<QualificationCase> QualificationCases => Set<QualificationCase>();
}
