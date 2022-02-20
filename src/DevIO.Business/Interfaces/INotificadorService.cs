using DevIO.Business.Models;
using System.Collections.Generic;

namespace DevIO.Business.Interfaces
{
    public interface INotificadorService
    {
        bool TemNoficacao();
        IEnumerable<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
