using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Compromisso
    {
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; }
        public Usuario Responsavel { get; set; }
        public Local Local { get; set; }

        public List<Participante> _participantes = new List<Participante>();
        public IReadOnlyCollection<Participante> Participantes => _participantes.AsReadOnly();

        public List<Anotacao> _anotacoes = new List<Anotacao>();
        public IReadOnlyCollection<Anotacao> Anotacoes => _anotacoes.AsReadOnly();

        [JsonConstructor]
        private Compromisso(DateTime dataHora, string descricao, Usuario responsavel, Local local, List<Participante> participantes = null, List<Anotacao> anotacoes = null)
        {
            DataHora = dataHora;
            Descricao = descricao;
            Responsavel = responsavel;
            Local = local;
            if (participantes != null)
                _participantes = participantes;
            if (anotacoes != null)
                _anotacoes = anotacoes;
        }

        public Compromisso(DateTime dataHora, string descricao, Usuario responsavel, Local local)
        {
            if (dataHora <= DateTime.Now)
                throw new ArgumentException("A data e hora devem ser no futuro.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.");

            DataHora = dataHora;
            Descricao = descricao;
            Responsavel = responsavel ?? throw new ArgumentNullException(nameof(responsavel));
            Local = local ?? throw new ArgumentNullException(nameof(local));

            Responsavel.AdicionarCompromisso(this);
        }

        public void AdicionarParticipante(Participante participante)
        {
            if (participante == null)
                throw new ArgumentNullException(nameof(participante));

            if (!_participantes.Contains(participante))
            {
                _participantes.Add(participante);
                participante.AdicionarCompromisso(this);
            }

            Local?.ValidarCapacidade(_participantes.Count);
        }

        public void AdicionarAnotacao(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Texto da anotação não pode ser vazio.");

            _anotacoes.Add(new Anotacao(texto));
        }

        public override string ToString()
        {
            return $"{Descricao} em {DataHora} no local {Local?.Nome} com {Participantes.Count} participante(s)";
        }
    }
}
