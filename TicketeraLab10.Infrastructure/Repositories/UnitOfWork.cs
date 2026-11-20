using TicketeraLab10.Domain.Interfaces;
using TicketeraLab10.Infrastructure.Data;

namespace TicketeraLab10.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketeraDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(TicketeraDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (!_repositories.ContainsKey(typeof(T)))
            _repositories[typeof(T)] = new GenericRepository<T>(_context);

        return (IGenericRepository<T>)_repositories[typeof(T)];
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}