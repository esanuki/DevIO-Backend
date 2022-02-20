using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;

namespace DevIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(DevIODbContext context) : base(context)
        {
        }
    }
}
