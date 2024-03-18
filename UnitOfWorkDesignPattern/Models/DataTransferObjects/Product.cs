using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.Models.DataTransferObjects;

public class Product : IHasId
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<long> OrderIds { get; set; }
}