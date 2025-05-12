using System;

namespace AgendaDeCompromissos.AgendaCompromisso
{
    public class Local 
    {
        public string Nome { get; private set; }
        public int CapacidadeMaxima { get; private set; }

        public Local(string nome, int capacidade)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do local é obrigatório.");

            if (capacidade <= 0)
                throw new ArgumentException("Capacidade deve ser maior que zero.");

            Nome = nome;
            Capacidade = capacidade;
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