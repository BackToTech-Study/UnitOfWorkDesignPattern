using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.UnitTests.Mock;

public class MockedDatabaseContextFactory : IDatabaseContextFactory
{
    private Mock<ApplicationDataContext> _dataContext = new Mock<ApplicationDataContext>();
    
    public void Dispose()
    {
    }
    
    private DbSet<T> GetQueryableDbSet<T>(int count) where T : class, IHasId, new()
    {
        var collection = new List<T>();
        for (var i = 0; i < count; i++)
        {
            collection.Add(new T { Id = i + 1 });
        }
        return collection.AsQueryable().BuildMockDbSet().Object;
    }
    
    public List<ProductDatabaseObject> SeedProducts(int count)
    {
        var productSet = GetQueryableDbSet<ProductDatabaseObject>(count);
        _dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                     .Returns(productSet);
        _dataContext.Setup(x => x.Products)
                    .Returns(productSet)
                    .Verifiable();  
        return productSet.ToList();
    }

    public List<OrderDatabaseObject> SeedOrders(int count)
    {
        var orderSet = GetQueryableDbSet<OrderDatabaseObject>(count);
        _dataContext.Setup(x => x.Set<OrderDatabaseObject>())
                    .Returns(orderSet);
        _dataContext.Setup(x => x.Orders)
                    .Returns(orderSet)
                    .Verifiable();
        return orderSet.ToList();
    }
    
    public List<OrderProductDatabaseObject> SeedOrderProducts(int count)
    {
        var orderProductSet = GetQueryableDbSet<OrderProductDatabaseObject>(count);
        _dataContext.Setup(x => x.Set<OrderProductDatabaseObject>())
                    .Returns(orderProductSet);
        _dataContext.Setup(x => x.OrderProducts)
                    .Returns(orderProductSet)
                    .Verifiable();
        return orderProductSet.ToList();
    }
    
    public ApplicationDataContext GetDataContext()
    {
        return _dataContext.Object;
    }
}