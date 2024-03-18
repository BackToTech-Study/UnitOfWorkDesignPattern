using UnitOfWorkDesignPattern.Models.DatabaseObjects;
using UnitOfWorkDesignPattern.Models.DataTransferObjects;

namespace UnitOfWorkDesignPattern.DataStorage;

public abstract class BaseRepository<TDatabase> where TDatabase : class, IHasId
{
    protected readonly ApplicationDataContext Context;

    protected BaseRepository(ApplicationDataContext context)
    {
        Context = context;
    }
    
    public long Count()
    {
        return Context.Set<TDatabase>().Count();
    }
    
    public List<TDatabase> GetCollection(Page page)
    {
        ArgumentNullException.ThrowIfNull(page);
        
        return Context.Set<TDatabase>()
            .OrderBy(dbo => dbo.Id)
            .Skip(page.PageNumber * page.PageSize)
            .Take(page.PageSize)
            .ToList();
    }

    public TDatabase GetById(double id)
    {
        var databaseObject = Context.Set<TDatabase>().Where(dbo => dbo.Id == id).FirstOrDefault();
        if (databaseObject == null)
        {
            throw new KeyNotFoundException($"The database object of type {typeof(TDatabase)} with the id {id} was not found.");
        }
        
        return databaseObject;
    }

    public TDatabase Create(TDatabase databaseObject)
    {
        ArgumentNullException.ThrowIfNull(databaseObject);
        
        if (databaseObject == null)
        {
            throw new InvalidOperationException($"The database object of type {typeof(TDatabase)} could not be created.");
        }
        
        return Context.Set<TDatabase>().Add(databaseObject).Entity;
    }

    public TDatabase Update(TDatabase databaseObject)
    {
        ArgumentNullException.ThrowIfNull(databaseObject);
        
        var dataSet = Context.Set<TDatabase>();
        var existingObject = dataSet.FirstOrDefault(dbo => dbo.Id == databaseObject.Id);
        if (existingObject == null)
        {
            throw new KeyNotFoundException($"The database object of type {typeof(TDatabase)} with the id {databaseObject.Id} was not found.");
        }
        
        Context.Set<TDatabase>().Update(databaseObject);
        
        existingObject = dataSet.FirstOrDefault(dbo => dbo.Id == databaseObject.Id);
        if (existingObject == null)
        {
            throw new InvalidOperationException($"The database object of type {typeof(TDatabase)} and id {databaseObject.Id} is no longer in the database.");
        }
        
        return existingObject;
    }

    public void Delete(long id)
    {
        var databaseObject = Context.Set<TDatabase>().Find(id);
        if (databaseObject == null)
        {
            throw new KeyNotFoundException($"The database object of type {typeof(TDatabase)} with the id {id} was not found.");
        }
        
        Context.Set<TDatabase>().Remove(databaseObject);
    }
}