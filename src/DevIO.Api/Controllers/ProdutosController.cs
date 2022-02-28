using AutoMapper;
using DevIO.Api.Extension;
using DevIO.Api.Interop.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static DevIO.Api.CustomAuthorization;

namespace DevIO.Api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutosController(
            INotificadorService notificador,
            IProdutoService produtoService, 
            IProdutoRepository produtoRepository,
            IMapper mapper,
            IUsuarioIdentity usuario) : base(notificador, usuario)
        {
            _produtoService = produtoService;
            _mapper = mapper;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        [ClaimsAuthorize("Produto", "Visualizar")]
        public async Task<IEnumerable<ProdutoViewModel>> Get()
        {
            var result = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());

            return result;
        }

        [HttpGet("{id:guid}")]
        [ClaimsAuthorize("Produto", "Visualizar")]
        public async Task<ActionResult<ProdutoViewModel>> Get(Guid id)
        {
            var result = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));

            if (result == null) return NotFound();

            return result;
        }

        [HttpPost]
        [ClaimsAuthorize("Produto", "Adicionar")]
        public async Task<ActionResult<ProdutoViewModel>> Post([FromBody] ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (!string.IsNullOrEmpty(produtoViewModel.ImagemUpload))
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome)) return CustomResponse(ModelState);

                produtoViewModel.Imagem = imagemNome;
            }

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);


        }

        [HttpPut]
        [ClaimsAuthorize("Produto", "Atualizar")]
        public async Task<ActionResult> Put([FromBody] ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produtoAtualizacao = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor((Guid)produtoViewModel.Id));

            if (produtoAtualizacao == null)
            {
                NotificarErro("Produto não encontrado!");
                return CustomResponse();
            }

            if (string.IsNullOrEmpty(produtoViewModel.Imagem)) produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (produtoViewModel.ImagemUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome)) return CustomResponse(ModelState);

                produtoAtualizacao.Imagem = imagemNome;
            }

            produtoAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoViewModel);
        }

        [HttpDelete]
        [ClaimsAuthorize("Produto", "Excluir")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);

            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(_mapper.Map<ProdutoViewModel>(produto));
        }

        [HttpPost("adicionar")]
        [ClaimsAuthorize("Produto", "Adicionar")]
        public async Task<ActionResult<ProdutoViewModel>> AdicionarFormFile([ModelBinder(BinderType = typeof(ProdutoModelBinder))] ProdutoImagemViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivoFormFile(produtoViewModel.ImagemUpload, imgPrefixo)) return CustomResponse(ModelState);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);

        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneceça uma imagem para este produto");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<bool> UploadArquivoFormFile(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create)) await arquivo.CopyToAsync(stream);

            return true;
        }
    }
}
