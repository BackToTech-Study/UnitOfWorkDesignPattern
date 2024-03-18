using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public class ProductRepository : BaseRepository<ProductDatabaseObject>
{
    public ProductRepository(ApplicationDataContext context) 
        : base(context)
    {
    }

    public List<ProductDatabaseObject> GetProductsByOrderId(long orderId)
    {
        var productIdCollection =  Context.OrderProducts
                                                    .Where(op => op.OrderId == orderId)
                                                    .Select(op => op.ProductId)
                                                    .ToList();

        var productCollection = new List<ProductDatabaseObject>();
        foreach (var id in productIdCollection)
        {
            var productDatabaseObject = Context.Set<ProductDatabaseObject>()
                                               .FirstOrDefault(p => p.Id == id);
            if (productDatabaseObject != null)
            {
                productCollection.Add(productDatabaseObject);
            }
        }
        
        return productCollection;
    }
}