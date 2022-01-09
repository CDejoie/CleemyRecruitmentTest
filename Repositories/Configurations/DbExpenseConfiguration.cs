using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Dtos;

namespace Repositories.Configurations
{
    public class DbExpenseConfiguration : IEntityTypeConfiguration<DbExpense>
    {
        public void Configure(EntityTypeBuilder<DbExpense> builder)
        {
            builder.ToTable("Expense", "Cleemy");
            
            builder.Property(expense => expense.Amount).HasColumnType("decimal(19,4)");
            builder.Property(expense => expense.Comment).IsRequired();
        }
    }
}