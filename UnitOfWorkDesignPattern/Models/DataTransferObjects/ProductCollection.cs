namespace UnitOfWorkDesignPattern.Models.DataTransferObjects;

public class ProductCollection
{
    public long ElementCount { get; set; }
    public Page Page { get; set; }
    public List<Product> Products { get; set; }
}