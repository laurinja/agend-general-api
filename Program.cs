using System.Globalization;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using AgendaDeCompromissos.AgendaCompromisso;

CultureInfo culturaBrasileira = new("pt-BR");
Console.WriteLine("Sistema de Agendas de Compromissos");

const string arquivoUsuarios = "usuarios.json";
List<Usuario> usuarios = CarregarUsuarios();

string nomeCompleto = LerEntrada("Insira o nome completo: ");
Usuario usuario = BuscarOuCriarUsuario(nomeCompleto, usuarios, SalvarUsuarios);

while (true)
{
    Console.WriteLine("\nEscolha uma opção do menu: ");
    Console.WriteLine("1 - Novo compromisso");
    Console.WriteLine("2 - Listar compromissos");
    Console.WriteLine("3 - Sair");
    string opcao = Console.ReadLine();

    switch (opcao)
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

static List<Usuario> CarregarUsuarios()
{
    if (File.Exists(arquivoUsuarios))
    {
        string json = File.ReadAllText(arquivoUsuarios);
        return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
    }
    return new List<Usuario>();
}

static void SalvarUsuarios(List<Usuario> usuarios)
{
    var settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    string json = JsonConvert.SerializeObject(usuarios, settings);
    File.WriteAllText(arquivoUsuarios, json);
}

static string Normalizar(string texto) =>
    new string(texto.Normalize(NormalizationForm.FormD)
        .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        .ToArray())
    .ToLowerInvariant()
    .Trim();

static Usuario BuscarOuCriarUsuario(string nomeCompleto, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    string nomeNormalizado = Normalizar(nomeCompleto);
    Usuario usuario = usuarios.FirstOrDefault(u => Normalizar(u.NomeCompleto) == nomeNormalizado);

    if (usuario == null)
    {
        usuario = new Usuario(nomeCompleto);
        usuarios.Add(usuario);
        Console.WriteLine($"Usuário {usuario.NomeCompleto} criado!");
        salvar(usuarios);
    }
    else
    {
        Console.WriteLine($"Bem-vindo de volta, {usuario.NomeCompleto}!");
    }
    return usuario;
}

static string LerEntrada(string mensagem)
{
    Console.Write(mensagem);
    return Console.ReadLine()?.Trim();
}

static void RegistrarCompromisso(Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvarUsuarios)
{
    string nomeLocal = LerEntrada("Insira o nome do local: ");
    int capacidade = LerInteiro("Insira a capacidade máxima do local: ");
    Local local;
    try
    {
        local = new Local(nomeLocal, capacidade);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Erro ao criar local: {ex.Message}");
        return;
    }

    DateTime dataHora = LerDataHora();
    string descricao = LerEntrada("Insira uma descrição para o compromisso: ");

    try
    {
        Compromisso compromisso = new(dataHora, descricao, usuario, local);

        if (LerEntrada("Deseja inserir um participante? (s/n): ").ToLower() == "s")
            AdicionarParticipantes(compromisso);

        if (LerEntrada("Deseja inserir uma anotação? (s/n): ").ToLower() == "s")
            AdicionarAnotacoes(compromisso);

        usuario.AdicionarCompromisso(compromisso);
        salvarUsuarios(usuarios);
        Console.WriteLine("\nCompromisso registrado com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro no registro do compromisso: {ex.Message}");
    }
}

static int LerInteiro(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        if (int.TryParse(Console.ReadLine(), out int valor))
            return valor;
        Console.WriteLine("Valor inválido. Tente novamente.");
    }
}

static DateTime LerDataHora()
{
    while (true)
    {
        try
        {
            string dataStr = LerEntrada("Digite a data do compromisso (dd/MM/aaaa): ");
            DateTime data = DateTime.ParseExact(dataStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string horaStr = LerEntrada("Insira a hora do compromisso (HH:mm): ");
            TimeSpan hora = TimeSpan.ParseExact(horaStr, "hh\\:mm", CultureInfo.InvariantCulture);

            return data.Add(hora);
        }
        catch
        {
            Console.WriteLine("Data ou hora inválida. Tente novamente.");
        }
    }
}

static void AdicionarParticipantes(Compromisso compromisso)
{
    while (true)
    {
        string nomeParticipante = LerEntrada("Insira o nome do participante ou vazio para cancelar a inserção: ");
        if (string.IsNullOrWhiteSpace(nomeParticipante)) break;
        try
        {
            Participante participante = new(nomeParticipante);
            compromisso.AdicionarParticipante(participante);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}

static void AdicionarAnotacoes(Compromisso compromisso)
{
    while (true)
    {
        string anotacao = LerEntrada("Insira uma anotação ou vazio para cancelar a inserção: ");
        if (string.IsNullOrWhiteSpace(anotacao)) break;
        compromisso.AdicionarAnotacao(anotacao);
    }
}

static void ListarCompromissos(Usuario usuario)
{
    Console.WriteLine("\nCompromissos registrados: ");
    if (usuario.Compromissos.Count == 0)
    {
        Console.WriteLine("Não há compromissos registrados.");
        return;
    }
    foreach (var compromisso in usuario.Compromissos)
    {
        Console.WriteLine($"\n{compromisso}");

        if (compromisso.Participantes.Count > 0)
        {
            Console.WriteLine("Participantes: ");
            foreach (var participante in compromisso.Participantes)
                Console.WriteLine($"- {participante.NomeCompleto}");
        }
        else
        {
            Console.WriteLine("Não há participantes registrados.");
        }

        if (compromisso.Anotacoes.Count > 0)
        {
            Console.WriteLine("Anotações: ");
            foreach (var anotacao in compromisso.Anotacoes)
                Console.WriteLine($"- {anotacao.Texto}");
        }
        else
        {
            Console.WriteLine("Não há anotações registradas.");
        }
    }
}