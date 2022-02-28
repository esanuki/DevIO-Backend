using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotificadorService _notificador;
        private readonly IUsuarioIdentity _usuarioIdentity;

        protected Guid UsuarioId { get; private set; }
        public bool UsuarioAutenticado { get; private set; }

        public MainController(
            INotificadorService notificador, 
            IUsuarioIdentity usuarioIdentity)
        {
            _notificador = notificador;
            _usuarioIdentity = usuarioIdentity;

            if (_usuarioIdentity.EhAutenticado())
            {
                UsuarioId = _usuarioIdentity.ObterUsuarioId();
                UsuarioAutenticado = true;
            }
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNoficacao();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErrorModelInvalida(modelState);

            return CustomResponse();
        }

        protected void NotificarErrorModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var item in erros)
            {
                var errorMsg = item.Exception == null ? item.ErrorMessage : item.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
