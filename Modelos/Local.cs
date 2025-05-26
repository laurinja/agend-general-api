using System;
using Newtonsoft.Json;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Local 
    {
        public string Nome { get; private set; }
        public int CapacidadeMaxima { get; private set; }

        [JsonConstructor]
        private Local(string nome, int capacidadeMaxima)
        {
            Nome = nome;
            CapacidadeMaxima = capacidadeMaxima;
        }

        public Local(string nome, int capacidadeMaxima, bool validar = true)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do local é obrigatório.");

            if (capacidadeMaxima <= 0)
                throw new ArgumentException("Capacidade deve ser maior que zero.");

            Nome = nome;
            CapacidadeMaxima = capacidadeMaxima;
        }

        public void ValidarCapacidade(int quantidade)
        {
            if (quantidade > CapacidadeMaxima)
                throw new InvalidOperationException("Quantidade de participantes excede a capacidade do local.");
        }

        public override string ToString()
        {
            return $"{Nome} (Capacidade: {CapacidadeMaxima})";
        }
    }
}