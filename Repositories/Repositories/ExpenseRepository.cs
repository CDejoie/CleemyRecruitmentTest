using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Dtos;
using Services.Abstractions.Entities;
using Services.Abstractions.Repositories.Contracts;

namespace Repositories.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpensesContext _context;
        private readonly IMapper _mapper;

        public ExpenseRepository(ExpensesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private DbExpense ConvertToDb(Expense expense)
        {
            return _mapper.Map<DbExpense>(expense);
        }

        private Expense ConvertFromDb(DbExpense dbExpense)
        {
            return _mapper.Map<Expense>(dbExpense);
        }

        public async Task CreateExpense(Expense expense)
        {
            DbExpense dbExpense = ConvertToDb(expense);
            await _context.Expenses.AddAsync(dbExpense);
            await _context.SaveChangesAsync();
        }

        // This method should be optimize because we use EF just for the request variable
        // I didn't succeed to find a way to integrate my dynamic sort in the query
        public async Task<IReadOnlyCollection<Expense>> GetAllFromUserIdSorted(long userId, string? sortProperty)
        {
            List<DbExpense>? request =
                await _context.Expenses.Where(dbExpense => dbExpense.UserId == userId).ToListAsync();
            if (sortProperty != null)
            {
                PropertyInfo? sortPropertyInfo = typeof(DbExpense).GetProperty(sortProperty);
                if (sortPropertyInfo != null)
                {
                    return request.OrderBy(expense => sortPropertyInfo.GetValue(expense, null))
                        .Select(ConvertFromDb).ToList().AsReadOnly();
                }
            }

            return request.Select(ConvertFromDb).ToList().AsReadOnly();
        }

        public async Task<bool> ExpenseAlreadyExist(Expense expense)
        {
            return await _context.Expenses.AnyAsync(dbExpense =>
                dbExpense.UserId == expense.UserId && dbExpense.Amount == expense.Amount &&
                dbExpense.Date == expense.Date);
        }
    }
}