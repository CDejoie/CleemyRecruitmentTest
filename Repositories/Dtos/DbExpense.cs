using System;
using Shared.Enums;

namespace Repositories.Dtos
{
    public class DbExpense
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType Type { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string Comment { get; set; } = string.Empty;
        
        public DbUser User { get; set; } = null!;
    }
}