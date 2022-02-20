using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/v1/fornecedores")]
    public class FornecedorController : MainController
    {
        public FornecedorController(INotificadorService notificador) : base(notificador)
        {
        }
    }
}
