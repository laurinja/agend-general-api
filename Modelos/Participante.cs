using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Participante
    {
        public string NomeCompleto { get; private set; }

        private List<Compromisso> _compromissos = new List<Compromisso>();
        public IReadOnlyCollection<Compromisso> Compromissos => _compromissos.AsReadOnly();

        [JsonConstructor]
        private Participante(string nomeCompleto, List<Compromisso> compromissos = null)
        {
            NomeCompleto = nomeCompleto;
            if (compromissos != null)
                _compromissos = compromissos;
        }

        public Participante(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new ArgumentException("Nome do participante é obrigatório.");

            NomeCompleto = nomeCompleto;
        }

        public void AdicionarCompromisso(Compromisso compromisso)
        {
            if (compromisso == null)
                throw new ArgumentNullException(nameof(compromisso));

            if (!_compromissos.Contains(compromisso))
            {
                _compromissos.Add(compromisso);
                compromisso.AdicionarParticipante(this);
            }
        }

        public override string ToString()
        {
            return NomeCompleto;
        }
    }
}
