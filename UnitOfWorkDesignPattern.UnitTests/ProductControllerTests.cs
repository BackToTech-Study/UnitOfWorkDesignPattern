using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitOfWorkDesignPattern.Controllers;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;
using UnitOfWorkDesignPattern.Models.MapperProfiles;
using UnitOfWorkDesignPattern.UnitTests.Mock;

namespace UnitOfWorkDesignPattern.UnitTests;

public class ProductControllerTests
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContextFactory _dbContextFactory;
    public ProductControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMapperProfile>();
        });
        _mapper = new Mapper(mapperConfiguration);
        
        _dbContextFactory = new MockedDatabaseContextFactory();
    }
    
    [Fact]
    public void GetCollection_EmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();        
        var unitOfWork = new Mock<UnitOfWork>(dataContext);
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
        var productSet = _dbContextFactory.SeedProducts(2);
        _dbContextFactory.SeedOrders(0);
        _dbContextFactory.SeedOrderProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();
        var unitOfWork = new Mock<UnitOfWork>(dataContext);
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
        foreach (var product in productSet)
        {
            Assert.NotNull(collection.Products.FirstOrDefault(p => p.Id == product.Id));
        }
        Assert.Equal(productSet.Count(), collection.ElementCount);
        Assert.Equal(pageNumber, collection.Page.PageNumber);
        Assert.Equal(pageSize, collection.Page.PageSize);
    }
    
    [Fact]
    public void GetCollection_InvalidPageNumber_ReturnsBadRequest()
    {
        // Arrange
        var productSet = _dbContextFactory.SeedProducts(2);
        _dbContextFactory.SeedOrders(0);
        _dbContextFactory.SeedOrderProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();
        var unitOfWork = new Mock<UnitOfWork>(dataContext);
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
        var productSet = _dbContextFactory.SeedProducts(2);
        _dbContextFactory.SeedOrders(0);
        _dbContextFactory.SeedOrderProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();
        var unitOfWork = new Mock<UnitOfWork>(dataContext);
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