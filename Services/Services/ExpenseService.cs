using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Contracts;
using Repositories.Dtos;
using Services.Contracts;
using Services.Entities;
using Services.Response;
using Shared;

namespace Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IMapper<Expense, DbExpense> _dbExpenseFromExpenseMapper;
        private readonly IMapper<DbExpense, Expense> _expenseFromDbExpenseMapper;
        private readonly IMapper<DbUser, User> _userFromDbUserMapper;

        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        public ExpenseService(IMapper<Expense, DbExpense> dbExpenseFromExpenseMapper,
            IMapper<DbExpense, Expense> expenseFromDbExpenseMapper,
            IMapper<DbUser, User> userFromDbUserMapper,
            IExpenseRepository expenseRepository, IUserRepository userRepository)
        {
            this._dbExpenseFromExpenseMapper = dbExpenseFromExpenseMapper;
            this._expenseFromDbExpenseMapper = expenseFromDbExpenseMapper;
            this._userFromDbUserMapper = userFromDbUserMapper;
            this._expenseRepository = expenseRepository;
            this._userRepository = userRepository;
        }

        public Result CreateExpense(Expense expense)
        {
            if (expense.Date > DateTime.Now)
            {
                return Result.Fail(ExpenseResponse.DateInTheFuture);
            }

            if (expense.Date < DateTime.Now.AddMonths(-3))
            {
                return Result.Fail(ExpenseResponse.DateIsTooFarAway);
            }

            if (string.IsNullOrWhiteSpace(expense.Comment))
            {
                return Result.Fail(ExpenseResponse.NoComment);
            }

            if (!this.TryToMatchCurrencyWithUserOne(expense, out Result result)) return result;

            if (this.IsDuplicateExpense(expense)) return Result.Fail(ExpenseResponse.ExpenseAlreadyExist);


            DbExpense dbExpense = this._dbExpenseFromExpenseMapper.Map(expense);
            this._expenseRepository.CreateExpense(dbExpense);
            return Result.Ok();
        }

        public Result<IReadOnlyCollection<GetExpense>> GetExpensesFromUserId(long userId)
        {
            if (!this.TryToGetUser(userId, out Result<User> userResult))
            {
                return Result.Fail<IReadOnlyCollection<GetExpense>>(userResult.Error);
            }

            IReadOnlyCollection<Expense> expenses = this.GetAllExpensesFromUserId(userId);

            var getExpenses = expenses.Select(expense => new GetExpense()
            {
                User = $"{userResult.Value.FirstName} {userResult.Value.LastName}",
                Date = expense.Date,
                Type = expense.Type,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Comment = expense.Comment,
            }).ToList().AsReadOnly();

            return Result.Ok<IReadOnlyCollection<GetExpense>>(getExpenses);
        }

        private bool TryToMatchCurrencyWithUserOne(Expense expense, out Result result)
        {
            if (!this.TryToGetUser(expense.UserId, out Result<User> userResult))
            {
                result = userResult;
                return false;
            }

            if (expense.Currency != userResult.Value.Currency)
            {
                result = Result.Fail(ExpenseResponse.NoMatchingCurrency);
                return false;
            }

            result = Result.Ok();
            return true;
        }

        private bool TryToGetUser(long userId, out Result<User> result)
        {
            DbUser dbExpenseUser = this._userRepository.GetUser(userId);
            if (dbExpenseUser is null)
            {
                result = Result.Fail<User>(UserResponse.UserNotFound);
                return false;
            }

            result = Result.Ok<User>(this._userFromDbUserMapper.Map(dbExpenseUser));
            return true;
        }

        private bool IsDuplicateExpense(Expense expense)
        {
            IReadOnlyCollection<Expense> userExpenses = this.GetAllExpensesFromUserId(expense.UserId);
            return userExpenses.Any(existingExpense =>
                existingExpense.Amount == expense.Amount && existingExpense.Date == expense.Date);
        }

        private IReadOnlyCollection<Expense> GetAllExpensesFromUserId(long userId)
        {
            IReadOnlyCollection<DbExpense> userDbExpenses = this._expenseRepository.GetAllFromUserId(userId);
            return userDbExpenses.Select(dbExpense => this._expenseFromDbExpenseMapper.Map(dbExpense)).ToList()
                .AsReadOnly();
        }
    }
}