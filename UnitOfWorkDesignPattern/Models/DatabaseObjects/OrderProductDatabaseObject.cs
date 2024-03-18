namespace UnitOfWorkDesignPattern.Models.DatabaseObjects;

public class OrderProductDatabaseObject : IHasId
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}