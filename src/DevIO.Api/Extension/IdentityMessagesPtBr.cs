using Microsoft.AspNetCore.Identity;

namespace DevIO.Api.Extension
{
    public class IdentityMessagesPtBr : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Ocorreu um erro desconhecido." }; }
    }
}
