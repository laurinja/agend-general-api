# Sistema de Agenda de Compromissos

Projeto de console em C# que simula uma **agenda de compromissos** com gerenciamento de:

-Usuários 

-Participantes

-Locais

-Anotações


A aplicação trabalha com persistência dos dados em arquivos JSON, garantindo que as informações sejam salvas entre execuções.

## Objetivo

Aplicar os principios de **Programação Orientada a Objetos** como:

-Abstração 

-Encapsulamento 

-Associação Simples N:N e Composição

## Funcionalidades 

-Cadastro do usuário

-Registro de compromissos com data, hora, local e descrição

-validação de capacidade de local

-Associação de múltiplos participantes a um compromisso

-Criação de anotações internas em cada compromisso

-Listagem de compromisso com todos os detalhes

-Persistência automática dos dados em JSON

-Interface Console ou CLI



## Estrutura do Projeto

/AGEND-GENERAL-API/
├── Modelos/
│   ├── Anotacao.cs
│   ├── Compromisso.cs
│   ├── Local.cs
│   ├── Participante.cs
│   └── Usuario.cs
├── Persistencia/
│   └── RepositorioCompromissos.cs
├── Program.cs
└── README.md

## Persistência dos Dados
-Os dados são armazenados em arquivos JSON.

-Estratégia utilizada: modelo hierárquico, onde:

-Cada compromisso contém seus participantes, local e anotações embutidos no mesmo arquivo.

-Arquivo principal de persistência:
compromissos.json

## Conceitos Aplicados
-**Associação Simples**: Compromisso possui referência ao Usuario e ao Local.

-**Associação N:N**: Compromisso e Participante possuem referências mútuas.


-**Composição**: Compromisso possui uma lista de Anotacao criada internamente (anotações não existem sem um compromisso).

-**Encapsulamento**: As coleções internas são protegidas utilizando IReadOnlyCollection<T>, garantindo que só possam ser manipuladas por métodos controlados da classe.

## Validações Importantes

-Data/hora dos compromissos devem ser futuras.

-Descrição é obrigatória.

-Número de participantes não pode ultrapassar a capacidade do local.

## Autora

Laura Kauana Bareto

**email**: laurabareto@alunos.utfpr.edu.br
