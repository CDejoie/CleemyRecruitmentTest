using Microsoft.EntityFrameworkCore;
using Repositories.Configurations;
using Repositories.Dtos;

namespace Repositories
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext (DbContextOptions<ExpensesContext> options)
            : base(options)
        {
        }
        
        public DbSet<DbExpense> Expenses { get; set; }
        public DbSet<DbUser> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new DbUserConfiguration());
        }
    }
}