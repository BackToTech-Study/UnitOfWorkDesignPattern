using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWorkDesignPattern.DataStorage;

public class UnitOfWork
{
    private readonly ApplicationDataContext _context;
    public OrderRepository OrderRepository;
    public ProductRepository ProductRepository;
    public OrderProductRepository OrderProductRepository;
    
    public UnitOfWork(ApplicationDataContext context)
    {
        _context = context;
        OrderRepository = new OrderRepository(_context);
        ProductRepository = new ProductRepository(_context);
        OrderProductRepository = new OrderProductRepository(_context);
    }
    
    public void Save()
    {
        _context.SaveChanges();
    }
    
    public IDbContextTransaction GetNewTransactionScope()
    {
        return _context.Database.BeginTransaction();
    }
    
    public void Commit(IDbContextTransaction transaction)
    {
        transaction.Commit();
    }
}