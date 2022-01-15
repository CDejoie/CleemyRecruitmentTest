using AutoMapper;
using Repositories.Dtos;
using Services.Abstractions.Entities;

namespace Repositories.Mappers
{
    public class RepositoriesProfile : Profile
    {
        public RepositoriesProfile()
        {
            CreateMap<Expense, DbExpense>();
            CreateMap<DbExpense, Expense>();

            CreateMap<DbUser, User>();
        }
    }
}