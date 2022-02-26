using DevIO.Business.Enuns;
using System.Collections.Generic;

namespace DevIO.Business.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public bool Ativo { get; set; }

        public Endereco Endereco { get; set; }
        public ICollection<Produto> Produtos { get; set; }
    }
}
