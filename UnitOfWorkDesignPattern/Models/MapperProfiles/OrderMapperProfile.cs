using AutoMapper;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;

namespace UnitOfWorkDesignPattern.Models.MapperProfiles;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<OrderDatabaseObject, Order>();
        CreateMap<Order, OrderDatabaseObject>();
    }
}