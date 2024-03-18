using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public class OrderProductRepository
{
    private readonly ApplicationDataContext _context;
    public OrderProductRepository(ApplicationDataContext context) 
    {
        _context = context;
    }

    public List<long> GetProductIdsByOrderId(long orderId)
    {
        return _context.OrderProducts
                       .Where(op => op.OrderId == orderId)
                       .Select(op => op.ProductId)
                       .ToList();
    }
    
    public List<long> GetOrderIdsByProductId(long productId)
    {
        return _context.OrderProducts
                       .Where(op => op.ProductId == productId)
                       .Select(op => op.OrderId)
                       .ToList();
    }
    
    public OrderProductDatabaseObject? GetByOrderIdAndProductId(double orderId, double productId)
    {
        return _context.OrderProducts
                       .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
    }
    
    public void AddOrderProduct(long orderId, long productId)
    {
        var orderProduct = _context.OrderProducts
            .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
        if (orderProduct != null)
        {
            throw new KeyNotFoundException($"The order product with the order id {orderId} and the product id {productId} is already recorded.");
        }
        
        _context.OrderProducts.Add(new OrderProductDatabaseObject
        {
            OrderId = orderId,
            ProductId = productId
        });
    }
    
    public void DeleteOrderProduct(long orderId, long productId)
    {
        var orderProduct = _context.OrderProducts
                                  .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
        if (orderProduct == null)
        {
            throw new KeyNotFoundException($"The order product with the order id {orderId} and the product id {productId} was not found.");
        }
        
        _context.OrderProducts.Remove(orderProduct);
    }
    
    public void DeleteById(long id)
    {
        var orderProduct = _context.OrderProducts.Find(id);
        if (orderProduct == null)
        {
            throw new KeyNotFoundException($"The order product with the id {id} was not found.");
        }
        
        _context.OrderProducts.Remove(orderProduct);
    }
}