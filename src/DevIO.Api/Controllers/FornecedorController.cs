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

namespace DevIO.Api.Controllers
{
    [Route("api/v1/fornecedores")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedorController(
            INotificadorService notificador,
            IFornecedorRepository fornecedorRepository,
            IFornecedorService fornecedorService, 
            IMapper mapper) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorComEndereco(id));

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorView)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorView));

            return CustomResponse(fornecedorView);
        }

        [HttpPut]
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
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterPorId(id);

            if (fornecedor == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse(_mapper.Map<FornecedorViewModel>(fornecedor));
        }
    }
}
