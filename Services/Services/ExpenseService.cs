using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Abstractions.Entities;
using Services.Abstractions.Repositories.Contracts;
using Services.Contracts;
using Services.Errors;
using Services.Factory;
using Shared;

namespace Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        public ExpenseService(IExpenseRepository expenseRepository, IUserRepository userRepository)
        {
            this._expenseRepository = expenseRepository;
            this._userRepository = userRepository;
        }

        public async Task<Result> CreateExpense(Expense expense)
        {
            Result result = new Result();
            if (expense.Date > DateTime.Now)
            {
                Result.AddError(result, ExpenseError.DateInTheFuture);
            }

            if (expense.Date < DateTime.Now.AddMonths(-3))
            {
                Result.AddError(result, ExpenseError.DateIsTooFarAway);
            }

            if (string.IsNullOrWhiteSpace(expense.Comment))
            {
                Result.AddError(result, ExpenseError.NoComment);
            }

            Result currencyResult = await this.TryToMatchCurrencyWithUserOne(expense);
            if (currencyResult.Failure)
            {
                Result.AddErrors(result, currencyResult.Errors);
            }

            if (await this._expenseRepository.ExpenseAlreadyExist(expense))
            {
                Result.AddError(result, ExpenseError.ExpenseAlreadyExist);
            }

            if (result.Success)
            {
                await this._expenseRepository.CreateExpense(expense);
            }

            return result;
        }

        public async Task<Result<IReadOnlyCollection<GetExpense>>> GetExpensesFromUserIdSorted(long userId,
            string? sortProperty)
        {
            Result<User> userResult = await this.GetUser(userId);
            if (userResult.Failure)
            {
                return Result.Fail<IReadOnlyCollection<GetExpense>>(userResult.Errors.Single());
            }

            IReadOnlyCollection<Expense> expenses = await
                this._expenseRepository.GetAllFromUserIdSorted(userId, sortProperty);

            var getExpenses = expenses.Select(expense => GetExpenseFactory.CreateGetExpense(expense, userResult.Value))
                .ToList().AsReadOnly();

            return Result.Ok<IReadOnlyCollection<GetExpense>>(getExpenses);
        }

        private async Task<Result> TryToMatchCurrencyWithUserOne(Expense expense)
        {
            Result<User> userResult = await this.GetUser(expense.UserId);
            if (userResult.Failure)
            {
                return userResult;
            }

            if (expense.Currency != userResult.Value.Currency)
            {
                return Result.Fail(ExpenseError.NoMatchingCurrency);
            }

            return Result.Ok();
        }

        private async Task<Result<User>> GetUser(long userId)
        {
            User? expenseUser = await this._userRepository.GetUser(userId);
            if (expenseUser is null)
            {
                return Result.Fail<User>(UserError.UserNotFound);
            }

            return Result.Ok<User>(expenseUser);
        }
    }
}