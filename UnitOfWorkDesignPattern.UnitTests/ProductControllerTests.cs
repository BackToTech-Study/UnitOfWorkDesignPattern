using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UnitOfWorkDesignPattern.Controllers;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;
using UnitOfWorkDesignPattern.Models.MapperProfiles;

namespace UnitOfWorkDesignPattern.UnitTests;

public class ProductControllerTests
{
    private readonly IMapper _mapper;
    public ProductControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMapperProfile>();
        });
        _mapper = new Mapper(mapperConfiguration);
    }
    
    private Mock<DbSet<T>> GetQueryableDbSet<T>(int count) where T : class, IHasId, new()
    {
        var collection = new List<T>();
        for (var i = 0; i < count; i++)
        {
            collection.Add(new T { Id = i + 1 });
        }
        return collection.AsQueryable().BuildMockDbSet();
    }
    
    private Mock<ApplicationDataContext> GetMockDataContext(
        Mock<DbSet<ProductDatabaseObject>> productSet,
        Mock<DbSet<OrderDatabaseObject>> orderSet,
        Mock<DbSet<OrderProductDatabaseObject>> orderProductSet)
    {
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(productSet.Object);
        dataContext.Setup(x => x.Products)
                    .Returns(productSet.Object)
                    .Verifiable();
        dataContext.Setup(x => x.Set<OrderDatabaseObject>())
                    .Returns(orderSet.Object);
        dataContext.Setup(x => x.Orders)
                    .Returns(orderSet.Object)    
                    .Verifiable();
        dataContext.Setup(x => x.Set<OrderProductDatabaseObject>())
                    .Returns(orderProductSet.Object);
        dataContext.Setup(x => x.OrderProducts)
                    .Returns(orderProductSet.Object)
                    .Verifiable();
        return dataContext;
    }
    
    [Fact]
    public void GetCollection_EmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        var mock = GetQueryableDbSet<ProductDatabaseObject>(0);
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var unitOfWork = new Mock<UnitOfWork>(dataContext.Object);
        var productController = new ProductController(unitOfWork.Object, _mapper);
        var pageNumber = 0;
        var pageSize = 10;
        
        // Act
        var result = productController.GetAll(pageNumber, pageSize).Result as OkObjectResult;
        
        // Assert
        Assert.NotNull(result?.Value);
        var collection = result.Value as ProductCollection;
        Assert.NotNull(collection);
        Assert.Empty(collection.Products);
        Assert.Equal(0, collection.ElementCount);
        Assert.Equal(pageNumber, collection.Page.PageNumber);
        Assert.Equal(pageSize, collection.Page.PageSize);
    }
    
    [Fact]
    public void GetCollection_PopulatedDatabase_ReturnsCollection()
    {
        // Arrange
        var productSet = GetQueryableDbSet<ProductDatabaseObject>(2);
        var orderSet = GetQueryableDbSet<OrderDatabaseObject>(0);
        var orderProductSet = GetQueryableDbSet<OrderProductDatabaseObject>(0);
        var dataContext = GetMockDataContext(productSet, orderSet, orderProductSet);
        var unitOfWork = new Mock<UnitOfWork>(dataContext.Object);
        var productController = new ProductController(unitOfWork.Object, _mapper);
        var pageNumber = 0;
        var pageSize = 10;
        
        // Act
        var result = productController.GetAll(pageNumber, pageSize).Result as OkObjectResult;
        
        // Assert
        Assert.NotNull(result?.Value);
        var collection = result.Value as ProductCollection;
        Assert.NotNull(collection);
        Assert.NotEmpty(collection.Products);
        foreach (var product in productSet.Object)
        {
            Assert.NotNull(collection.Products.FirstOrDefault(p => p.Id == product.Id));
        }
        Assert.Equal(productSet.Object.Count(), collection.ElementCount);
        Assert.Equal(pageNumber, collection.Page.PageNumber);
        Assert.Equal(pageSize, collection.Page.PageSize);
    }
    
    [Fact]
    public void GetCollection_InvalidPageNumber_ReturnsBadRequest()
    {
        // Arrange
        var productSet = GetQueryableDbSet<ProductDatabaseObject>(2);
        var orderSet = GetQueryableDbSet<OrderDatabaseObject>(0);
        var orderProductSet = GetQueryableDbSet<OrderProductDatabaseObject>(0);
        var dataContext = GetMockDataContext(productSet, orderSet, orderProductSet);
        var unitOfWork = new Mock<UnitOfWork>(dataContext.Object);
        var productController = new ProductController(unitOfWork.Object, _mapper);
        var pageNumber = -1;
        var pageSize = 10;
        
        // Act
        var result = productController.GetAll(pageNumber, pageSize).Result as BadRequestObjectResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public void GetCollection_InvalidPageSize_ReturnsBadRequest()
    {
        // Arrange
        var productSet = GetQueryableDbSet<ProductDatabaseObject>(2);
        var orderSet = GetQueryableDbSet<OrderDatabaseObject>(0);
        var orderProductSet = GetQueryableDbSet<OrderProductDatabaseObject>(0);
        var dataContext = GetMockDataContext(productSet, orderSet, orderProductSet);
        var unitOfWork = new Mock<UnitOfWork>(dataContext.Object);
        var productController = new ProductController(unitOfWork.Object, _mapper);
        var pageNumber = 0;
        var pageSize = 0;
        
        // Act
        var result = productController.GetAll(pageNumber, pageSize).Result as BadRequestObjectResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}