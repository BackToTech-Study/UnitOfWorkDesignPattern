using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.UnitTests.Mock;

namespace UnitOfWorkDesignPattern.UnitTests;

public class UnitOfWorkTests : IDisposable
{
    private readonly IDatabaseContextFactory _dbContextFactory;
    private List<long> _productIdCollection = new();
    
    public UnitOfWorkTests()
    {
        _dbContextFactory = new TestDatabaseContextFactory();
    }
    
    public void Dispose()
    {
        var dataContext = _dbContextFactory.GetDataContext();
        foreach (var id in _productIdCollection)
        {
            var product = dataContext.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product != null)
            {
                dataContext.Products.Remove(product);
                dataContext.SaveChanges();
            }
        }
    }
    
    [Fact]
    public void Save_NoCommit_ChangesNotStored()
    {
        // Arrange
        var dataContext = _dbContextFactory.GetDataContext();
        var unitOfWork = new UnitOfWork(dataContext);
        var product = new ProductDatabaseObject
        {
            Name = $"Product {Guid.NewGuid()}",
            Description = "Description"
        };
        var productRepository = unitOfWork.ProductRepository;
        long createdProductId = 0;
        
        // Act
        using (var transaction = unitOfWork.GetNewTransactionScope()) {
            var createdProduct = productRepository.Create(product);
            unitOfWork.Save();
            createdProductId = createdProduct.Id;
        }

        // Assert
        Assert.Throws<KeyNotFoundException>(() =>  productRepository.GetById(createdProductId));
    }
    
    [Fact]
    public void Save_Commit_ChangesStored()
    {
        // Arrange
        var dataContext = _dbContextFactory.GetDataContext();
        var unitOfWork = new UnitOfWork(dataContext);
        var product = new ProductDatabaseObject
        {
            Name = $"Product {Guid.NewGuid()}",
            Description = "Description"
        };
        var productRepository = unitOfWork.ProductRepository;
        long createdProductId = 0;
        
        // Act
        using (var transaction = unitOfWork.GetNewTransactionScope()) {
            var createdProduct = productRepository.Create(product);
            unitOfWork.Save();
            unitOfWork.Commit(transaction);
            createdProductId = createdProduct.Id;
            _productIdCollection.Add(createdProductId);
        }
        var retrievedProduct = productRepository.GetById(createdProductId);

        // Assert
        Assert.NotNull(retrievedProduct);
    }
}