# Sistema de Agenda de Compromissos

Projeto de console em C# que simula uma **agenda de compromissos** com gerenciamento de:

-Usuários 

-Participantes

-Locais

-Anotações

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

## Estrutura do Projeto

/AGEND-GENERAL-API/

   |-Modelos/
  
 Anotação.cs
 
 Compromisso.cs
 
 Local.cs
 
 Participante.cs
 
 Usuario.cs
 
   |-Program.cs
  
   |-README.md

## Conceitos Aplicados
-**Associação Simples**: 'Compromisso' contén referencia a usuário e 'Local'

-**Associação N:N**: 'Compromisso' e 'participante' se referenciam mutualmente

-**Composição**: 'Compromisso' possui lista de 'Anotação', criada internamente

-**Encapsulamento**:coleções internas são protegidas com 'IReadOnlyCollection<T>'.

## Validações Importantes

-Data/hora dos compromissos devem ser futuras.

-Descrição é obrigatória.

-Número de participantes não pode ultrapassar a capacidade do local.

## Autora

Laura Kauana Bareto

**email**: laura.bareto@alunos.utfpr.edu.com
