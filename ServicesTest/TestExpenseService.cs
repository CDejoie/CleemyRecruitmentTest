using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq;
using Repositories.Contracts;
using Repositories.Dtos;
using Services.Entities;
using Services.Response;
using Services.Services;
using Shared;
using Shared.Enums;
using Xunit;

namespace ServicesTest
{
    public class TestExpenseService : IDisposable
    {
        private readonly MockRepository _mockFactory;

        private readonly Mock<IMapper<Expense, DbExpense>> _mockDbExpenseFromExpenseMapper;
        private readonly Mock<IMapper<DbExpense, Expense>> _mockExpenseFromDbExpenseMapper;
        private readonly Mock<IMapper<DbUser, User>> _mockUserFromDbUserMapper;

        private readonly Mock<IExpenseRepository> _mockExpenseRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly Fixture _fixture;

        private readonly ExpenseService _sut;

        public TestExpenseService()
        {
            this._mockFactory = new MockRepository(MockBehavior.Strict);
            this._mockDbExpenseFromExpenseMapper = this._mockFactory.Create<IMapper<Expense, DbExpense>>();
            this._mockExpenseFromDbExpenseMapper = this._mockFactory.Create<IMapper<DbExpense, Expense>>();
            this._mockUserFromDbUserMapper = this._mockFactory.Create<IMapper<DbUser, User>>();
            this._mockExpenseRepository = this._mockFactory.Create<IExpenseRepository>();
            this._mockUserRepository = this._mockFactory.Create<IUserRepository>();

            this._fixture = new Fixture();

            this._sut = new ExpenseService(
                this._mockDbExpenseFromExpenseMapper.Object,
                this._mockExpenseFromDbExpenseMapper.Object,
                this._mockUserFromDbUserMapper.Object,
                this._mockExpenseRepository.Object,
                this._mockUserRepository.Object);
        }

        public void Dispose()
        {
            this._mockFactory.VerifyAll();
        }

        [Fact]
        public void TestExpenseService_GivenDateInTheFuture_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddDays(1))
                .Create();
            
            // Act
            Result result = this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(ExpenseResponse.DateInTheFuture);
        }
        
        [Fact]
        public void TestExpenseService_GivenMoreThanThreeMonthDate_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-4))
                .Create();
            
            // Act
            Result result = this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(ExpenseResponse.DateIsTooFarAway);
        }
        
        [Fact]
        public void TestExpenseService_GivenNoComment_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .Without(expense => expense.Comment)
                .Create();
            
            // Act
            Result result = this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(ExpenseResponse.NoComment);
        }
        
        [Fact]
        public void TestExpenseService_GivenUserIdThatDoesntExist_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .Create();

            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns((DbUser)null);
                
            // Act
            Result result = this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(UserResponse.UserNotFound);
        }
        
        [Theory]
        [InlineData(Currency.RUB, Currency.USD)]
        [InlineData(Currency.USD, Currency.RUB)]
        public void TestExpenseService_GivenCurrencyThatDoesntMatchWithTheUserOne_ThenReturnFailure(
            Currency expenseCurrency, Currency userCurrency)
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Currency, expenseCurrency)
                .Create();

            DbUser dbUser = this._fixture.Build<DbUser>()
                .With(user => user.Currency, userCurrency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(dbUser);

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, dbUser.Currency)
                .Create();
            this._mockUserFromDbUserMapper
                .Setup(mapper => mapper.Map(dbUser))
                .Returns(user);
                
            // Act
            Result result = this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(ExpenseResponse.NoMatchingCurrency);
        }
        
        [Fact]
        public void TestExpenseService_GivenExpenseThatAlreadyExist_ThenReturnFailure()
        {
            // Arrange
            Expense newExpense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .Create();
        
            DbUser dbUser = this._fixture.Build<DbUser>()
                .With(user => user.Currency, newExpense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(newExpense.UserId))
                .Returns(dbUser);
        
            User user = this._fixture.Build<User>()
                .With(user => user.Currency, dbUser.Currency)
                .Create();
            this._mockUserFromDbUserMapper
                .Setup(mapper => mapper.Map(dbUser))
                .Returns(user);

            DbExpense dbExpense = this._fixture.Build<DbExpense>()
                .With(expense => expense.Date, newExpense.Date)
                .With(expense => expense.Amount, newExpense.Amount)
                .Create();
            this._mockExpenseRepository
                .Setup(repository => repository.GetAllFromUserId(newExpense.UserId))
                .Returns(new List<DbExpense>() { dbExpense });
            
            Expense existingExpense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, dbExpense.Date)
                .With(expense => expense.Amount, dbExpense.Amount)
                .Create();
            this._mockExpenseFromDbExpenseMapper
                .Setup(mapper => mapper.Map(dbExpense))
                .Returns(existingExpense);
                
            // Act
            Result result = this._sut.CreateExpense(newExpense);
        
            // Assert
            result.Failure.Should().BeTrue();
            result.Error.Should().Be(ExpenseResponse.ExpenseAlreadyExist);
        }
        
        [Fact]
        public void TestExpenseService_GivenGoodExpense_ThenReturnSuccess()
        {
            // Arrange
            Expense newExpense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .Create();
        
            DbUser dbUser = this._fixture.Build<DbUser>()
                .With(user => user.Currency, newExpense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(newExpense.UserId))
                .Returns(dbUser);
        
            User user = this._fixture.Build<User>()
                .With(user => user.Currency, dbUser.Currency)
                .Create();
            this._mockUserFromDbUserMapper
                .Setup(mapper => mapper.Map(dbUser))
                .Returns(user);

            DbExpense dbExpense = this._fixture.Build<DbExpense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-2))
                .With(expense => expense.Amount, 29.99m)
                .Create();
            this._mockExpenseRepository
                .Setup(repository => repository.GetAllFromUserId(newExpense.UserId))
                .Returns(new List<DbExpense>() { dbExpense });
            
            Expense existingExpense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, dbExpense.Date)
                .With(expense => expense.Amount, dbExpense.Amount)
                .Create();
            this._mockExpenseFromDbExpenseMapper
                .Setup(mapper => mapper.Map(dbExpense))
                .Returns(existingExpense);
            
            DbExpense newDbExpense = this._fixture.Build<DbExpense>()
                .With(expense => expense.Date, newExpense.Date)
                .With(expense => expense.Amount, newExpense.Amount)
                .Create();
            this._mockDbExpenseFromExpenseMapper
                .Setup(mapper => mapper.Map(newExpense))
                .Returns(newDbExpense);

            this._mockExpenseRepository
                .Setup(repository => repository.CreateExpense(newDbExpense));
                
            // Act
            Result result = this._sut.CreateExpense(newExpense);
        
            // Assert
            result.Success.Should().BeTrue();
        }
    }
}