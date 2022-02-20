using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(DevIODbContext context) : base(context)
        {
        }

        public async Task<Fornecedor> ObterFornecedorComEndereco(Guid id)
        {
            return await _dataSet.AsNoTracking().Include(f => f.Endereco).FirstOrDefaultAsync(f => f.Id.Equals(id));
        }
    }
}
