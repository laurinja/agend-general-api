using System;
using System.Collections.Generic;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Usuario
    {
        public string NomeCompleto { get; private set; }

        private List<Compromisso> _compromissos = new List<Compromisso>();
        public IReadOnlyCollection<Compromisso> Compromissos => _compromissos.AsReadOnly();

        public Usuario(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new ArgumentException("Nome do usuário é obrigatório.");

            NomeCompleto = nomeCompleto;
        }

        public void AdicionarCompromisso(Compromisso compromisso)
        {
            if (compromisso == null)
                throw new ArgumentNullException(nameof(compromisso));

            if (!_compromissos.Contains(compromisso))
                _compromissos.Add(compromisso);
        }
    }
}
