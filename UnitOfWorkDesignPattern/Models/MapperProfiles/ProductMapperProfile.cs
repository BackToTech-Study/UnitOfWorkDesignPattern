using AutoMapper;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;

namespace UnitOfWorkDesignPattern.Models.MapperProfiles;

public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<ProductDatabaseObject, Product>();
        CreateMap<Product, ProductDatabaseObject>();
    }
}