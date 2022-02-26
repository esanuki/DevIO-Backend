using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DevIODbContext context) : base(context)
        {
        }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await _dataSet.AsNoTracking().Include(p => p.Fornecedor).FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await _context
                .Set<Produto>()
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }
    }
}
