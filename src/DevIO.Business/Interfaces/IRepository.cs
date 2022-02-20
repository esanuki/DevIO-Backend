using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<T> ObterPorId(Guid id);
        Task<IEnumerable<T>> ObterTodos();
        Task Adicionar(T entity);
        Task Atualizar(T entity);
        Task Remover(Guid id);
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
        Task<int> SaveChanges();

    }
}
