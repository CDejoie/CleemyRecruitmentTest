using System;
using System.Collections.Generic;
using System.Linq;
using CleemyRecruitmentTest.Response;
using CleemyRecruitmentTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Entities;
using Shared;
using Shared.Enums;

namespace CleemyRecruitmentTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMapper<GetExpense, GetExpenseViewModel> _getExpenseViewModelFromGetExpenseMapper;

        private readonly IExpenseService _expenseService;

        public ExpenseController(IMapper<GetExpense, GetExpenseViewModel> getExpenseViewModelFromGetExpenseMapper,
            IExpenseService expenseService)
        {
            this._getExpenseViewModelFromGetExpenseMapper = getExpenseViewModelFromGetExpenseMapper;
            this._expenseService = expenseService;
        }

        [HttpPost]
        public ActionResult CreateExpense([FromBody] ExpenseViewModel expenseViewModel)
        {
            bool viewModelCanBeConvertIntoEntity =
                this.TryToConvertExpenseViewModelIntoExpense(expenseViewModel, out Result<Expense> convertedResult);
            if (!viewModelCanBeConvertIntoEntity)
            {
                return this.BadRequest(convertedResult.Error);
            }

            Result result = this._expenseService.CreateExpense(convertedResult.Value);

            if (result.Failure)
            {
                return this.BadRequest(result.Error);
            }

            return this.Ok();
        }

        [HttpGet("user/{userId:long}")]
        public ActionResult<IEnumerable<GetExpenseViewModel>> GetExpensesFromUserId([FromRoute] long userId,
            [FromQuery] bool sortByAmount = false, bool sortByDate = false)
        {
            if (sortByAmount && sortByDate)
            {
                return this.BadRequest("You can't filter by amount and date");
            }

            Result<IReadOnlyCollection<GetExpense>> expenses = this._expenseService.GetExpensesFromUserId(userId);

            if (expenses.Failure)
            {
                return this.NotFound(expenses.Error);
            }

            List<GetExpenseViewModel> expenseViewModels =
                expenses.Value.Select(expense => this._getExpenseViewModelFromGetExpenseMapper.Map(expense)).ToList();

            if (sortByAmount)
            {
                return this.Ok(expenseViewModels.OrderBy(expense => expense.Amount));
            }

            if (sortByDate)
            {
                return this.Ok(expenseViewModels.OrderBy(expense => expense.Date));
            }

            return this.Ok(expenseViewModels);
        }

        private bool TryToConvertExpenseViewModelIntoExpense(ExpenseViewModel expenseViewModel,
            out Result<Expense> result)
        {
            bool canConvertType = Enum.TryParse(expenseViewModel.Type, out ExpenseType expenseType);
            if (!canConvertType)
            {
                result = Result.Fail<Expense>(ConvertExpenseViewModelIntoExpenseResponse.WrongType);
                return false;
            }

            bool canConvertCurrency = Enum.TryParse(expenseViewModel.Currency, out Currency expenseCurrency);
            if (!canConvertCurrency)
            {
                result = Result.Fail<Expense>(ConvertExpenseViewModelIntoExpenseResponse.WrongCurrency);
                return false;
            }

            result = Result.Ok(new Expense
            {
                UserId = expenseViewModel.UserId,
                Date = expenseViewModel.Date,
                Type = expenseType,
                Amount = expenseViewModel.Amount,
                Currency = expenseCurrency,
                Comment = expenseViewModel.Comment,
            });
            return true;
        }
    }
}