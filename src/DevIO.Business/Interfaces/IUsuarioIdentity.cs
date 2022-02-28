using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DevIO.Business.Interfaces
{
    public interface IUsuarioIdentity
    {
        string Nome { get; }
        Guid ObterUsuarioId();
        string ObterUsuarioEmail();
        bool EhAutenticado();
        bool TemPermissao(string permissao);
        IEnumerable<Claim> ObterClaims();
    }
}
