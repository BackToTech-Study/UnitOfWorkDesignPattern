using Microsoft.EntityFrameworkCore;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public class ApplicationDataContext : DbContext
{
    public virtual DbSet<OrderProductDatabaseObject> OrderProducts { get; set; }
    public virtual DbSet<OrderDatabaseObject> Orders { get; set; }
    public virtual DbSet<ProductDatabaseObject> Products { get; set; }
    
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) 
        : base(options)
    {
    }
    
    public ApplicationDataContext() 
    {
    }
}