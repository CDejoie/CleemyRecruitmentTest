using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Services.Abstractions.Entities;
using Services.Abstractions.Repositories.Contracts;
using Services.Errors;
using Services.Services;
using Shared;
using Shared.Enums;
using Xunit;

namespace ServicesTest
{
    public class TestExpenseService : IDisposable
    {
        private readonly MockRepository _mockFactory;

        private readonly Mock<IExpenseRepository> _mockExpenseRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly Fixture _fixture;

        private readonly ExpenseService _sut;

        public TestExpenseService()
        {
            this._mockFactory = new MockRepository(MockBehavior.Strict);
            this._mockExpenseRepository = this._mockFactory.Create<IExpenseRepository>();
            this._mockUserRepository = this._mockFactory.Create<IUserRepository>();

            this._fixture = new Fixture();

            this._sut = new ExpenseService(
                this._mockExpenseRepository.Object,
                this._mockUserRepository.Object);
        }

        public void Dispose()
        {
            this._mockFactory.VerifyAll();
        }

        [Fact]
        public async void TestExpenseService_GivenDateInTheFuture_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(1))
                .With(expense => expense.Amount, 19.99m)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, expense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(ExpenseError.DateInTheFuture);
        }

        [Fact]
        public async void TestExpenseService_GivenMoreThanThreeMonthDate_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-4))
                .With(expense => expense.Amount, 19.99m)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, expense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(ExpenseError.DateIsTooFarAway);
        }

        [Fact]
        public async void TestExpenseService_GivenNoComment_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .Without(expense => expense.Comment)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, expense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(ExpenseError.NoComment);
        }

        [Fact]
        public async void TestExpenseService_GivenUserIdThatDoesntExist_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .Create();
            
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult((User)null));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(UserError.UserNotFound);
        }

        [Theory]
        [InlineData(Currency.RUB, Currency.USD)]
        [InlineData(Currency.USD, Currency.RUB)]
        public async void TestExpenseService_GivenCurrencyThatDoesntMatchWithTheUserOne_ThenReturnFailure(
            Currency expenseCurrency, Currency userCurrency)
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .With(expense => expense.Currency, expenseCurrency)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, userCurrency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(ExpenseError.NoMatchingCurrency);
        }

        [Fact]
        public async void TestExpenseService_GivenExpenseThatAlreadyExist_ThenReturnFailure()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, expense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(true));

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Failure.Should().BeTrue();
            result.Errors.Single().Should().Be(ExpenseError.ExpenseAlreadyExist);
        }

        [Fact]
        public async void TestExpenseService_GivenGoodExpense_ThenReturnSuccess()
        {
            // Arrange
            Expense expense = this._fixture.Build<Expense>()
                .With(expense => expense.Date, DateTime.Now.AddMonths(-1))
                .With(expense => expense.Amount, 19.99m)
                .Create();

            User user = this._fixture.Build<User>()
                .With(user => user.Currency, expense.Currency)
                .Create();
            this._mockUserRepository
                .Setup(repository => repository.GetUser(expense.UserId))
                .Returns(Task.FromResult(user));

            this._mockExpenseRepository
                .Setup(repository => repository.ExpenseAlreadyExist(expense))
                .Returns(Task.FromResult(false));

            this._mockExpenseRepository
                .Setup(repository => repository.CreateExpense(expense))
                .Returns(Task.CompletedTask);

            // Act
            Result result = await this._sut.CreateExpense(expense);

            // Assert
            result.Success.Should().BeTrue();
        }
    }
}