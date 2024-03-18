using UnitOfWorkDesignPattern.Models.DatabaseObjects;

namespace UnitOfWorkDesignPattern.Models.DataTransferObjects;

public class Order : IHasId
{
    public long Id { get; set; }
    public List<long> ProductIds { get; set; }
}