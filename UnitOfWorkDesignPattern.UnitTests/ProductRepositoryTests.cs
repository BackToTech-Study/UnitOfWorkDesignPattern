using MockQueryable.Moq;
using Moq;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;

namespace UnitOfWorkDesignPattern.UnitTests;

public class ProductRepositoryTests
{
    [Fact]
    public void Count_EmptyDatabase_ReturnsZero()
    {
        // Arrange
        var mock = new List<ProductDatabaseObject>().AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        
        // Act
        var productRepository = new ProductRepository(dataContext.Object);
        
        // Assert
        Assert.Equal(0, productRepository.Count());
    }
    
    [Fact]
    public void Count_PopulatedDatabase_ReturnsCount()
    {
        // Arrange
        var databaseObjects = new List<ProductDatabaseObject>
        {
            new ProductDatabaseObject { Id = 1 },
            new ProductDatabaseObject { Id = 2 },
        };
        var mock = databaseObjects.AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
            .Returns(mock.Object);        
        
        // Act
        var productRepository = new ProductRepository(dataContext.Object);
        
        // Assert
        Assert.Equal(databaseObjects.Count, productRepository.Count());
    }
    
    [Fact]
    public void GetCollection_EmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        var mock = new List<ProductDatabaseObject>().AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
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
        var databaseObjects = new List<ProductDatabaseObject>
        {
            new ProductDatabaseObject { Id = 1 },
            new ProductDatabaseObject { Id = 2 },
        };
        var mock = databaseObjects.AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
            .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
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
        var mock = new List<ProductDatabaseObject>().AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => productRepository.GetCollection(null));
    }
    
    [Fact]
    public void GetCollection_PageNumberLessThanZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var mock = new List<ProductDatabaseObject>().AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
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
        var mock = new List<ProductDatabaseObject>().AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
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
        var databaseObjects = new List<ProductDatabaseObject>
        {
            new ProductDatabaseObject { Id = 1 },
            new ProductDatabaseObject { Id = 2 },
        };
        var mock = databaseObjects.AsQueryable().BuildMockDbSet();
        var dataContext = new Mock<ApplicationDataContext>();
        dataContext.Setup(x => x.Set<ProductDatabaseObject>())
                    .Returns(mock.Object);        
        var productRepository = new ProductRepository(dataContext.Object);
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