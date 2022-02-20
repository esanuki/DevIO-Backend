using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificadorService _notificador;

        protected BaseService(INotificadorService notificador)
        {
            _notificador = notificador;
        }
        
        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var item in validationResult.Errors)
            {
                Notificar(item.ErrorMessage);
            }
        }

        protected bool ExecutarValidacao<V, E>(V validacao, E entity) where V : AbstractValidator<E> where E : Entity
        {
            var validator = validacao.Validate(entity);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}
