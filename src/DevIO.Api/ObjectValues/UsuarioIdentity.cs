using DevIO.Api.Extension;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevIO.Api.ObjectValues
{
    public class UsuarioIdentity : IUsuarioIdentity
    {

        private readonly IHttpContextAccessor _context;

        public UsuarioIdentity(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string Nome => _context.HttpContext.User.Identity.Name;

        public bool EhAutenticado() => _context.HttpContext.User.Identity.IsAuthenticated;
        public IEnumerable<Claim> ObterClaims() => _context.HttpContext.User.Claims;

        public string ObterUsuarioEmail() => EhAutenticado() ? _context.HttpContext.User.ObterUsuarioEmail() : "";

        public Guid ObterUsuarioId() => EhAutenticado() ? Guid.Parse(_context.HttpContext.User.ObterUsuarioId()) : Guid.Empty;

        public bool TemPermissao(string permissao) => _context.HttpContext.User.IsInRole(permissao);

    }


}
