using AutoMapper;
using DevIO.Api.Interop.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DevIO.Api.CustomAuthorization;

namespace DevIO.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/fornecedores")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedorController(
            INotificadorService notificador,
            IFornecedorRepository fornecedorRepository,
            IFornecedorService fornecedorService, 
            IMapper mapper,
            IUsuarioIdentity usuario) : base(notificador, usuario)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        //[AllowAnonymous]
        [HttpGet]
        [ClaimsAuthorize("Fornecedor", "Visualizar")]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ClaimsAuthorize("Fornecedor", "Visualizar")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [HttpPost]
        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorView)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorView));

            return CustomResponse(fornecedorView);
        }

        [HttpPut]
        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar([FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var fornecedor = _fornecedorRepository.Buscar(f => f.Id == fornecedorViewModel.Id).Result;

            if (!(fornecedor.Any()))
            {
                NotificarErro("Fornecedor não encontrado");
                return CustomResponse(fornecedorViewModel);
            }

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ClaimsAuthorize("Fornecedor", "Excluir")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterPorId(id);

            if (fornecedor == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse(_mapper.Map<FornecedorViewModel>(fornecedor));
        }
    }
}
