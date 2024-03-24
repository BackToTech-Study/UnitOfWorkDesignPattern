using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;
using UnitOfWorkDesignPattern.UnitTests.Mock;

namespace UnitOfWorkDesignPattern.UnitTests;

public class ProductRepositoryTests
{
    private readonly IDatabaseContextFactory _dbContextFactory;
    public ProductRepositoryTests()
    {
        _dbContextFactory = new MockedDatabaseContextFactory();
    }
    
    [Fact]
    public void Count_EmptyDatabase_ReturnsZero()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();
        
        // Act
        var productRepository = new ProductRepository(dataContext);
        
        // Assert
        Assert.Equal(0, productRepository.Count());
    }
    
    [Fact]
    public void Count_PopulatedDatabase_ReturnsCount()
    {
        // Arrange
        var databaseObjects = _dbContextFactory.SeedProducts(2);
        var dataContext = _dbContextFactory.GetDataContext();
        
        // Act
        var productRepository = new ProductRepository(dataContext);
        
        // Assert
        Assert.Equal(databaseObjects.Count(), productRepository.Count());
    }
    
    [Fact]
    public void GetCollection_EmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext(); 
        var productRepository = new ProductRepository(dataContext);
        var page = new Page()
        {
            PageNumber = 0,
            PageSize = 10
        };
        
        // Act
        var collection = productRepository.GetCollection(page);
        
        // Assert
        Assert.Empty(collection);
    }
    
    [Fact]
    public void GetCollection_PopulatedDatabase_ReturnsCollection()
    {
        // Arrange
        var databaseObjects = _dbContextFactory.SeedProducts(2);
        var dataContext = _dbContextFactory.GetDataContext();   
        var productRepository = new ProductRepository(dataContext);
        var page = new Page()
        {
            PageNumber = 0,
            PageSize = 10
        };
        
        // Act
        var collection = productRepository.GetCollection(page);
        
        // Assert
        foreach (var product in databaseObjects)
        {
            Assert.NotNull(collection.FirstOrDefault(p => p.Id == product.Id));
        }
    }
    
    [Fact]
    public void GetCollection_NullPage_ThrowsArgumentNullException()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();        
        var productRepository = new ProductRepository(dataContext);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => productRepository.GetCollection(null));
    }
    
    [Fact]
    public void GetCollection_PageNumberLessThanZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();        
        var productRepository = new ProductRepository(dataContext);
        var page = new Page()
        {
            PageNumber = -1,
            PageSize = 10
        };
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => productRepository.GetCollection(page));
    }
    
    [Fact]
    public void GetCollection_PageSizeLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        _dbContextFactory.SeedProducts(0);
        var dataContext = _dbContextFactory.GetDataContext();      
        var productRepository = new ProductRepository(dataContext);
        var page = new Page()
        {
            PageNumber = 0,
            PageSize = 0
        };
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => productRepository.GetCollection(page));
    }
    
    [Fact]
    public void GetCollection_PageSizeZero_ReturnsEmptyCollection()
    {
        // Arrange
        var databaseObjects = _dbContextFactory.SeedProducts(2);
        var dataContext = _dbContextFactory.GetDataContext();          
        var productRepository = new ProductRepository(dataContext);
        var page = new Page()
        {
            PageNumber = 0,
            PageSize = 0
        };
        
        // Act
        var collection = productRepository.GetCollection(page);
        
        // Assert
        Assert.Empty(collection);
    }
}