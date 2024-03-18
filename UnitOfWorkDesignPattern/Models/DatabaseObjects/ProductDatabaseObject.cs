namespace UnitOfWorkDesignPattern.Models.DatabaseObjects;

public class ProductDatabaseObject : IHasId
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}