using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Core.Entities.Common;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class Repository<T> : IRepository<T> where T : BaseEntity,new()
{
    readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public async Task CreateAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        Table.Remove(entity);
    }

    public IQueryable<T> FindAll(Expression<Func<T, bool>> expression, params string[] includes)
    {
        var query = Table.AsQueryable();
        return _getIncludes(query, includes).Where(expression);
    }

    public IQueryable<T> GetAll(params string[] includes)
    {
        var query = Table.AsQueryable();
        return _getIncludes(query, includes);
    }

    public async Task<T> GetByIdAsync(int id, params string[] includes)
    {
        if (includes.Length == 0)
        {
            return await Table.FindAsync(id);
        }
        var query = Table.AsQueryable();
        return await _getIncludes(query, includes).SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] includes)
    {
        var query = Table.AsQueryable();
        return await _getIncludes(query, includes).SingleOrDefaultAsync(expression);
    }

    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
    {
        return await Table.AnyAsync(expression);
    }

    public void RevertSoftDelete(T entity)
    {
        entity.IsDeleted = false;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void SoftDelete(T entity)
    {
        entity.IsDeleted = true;
    }

    IQueryable<T> _getIncludes(IQueryable<T> query, params string[] includes)
    {
        foreach (var item in includes)
        {
            query = query.Include(item);
        }
        return query;
    }
}

