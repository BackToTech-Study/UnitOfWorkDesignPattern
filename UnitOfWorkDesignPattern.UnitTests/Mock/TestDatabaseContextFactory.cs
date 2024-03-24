using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.UnitTests.Mock;

public class TestDatabaseContextFactory : IDatabaseContextFactory
{
    private ApplicationDataContext _dbContext;
    private List<long> _productIds = new();
    private List<long> _orderIds = new();
    private List<long> _orderProductIds = new();
    
    public void Dispose()
    {
        foreach (var id in _orderProductIds)
        {
            var orderProduct = _dbContext.OrderProducts.Find(id);
            if (orderProduct != null)
            {
                _dbContext.OrderProducts.Remove(orderProduct);
            }
        }
        
        foreach (var id in _productIds)
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
            }
        }
        
        foreach (var id in _orderIds)
        {
            var order = _dbContext.Orders.Find(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
            }
        }
    }
    
    public TestDatabaseContextFactory(string configFile = "appsettings.Test.json")
    {
        var config = new ConfigurationBuilder()
                                    .AddJsonFile(configFile)
                                    .AddEnvironmentVariables() 
                                    .Build();
        var connectionString = config.GetConnectionString("ApplicationDatabase");
        _dbContext = new ApplicationDataContext(new DbContextOptionsBuilder<ApplicationDataContext>()
                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                        .Options
        );
    }
    
    public List<ProductDatabaseObject> SeedProducts(int count)
    {
        var products = new List<ProductDatabaseObject>();
        for (var i = 0; i < count; i++)
        {
            products.Add(new ProductDatabaseObject { Id = i + 1 });
        }
        
        using var transaction = _dbContext.Database.BeginTransaction();
        _dbContext.Products.AddRange(products);
        _dbContext.SaveChanges();
        transaction.Commit();
        
        _productIds = products.Select(x => x.Id).ToList();
        return products;
    }

    public List<OrderDatabaseObject> SeedOrders(int count)
    {
        var orders = new List<OrderDatabaseObject>();
        for (var i = 0; i < count; i++)
        {
            orders.Add(new OrderDatabaseObject { Id = i + 1 });
        }
        
        using var transaction = _dbContext.Database.BeginTransaction();
        _dbContext.Orders.AddRange(orders);
        _dbContext.SaveChanges();
        transaction.Commit();
        
        _orderIds = orders.Select(x => x.Id).ToList();
        return orders;
    }

    public List<OrderProductDatabaseObject> SeedOrderProducts(int count)
    {
        var orderProducts = new List<OrderProductDatabaseObject>();
        for (var i = 0; i < count; i++)
        {
            orderProducts.Add(new OrderProductDatabaseObject { Id = i + 1 });
        }
        
        using var transaction = _dbContext.Database.BeginTransaction();
        _dbContext.OrderProducts.AddRange(orderProducts);
        _dbContext.SaveChanges();
        transaction.Commit();
        
        _orderProductIds = orderProducts.Select(x => x.Id).ToList();
        return orderProducts;
    }

    public ApplicationDataContext GetDataContext()
    {
        return _dbContext;
    }
}