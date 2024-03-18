namespace UnitOfWorkDesignPattern.Models.DataTransferObjects;

public class OrderCollection
{
    public long ElementCount { get; set; }
    public Page Page { get; set; }
    public List<Order> Orders { get; set; }
}