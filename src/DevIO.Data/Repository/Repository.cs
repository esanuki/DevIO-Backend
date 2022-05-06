using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly DevIODbContext _context;
        protected readonly DbSet<T> _dataSet;

        public Repository(DevIODbContext context)
        {
            _context = context;
            _dataSet = context.Set<T>();
        }
        
        public virtual async Task<T> ObterPorId(Guid id)
        {
            return await _dataSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<IEnumerable<T>> ObterTodos()
        {
            return await _dataSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await _dataSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task Adicionar(T entity)
        {
            _context.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(T entity)
        {
            _context.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            _context.Remove(new T { Id = id });
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
