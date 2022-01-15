using System;

namespace CleemyRecruitmentTest.ViewModels
{
    public class GetExpenseViewModel
    {
        public string User { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Type { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;
    }
}