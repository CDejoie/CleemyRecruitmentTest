using System;

namespace CleemyRecruitmentTest.ViewModels
{
    public class GetExpenseViewModel
    {
        public string User { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Comment { get; set; }
    }
}