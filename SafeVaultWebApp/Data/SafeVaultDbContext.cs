using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Controllers;

namespace SafeVaultWebApp.Data
{
    public class SafeVaultDbContext : IdentityDbContext<User>
    {
        public SafeVaultDbContext(DbContextOptions<SafeVaultDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Add DbSet<FinancialRecord> FinancialRecords { get; set; } when you create the model
    }
}
