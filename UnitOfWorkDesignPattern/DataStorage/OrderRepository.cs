using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public class OrderRepository : BaseRepository<OrderDatabaseObject>
{
    public OrderRepository(ApplicationDataContext context) 
        : base(context)
    {
    }

    public List<OrderDatabaseObject> GetOrdersByProductId(long productId)
    {
        var orderIdCollection =  Context.OrderProducts.Where(op => op.ProductId == productId)
                                                                .Select(op => op.OrderId)
                                                                .ToList();

        var orderCollection = new List<OrderDatabaseObject>();
        foreach (var id in orderIdCollection)
        {
            var orderDatabaseObject = Context.Set<OrderDatabaseObject>()
                                             .FirstOrDefault(o => o.Id == id);
            if (orderDatabaseObject != null)
            {
                orderCollection.Add(orderDatabaseObject);
            }
        }
        
        return orderCollection;
    }
}