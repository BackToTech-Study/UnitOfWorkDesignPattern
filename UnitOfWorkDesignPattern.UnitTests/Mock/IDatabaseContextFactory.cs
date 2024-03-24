using Microsoft.EntityFrameworkCore;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.UnitTests.Mock;

public interface IDatabaseContextFactory : IDisposable
{
    public List<ProductDatabaseObject> SeedProducts(int count);
    public List<OrderDatabaseObject> SeedOrders(int count);
    public List<OrderProductDatabaseObject> SeedOrderProducts(int count);
    public ApplicationDataContext GetDataContext();
}