using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private IFornecedorRepository _fornecedorRepository;
        private IEnderecoRepository _enderecoRepository;

        public FornecedorService(
            INotificadorService notificador, 
            IFornecedorRepository fornecedorRepository, 
            IEnderecoRepository enderecoRepository) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<bool> Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) ||
                !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco))
                return false;

            if (_fornecedorRepository.Buscar(f => f.Documento.Equals(fornecedor.Documento)).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado");
                return false;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
            return true;
        }

        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;

            if (_fornecedorRepository.Buscar(f => f.Documento.Equals(fornecedor.Documento) && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento");
                return false;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
            return true;
        }

        public async Task<bool> Remover(Guid id)
        {
            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);

            if (endereco != null) await _enderecoRepository.Remover(endereco.Id);

            await _fornecedorRepository.Remover(id);

            return true;
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }

    }
}
