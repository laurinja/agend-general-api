using System.Globalization;
using System.Runtime.InteropServices;
using AgendaDeCompromissos;
using AgendaDeCompromissos.AgendaCompromisso;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

CultureInfo culturaBrasileira = new("pt-BR");

Console.WriteLine("Sistema de Agendas de Compromissos");

const string arquivoUsuarios = "usuarios.json";
List<Usuario> usuarios = new List<Usuario>();

if (File.Exists(arquivoUsuarios))
{
    string json = File.ReadAllText(arquivoUsuarios);
    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
}

Console.Write("Insira o nome completo: ");
string nomeCompleto = Console.ReadLine()?.Trim();

string Normalizar(string texto) =>
    new string(texto.Normalize(System.Text.NormalizationForm.FormD)
        .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        .ToArray())
    .ToLowerInvariant()
    .Trim();

string nomeNormalizado = Normalizar(nomeCompleto);

Usuario usuario = usuarios.FirstOrDefault(u => Normalizar(u.NomeCompleto) == nomeNormalizado);

if (usuario == null)
{
    usuario = new Usuario(nomeCompleto);
    usuarios.Add(usuario);
    Console.WriteLine($"Usuário {usuario.NomeCompleto} criado!");
    SalvarUsuarios(usuarios);
}
else
{
    Console.WriteLine($"Bem-vindo de volta, {usuario.NomeCompleto}!");
}

void SalvarUsuarios(List<Usuario> usuarios)
{
    var settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    string json = JsonConvert.SerializeObject(usuarios, settings);
    File.WriteAllText(arquivoUsuarios, json);
}

while(true)
{
    Console.WriteLine("\nEscolha uma opção do menu: ");
    Console.WriteLine("1 - Novo compromisso");
    Console.WriteLine("2 - Listar compromissos");
    Console.WriteLine("3 - Sair");
    string opcao = Console.ReadLine();

    switch(opcao)
    {
        case "1":
            RegistrarCompromisso(usuario, usuarios, SalvarUsuarios);
            break;
        case "2":
            ListarCompromissos(usuario);
            break;
        case "3":
            return;
        default:
            Console.WriteLine("Opção inválida. Insira uma opção válida.");
            break; 
            
    }
}

static void RegistrarCompromisso(Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvarUsuarios)
{
    DateTime? data = null;
    TimeSpan? hora = null;
    string descricao = null;
    string nomeLocal = null;
    int? capacidade = null; 

    while(string.IsNullOrWhiteSpace(nomeLocal) || capacidade == null)
    {
        Console.Write("Insira o nome do local: ");
        nomeLocal = Console.ReadLine();

        while(capacidade == null)
        {
            Console.Write("Insira a capacidade máxima do local: ");
            var capacidadeInserida = Console.ReadLine();
            try
            {
                capacidade = int.Parse(capacidadeInserida);
            }
            catch(FormatException)
            {
                Console.WriteLine($"{capacidadeInserida} não é um número válido. Insira um número válido.");
            }
        }
        try
        {
            Local local = new(nomeLocal, capacidade.Value);
        }
        catch(ArgumentException excecao)
        {
            Console.WriteLine($"Erro ao criar local: {excecao.Message}");
            nomeLocal = null;
            capacidade = null;
        }
    }

    while(data == null||hora == null||string.IsNullOrWhiteSpace(descricao))
    {
        while(data == null)
        {
            Console.Write("Digite a data do compromisso (dd/MM/aaaa): ");
            var dataInserida = Console.ReadLine();
            try
            {
                data = DateTime.ParseExact(dataInserida, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch(FormatException)
            {
                Console.WriteLine($"{dataInserida} não é uma data válida. Insira uma data válida.");
            }
        }
        while(hora == null)
        {
            Console.Write("Insira a hora do compromisso (HH:mm): ");
            var horaInserida = Console.ReadLine();
            try
            {
                hora = TimeSpan.ParseExact(horaInserida, "hh\\:mm", CultureInfo.InvariantCulture);
            }
            catch(FormatException)
            {
                Console.WriteLine($"{horaInserida} não tem um formato válido. Insira um formato válido (HH:mm).");
            }
            catch(OverflowException)
            {
                Console.WriteLine($"{horaInserida} não é um horário válido. Insira um horário válido (00:00 - 23:59).");
            }
        }
        
        DateTime dataHora = data.Value.Add(hora.Value);

        Console.Write("Insira uma descrição para o compromisso: ");
        descricao = Console.ReadLine();

        try
        {
            Local local = new(nomeLocal, capacidade.Value);
            Compromisso compromisso = new(dataHora, descricao, usuario, local);

            Console.Write("Deseja inserir um participante? (s/n): ");
            if(Console.ReadLine()?.ToLower()=="s")
            {
                try
                {
                    while(true)
                    {
                        Console.WriteLine("Insira o nome do participante ou vazio para cancelar a inserção: ");
                        string nomeParticipante = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nomeParticipante)) break;

                        Participante participante = new(nomeParticipante);
                        compromisso.AdicionarParticipante(participante);
                    }
                }
                catch(Exception excecao)
                {
                    Console.WriteLine($"{excecao.Message}");
                    Console.WriteLine($"Nenhum participante adicionado");
                }
            }
            Console.Write("Deseja inserir uma anotação? (s/n): ");
            if(Console.ReadLine()?.ToLower() == "s")
            {
                while(true)
                {
                    Console.Write("Insira um anotação ou vazio para cancelar a inserção: ");
                    string anotaçãoInserida = Console.ReadLine();
                    if(string.IsNullOrWhiteSpace(anotaçãoInserida)) break;
                    compromisso.AdicionarAnotacao(anotaçãoInserida);
                }
            }

            usuario.AdicionarCompromisso(compromisso);
            salvarUsuarios(usuarios);
            Console.WriteLine("\nCompromisso registrado com sucesso.");
        }
        catch(Exception excecao)
        {
            Console.WriteLine($"Erro no registro do compromisso: {excecao.Message}");
            data = null;
            hora = null;
            descricao = null;
        }

    }
}

static void ListarCompromissos(Usuario usuario)
{
    Console.WriteLine("\nCompromissos registrados: ");
    if(usuario.Compromissos.Count == 0)
    {
        Console.WriteLine("Não há compromissos registrados.");
        return;
    }
    foreach(var compromisso in usuario.Compromissos)
    {
        Console.WriteLine($"\n{compromisso}");

        if(compromisso.Participantes.Count>0)
        {
            Console.WriteLine("Participantes: ");
            foreach (var participante in compromisso.Participantes)
            {
                Console.WriteLine($"- {participante.NomeCompleto}");
            }
        }
        else
        {
            Console.WriteLine("Não há participantes registrados.");
        }

        if(compromisso.Anotacoes.Count>0)
        {
            Console.WriteLine("Anotações: ");
            foreach (var anotacao in compromisso.Anotacoes)
            {
                Console.WriteLine($"- {anotacao.Texto}");
            }
        }
        else
        {
            Console.WriteLine("Não há anotações registradas.");
        }
    }
}