using System;
using Newtonsoft.Json;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Anotacao
    {
        public string Texto { get; private set; }
        public DateTime DataCriacao { get; private set; }

        [JsonConstructor]
        private Anotacao(string texto, DateTime dataCriacao)
        {
            Texto = texto;
            DataCriacao = dataCriacao;
        }

        internal Anotacao(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Texto da anotação é obrigatório.");

            Texto = texto;
            DataCriacao = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{DataCriacao}] {Texto}";
        }
    }
}
