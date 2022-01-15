using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleemyRecruitmentTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Entities;
using Services.Contracts;
using Shared;

namespace CleemyRecruitmentTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IExpenseService _expenseService;

        public ExpenseController(IMapper mapper,
            IExpenseService expenseService)
        {
            this._mapper = mapper;
            this._expenseService = expenseService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateExpense([FromBody] ExpenseViewModel expenseViewModel)
        {
            Expense expense = this._mapper.Map<Expense>(expenseViewModel);
            Result result = await this._expenseService.CreateExpense(expense);

            if (result.Failure)
            {
                return this.BadRequest(string.Join(Environment.NewLine, result.Errors));
            }

            return this.Ok();
        }

        [HttpGet("user/{userId:long}")]
        public async Task<ActionResult<IEnumerable<GetExpenseViewModel>>> GetExpensesFromUserId([FromRoute] long userId,
            [FromQuery] string? sortProperty)
        {
            if (sortProperty != null && typeof(GetExpenseViewModel).GetProperty(sortProperty) is null)
            {
                return this.BadRequest($"You can't filter by \"{sortProperty}\"");
            }

            Result<IReadOnlyCollection<GetExpense>> expenses =
                await this._expenseService.GetExpensesFromUserIdSorted(userId, sortProperty);

            if (expenses.Failure)
            {
                return this.NotFound(string.Join(Environment.NewLine, expenses.Errors));
            }

            List<GetExpenseViewModel> expenseViewModels =
                expenses.Value.Select(expense => this._mapper.Map<GetExpenseViewModel>(expense)).ToList();

            return this.Ok(expenseViewModels);
        }
    }
}