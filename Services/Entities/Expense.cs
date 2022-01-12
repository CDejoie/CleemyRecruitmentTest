using System;
using Shared.Enums;

namespace Services.Entities
{
    public class Expense
    {
        public long Id { get; set; }
        
        public long UserId { get; set; }

        public DateTime Date { get; set; }
        
        public ExpenseType Type { get; set; }
        
        public decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
        
        public string Comment { get; set; }
    }
}