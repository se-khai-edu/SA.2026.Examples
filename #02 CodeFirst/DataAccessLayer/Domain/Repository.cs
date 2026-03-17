using Microsoft.EntityFrameworkCore;

namespace Domain;

// todo : 4.1 Add generic repository class implementing IRepository<T> interface
// for basic CRUD operations
public class Repository<T> : IRepository<T> where T : class
{
    private readonly Storage _context;
    private readonly DbSet<T> _dbSet;

    public Repository(Storage context) 
    { 
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public void Add(T entity) => _dbSet.Add(entity);

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
    }

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public T? GetById(params object[] keyValues) => _dbSet.Find(keyValues);    

    public void Save() => _context.SaveChanges();

    public void Update(T entity) =>
         _dbSet.Update(entity);
}
