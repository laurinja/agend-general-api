
#  Sistema de Agenda de Compromissos

Projeto de console em **C#** que simula uma **agenda de compromissos**, com funcionalidades de gerenciamento de:

-  Usuários
-  Participantes
-  Locais
-  Anotações
-  Compromissos

> **Persistência** dos dados em arquivos JSON e interface via **menu no console (CLI interativo)**.

---

##  Objetivo

Aplicar os princípios de **Programação Orientada a Objetos (POO)**, como:

-  Abstração
-  Encapsulamento
-  Associação simples e N:N
-  Composição
-  Validação de dados
-  Persistência de informações em JSON

---

##  Funcionalidades

-  Cadastro de usuários
-  Registro de compromissos com:
  - Data, hora, local e descrição
  - Validação de data/hora no futuro
  - Verificação da capacidade do local
-  Associação de múltiplos participantes
-  Criação de anotações internas para cada compromisso
-  Listagem completa dos compromissos com todos os detalhes
-  Salvamento automático dos dados no arquivo `usuarios.json`

---

##  Estrutura do Projeto

```
/Agenda-General-Api/
│
├── Modelos/
│   ├── Usuario.cs
│   ├── Compromisso.cs
│   ├── Participante.cs
│   ├── Anotacao.cs
│   └── Local.cs
│
├── Program.cs
├── usuarios.json (gerado automaticamente)
├── README.md
```

---

##  Conceitos Aplicados

- **Associação Simples:**  
  → Cada `Compromisso` possui um `Usuario` (criador) e um `Local`.

- **Associação N:N:**  
  → `Compromisso` e `Participante` possuem relação bidirecional (um participante pode estar em vários compromissos e vice-versa).

- **Composição:**  
  → `Compromisso` contém uma lista de `Anotacao` (anotações internas que não existem fora do compromisso).

- **Encapsulamento:**  
  → As coleções internas são protegidas, expostas apenas para leitura com `IReadOnlyCollection<T>`.

---

##  Validações Implementadas

-  **Data e hora:** obrigatoriamente no futuro.
-  **Descrição:** campo obrigatório para todo compromisso.
-  **Capacidade:** não permite adicionar mais participantes do que a capacidade do local.

---

##  Persistência dos Dados

-  Todos os dados são armazenados em `usuarios.json`.
-  Salvamento automático após cada operação (criação de usuários ou compromissos).
-  Leitura dos dados na inicialização.

---

##  Como Executar

1. Clone o repositório:

```bash
git clone https://github.com/laurinja/agend-general-api.git
cd agend-general-api
```

2. Compile e execute o projeto:

```bash
dotnet build
dotnet run
```

3. Siga as instruções no menu do console.

---

##  Exemplo de Uso

```plaintext
Sistema de Agendas de Compromissos

Insira o nome completo: Laura Kauana Barreto
Bem-vindo de volta, Laura Kauana Barreto!

Escolha uma opção do menu:
1 - Novo compromisso
2 - Listar compromissos
3 - Sair
```


##  Autora

**Laura Kauana Barreto**  
email: laurabareto@alunos.utfpr.edu.br  

