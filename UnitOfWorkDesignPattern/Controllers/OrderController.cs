using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;
namespace UnitOfWorkDesignPattern.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public OrderController(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get")]
    public ActionResult<OrderCollection> GetAll(int pageNumber, int pageSize)
    {
        if (pageNumber < 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size are required!");
        }
        
        Page page = new()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        var count = _unitOfWork.OrderRepository.Count();
        var databaseObjectCollection = count <= 0 ? new List<OrderDatabaseObject>() : _unitOfWork.OrderRepository.GetCollection(page);
        var orderCollection = _mapper.Map<List<Order>>(databaseObjectCollection);
        foreach (var order in orderCollection)
        {
            order.ProductIds = _unitOfWork.ProductRepository.GetProductsByOrderId(order.Id)
                                                            .Select(p => p.Id)
                                                            .ToList();
        }
        
        return Ok(new OrderCollection
        {
            ElementCount = count,
            Page = page,
            Orders = orderCollection
        });
    }
    
    [HttpGet]
    [Route("get/{id}")]
    public ActionResult<Order> GetById(long id)
    {
        if (id <= 0)
        {
            return BadRequest("The order id is required!");
        }
        
        var databaseObject = _unitOfWork.OrderRepository.GetById(id);
        var order = _mapper.Map<Order>(databaseObject);
        return Ok(order);
    }
    
    [HttpPost]
    [Route("create")]
    public ActionResult<Order> Create(Order order)
    {
        if (order == null)
        {
            return BadRequest("The order is required!");
        }
        
        using var transaction = _unitOfWork.GetNewTransactionScope();
        try
        {
            var newDatabaseObject = _mapper.Map<OrderDatabaseObject>(order);
            var createdDatabaseObject = _unitOfWork.OrderRepository.Create(newDatabaseObject);
            _unitOfWork.Save();
            _unitOfWork.Commit(transaction);
            var createdOrder = _mapper.Map<Order>(createdDatabaseObject);
            return Ok(createdOrder);
        }
        catch (Exception e)
        {
            return UnprocessableEntity(e.Message);
        }
    }
    
    [HttpPut]
    [Route("add-products/{id}")]
    public ActionResult AddProducts([FromRoute]long id, [FromBody]ProductCollection productCollection)
    {
        if (id <= 0)
        {
            return BadRequest("The order id is required!");
        }
        
        if (productCollection?.Products == null || productCollection.Products.Count <= 0)
        {
            return BadRequest("The product collection is required!");
        }
        
        var order = _unitOfWork.OrderRepository.GetById(id);
        if (order == null)
        {
            return NotFound($"The order with id {id} was not found!");
        }

        using var transaction = _unitOfWork.GetNewTransactionScope();
        try
        {
            foreach (var product in productCollection.Products)
            {
                var productId = product.Id;
                if (productId <= 0)
                {
                    var newDataBaseObject = _mapper.Map<ProductDatabaseObject>(product);
                    var createdProduct = _unitOfWork.ProductRepository.Create(newDataBaseObject);
                    _unitOfWork.Save();
                    productId = createdProduct.Id;
                }
                else
                {
                    if (_unitOfWork.ProductRepository.GetById(productId) == null)
                    {
                        return NotFound($"The product with id {productId} was not found!");
                    }
                    
                    if (_unitOfWork.OrderProductRepository.GetByOrderIdAndProductId(id, productId) != null)
                    {
                        return BadRequest($"The product with id {productId} is already recorded for the order with id {id}!");
                    }
                }

                _unitOfWork.OrderProductRepository.AddOrderProduct(id, productId);
                _unitOfWork.Save();
            }
            
            _unitOfWork.Commit(transaction);
            return NoContent();
        } 
        catch (Exception e)
        {
            return UnprocessableEntity(e.Message);
        }
    }
    
    [HttpDelete]
    [Route("delete")]
    public ActionResult Delete(long id)
    {
        if (id <= 0)
        {
            return BadRequest("The order id is required!");
        }

        using var transaction = _unitOfWork.GetNewTransactionScope();
        try
        {
            _unitOfWork.OrderRepository.Delete(id);
            _unitOfWork.Save();
            _unitOfWork.Commit(transaction);            
            return NoContent();
        }
        catch (Exception e)
        {
            return UnprocessableEntity(e.Message);
        }
    }
}