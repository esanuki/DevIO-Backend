using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.Business.Services
{
    public class NotificadorService : INotificadorService
    {
        private IList<Notificacao> _listNotificacao;

        public NotificadorService()
        {
            _listNotificacao = new List<Notificacao>();
        }

        public void Handle(Notificacao notificacao)
        {
            _listNotificacao.Add(notificacao);
        }

        public IEnumerable<Notificacao> ObterNotificacoes()
        {
            return _listNotificacao;
        }

        public bool TemNoficacao()
        {
            return _listNotificacao.Any();
        }
    }
}
