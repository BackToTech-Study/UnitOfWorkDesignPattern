using Microsoft.EntityFrameworkCore;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public class ApplicationDataContext : DbContext
{
    public DbSet<OrderProductDatabaseObject> OrderProducts { get; set; }
    public DbSet<OrderDatabaseObject> Orders { get; set; }
    public DbSet<ProductDatabaseObject> Products { get; set; }
    
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) 
        : base(options)
    {
    }
}