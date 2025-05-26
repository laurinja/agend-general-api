using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Usuario
    {
        public string NomeCompleto { get; private set; }

        private List<Compromisso> _compromissos = new List<Compromisso>();
        public IReadOnlyCollection<Compromisso> Compromissos => _compromissos.AsReadOnly();

        [JsonConstructor]
        private Usuario(string nomeCompleto, List<Compromisso> compromissos = null)
        {
            NomeCompleto = nomeCompleto;
            if (compromissos != null)
                _compromissos = compromissos;
        }

        public Usuario(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new ArgumentException("Nome do usuário é obrigatório.");

            NomeCompleto = nomeCompleto;
        }

        public void AdicionarCompromisso(Compromisso compromisso)
        {
            if (compromisso == null)
                throw new ArgumentNullException(nameof(compromisso),"Compromisso não pode ser nulo.");

            if (!_compromissos.Contains(compromisso))
                _compromissos.Add(compromisso);
        }
    }
}
