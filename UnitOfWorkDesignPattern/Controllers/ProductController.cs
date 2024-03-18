using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;

namespace UnitOfWorkDesignPattern.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public ProductController(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("get")]
    public ActionResult<ProductCollection> GetAll(int pageNumber, int pageSize)
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
        
        var count = _unitOfWork.ProductRepository.Count();
        var databaseObjectCollection = count <= 0 ? new List<ProductDatabaseObject>() : _unitOfWork.ProductRepository.GetCollection(page);
        var productCollection = _mapper.Map<List<Product>>(databaseObjectCollection);
        foreach (var product in productCollection)
        {
            product.OrderIds = _unitOfWork.OrderRepository.GetOrdersByProductId(product.Id)
                                                          .Select(o => o.Id)
                                                          .ToList();
        }
        
        return Ok(new ProductCollection
        {
            ElementCount = count,
            Page = page,
            Products = productCollection
        });
    }
    
    [HttpGet]
    [Route("get/{id}")]
    public ActionResult<Product> GetById(long id)
    {
        if (id <= 0)
        {
            return BadRequest("The product id is required!");
        }
        
        var databaseObject = _unitOfWork.ProductRepository.GetById(id);
        var product = _mapper.Map<Product>(databaseObject);
        return Ok(product);
    }
    
    [HttpPost]
    [Route("create")]
    public ActionResult<Product> Create(Product product)
    {
        if (product == null)
        {
            return BadRequest("The product is required!");
        }
        
        using var transaction = _unitOfWork.GetNewTransactionScope();
        try
        {
            var newDataBaseObject = _mapper.Map<ProductDatabaseObject>(product);
            var createdDataBaseObject = _unitOfWork.ProductRepository.Create(newDataBaseObject);
            _unitOfWork.Save();
            _unitOfWork.Commit(transaction);
            var createdProduct = _mapper.Map<Product>(createdDataBaseObject);
            return Ok(createdProduct);    
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
            return BadRequest("The product id is required!");
        }
        using var transaction = _unitOfWork.GetNewTransactionScope();
        try
        {
            _unitOfWork.ProductRepository.Delete(id);
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