using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.DAL.Repositories.Interfaces;

public interface IRepository<T> where T: BaseEntity,new()
{
    DbSet<T> Table { get; }
    IQueryable<T> GetAll(params string[] includes);
    IQueryable<T> FindAll(Expression<Func<T, bool>> expression, params string[] includes);
    Task<T> GetByIdAsync(int id, params string[] includes);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] includes);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    void Delete(T entity);
    void SoftDelete(T entity);
    void RevertSoftDelete(T entity);
    Task DeleteAsync(int id);
    Task SaveAsync();
}

