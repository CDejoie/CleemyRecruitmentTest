namespace Shared
{
    // It should be replace by AutoMapper nugget package
    public interface IMapper<TFrom, TTo>
    {
        TTo Map(TFrom obj);
    }
}