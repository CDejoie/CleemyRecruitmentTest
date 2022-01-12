using System;
using System.ComponentModel.DataAnnotations;

namespace CleemyRecruitmentTest.ViewModels
{
    public class ExpenseViewModel
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "UserId must be defined")]
        public long UserId { get; set; }
        
        public DateTime Date { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Range(0.01, 999999999999999.99, ErrorMessage = "Amount should exist and be upper than 0.01")]
        public decimal Amount { get; set; }
        
        [Required]
        public string Currency { get; set; }
        
        [Required]
        public string Comment { get; set; }
    }
}