using System;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace CleemyRecruitmentTest.ViewModels
{
    public class ExpenseViewModel
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "UserId must be defined")]
        public long UserId { get; set; }
        
        public DateTime Date { get; set; }
        
        [EnumDataType(typeof(ExpenseType))]
        public string Type { get; set; } = string.Empty;
        
        [Range(0.01, 999999999999999.99, ErrorMessage = "Amount should exist and be upper than 0.01")]
        public decimal Amount { get; set; }
        
        [EnumDataType(typeof(Currency))]
        public string Currency { get; set; } = string.Empty;
        
        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}