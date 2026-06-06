# Meu Livro de Receitas

![hero-project-image]

## Sobre o projeto

**Meu Livro de Receitas** é uma API desenvolvida em **.NET** para gerenciamento de receitas culinárias.

A aplicação permite que usuários se cadastrem, façam login e gerenciem suas próprias receitas de forma simples e organizada. Cada receita pode conter título, ingredientes, modo de preparo, tempo de preparo, nível de dificuldade e uma imagem ilustrativa.

Além do CRUD de receitas, o projeto também aborda recursos comuns em aplicações reais, como autenticação com JWT, Refresh Token, login com Google, upload de imagens, integração com IA, mensageria, testes automatizados, pipelines CI/CD e análise de cobertura de código.

Este projeto foi criado com foco em boas práticas de desenvolvimento backend, organização de código e construção de uma API próxima do que é utilizado no mercado de trabalho.

---

## Curso na Udemy

### [.NET Core: um curso orientado para o mercado de trabalho][curso-udemy]

Este repositório faz parte do curso **.NET Core: um curso orientado para o mercado de trabalho**, disponível na Udemy.

Durante o curso, construímos uma API completa do zero, passando por arquitetura, banco de dados, autenticação, testes, Docker, Azure DevOps, pipelines, SonarCloud e boas práticas de desenvolvimento.

O curso é indicado para pessoas que já possuem conhecimento em lógica de programação e C#, e querem aprender a desenvolver APIs mais completas, organizadas e preparadas para cenários reais.

Para acessar o curso, clique [neste link][curso-udemy].

![hero-course-image]

---

## O que você vai aprender no curso

Durante o curso, você aprenderá a:

- Criar uma API REST com .NET
- Estruturar uma solução usando conceitos de DDD
- Aplicar princípios de SOLID
- Trabalhar com Entity Framework
- Criar cadastro e autenticação de usuários
- Implementar JWT e Refresh Token
- Integrar login com Google
- Criar CRUD completo de receitas
- Fazer upload de imagens
- Integrar a API com AI
- Utilizar mensageria com Azure Service Bus
- Criar testes de unidade
- Criar testes de integração
- Usar Testcontainers para os testes de integração
- Configurar Docker no ambiente de desenvolvimento
- Criar pipelines no Azure DevOps
- Publicar cobertura de testes no pipeline
- Organizar o desenvolvimento usando práticas de SCRUM

---

## Features

- **Cadastro de usuários**  
  Permite que usuários criem uma conta utilizando nome, e-mail e senha.

- **Autenticação segura**  
  Implementação de autenticação com JWT e Refresh Token.

- **Login com Google**  
  Integração com autenticação via conta Google.

- **Gerenciamento de receitas**  
  Criação, edição, exclusão, listagem e filtro de receitas.

- **Upload de imagem**  
  Permite adicionar uma imagem ilustrativa para cada receita.

- **Integração com AI**  
  Geração de receitas completas com imagens, com o apoio de inteligência artificial.

- **Mensageria**  
  Uso de Azure Service Bus para processamento assíncrono, como exclusão de contas.

- **Suporte a múltiplos bancos de dados**  
  Compatível com MySQL e SQL Server.

- **Testes automatizados**  
  Testes de unidade e testes de integração para garantir a qualidade da aplicação.

- **CI/CD**  
  Pipeline configurado no Azure DevOps com build, testes e análise de cobertura.

- **Análise de cobertura de código**  
  Visualização e interpretação da cobertura de testes para identificar partes do código que precisam de mais validação.

---

## Construído com

![badge-c#]
![badge-dot-net]
![badge-windows]
![badge-visual-studio]
![badge-mysql]
![badge-sqlserver]
![badge-swagger]
![badge-docker]
![badge-azure-devops]
![badge-azure]
![badge-yaml]
![badge-gmail]
![badge-openai]

---

## Arquitetura

O projeto segue uma organização baseada em **Domain-Driven Design (DDD)**, com separação de responsabilidades entre as camadas da aplicação e os projetos compartilhados.

A estrutura principal da solução é organizada da seguinte forma:

```txt
src/
 ├── Backend/
 │   ├── Api/
 │   ├── Application/
 │   ├── Domain/
 │   └── Infrastructure/
 │
 └── Shared/
     ├── Communication/
     └── Exception/

tests/
 ├── UseCases.Tests/
 ├── Validators.Tests/
 └── WebApi.Tests/
```

### Principais responsabilidades

#### Backend

Contém os projetos principais da API e concentra as regras, fluxos e integrações da aplicação.

- **API**
  Responsável por expor os endpoints, configurar middlewares, autenticação, documentação com Swagger e inicialização da aplicação.

- **Application**
  Contém os casos de uso da aplicação (Regras de negócio) com as validações e transormações necessárias para executar os fluxos do sistema.

- **Domain**
  Contém entidades e contratos principais.

- **Infrastructure**
  Responsável por implementações externas, como acesso ao banco de dados, serviços de autenticação, envio de e-mails, integrações e persistência.

#### Shared

Contém projetos compartilhados que podem ser utilizados por diferentes partes da solução.

- **Communication**
  Contém os objetos utilizados na comunicação da API, como requests, responses e contratos de entrada e saída.

- **Exception**
  Centraliza as exceções personalizadas e estruturas relacionadas ao tratamento de erros da aplicação.

#### Tests

Contém os projetos responsáveis por validar o comportamento da aplicação em diferentes níveis.

- **UseCases.Tests**
  Contém testes voltados para os casos de uso da aplicação, validando regras, fluxos e comportamentos da camada de Application.

- **Validators.Tests**
  Contém testes específicos para as validações da aplicação, garantindo que os dados de entrada sejam avaliados corretamente.

- **WebApi.Tests**
  Contém testes de integração da API, o comportamento real da API, incluindo banco de dados e configurações da aplicação.

## CI/CD e qualidade de código

O projeto utiliza **Azure DevOps Pipelines** para automatizar etapas importantes do desenvolvimento, como:

- Restore dos pacotes
- Build da solução
- Execução dos testes
- Publicação da cobertura de testes
- Publicação da imagem Docker com a API no Azure Container Registry

## Como executar o projeto

Para executar o projeto localmente, siga os passos abaixo.

### Requisitos

- Visual Studio 2026 ou Rider
- .NET SDK
- Docker Desktop
- MySQL Server ou SQL Server
- Git

---

## Instalação

Clone o repositório:

```sh
git clone https://github.com/welissonArley/MyRecipeBook.git
```

Acesse a pasta do projeto:

```sh
cd MyRecipeBook
```

Configure o arquivo `appsettings.Development.json` com as informações necessárias, como conexão com banco de dados, JWT, Google, Azure Service Bus e OpenAI, conforme os recursos que deseja executar.

Depois, execute a API pelo Visual Studio ou usando o comando:

```sh
dotnet run
```

Acesse a documentação da API pelo Swagger:

```txt
https://localhost:7070/swagger
```

---

## Executando os testes

Para executar todos os testes da solução:

```sh
dotnet test
```

Para os testes de integração, certifique-se de que o Docker esteja em execução.

---

## Licença

Este projeto está disponível para fins de estudo e aprendizado.

A distribuição, revenda ou uso comercial do conteúdo do curso e dos materiais associados não é permitida sem autorização.

---

## Autor

Criado por **Welisson Arley**.

Desenvolvedor .NET, mentor e instrutor, com experiência em desenvolvimento backend, arquitetura de software, APIs, testes automatizados, Azure DevOps e boas práticas de desenvolvimento.

---

<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
[curso-udemy]: https://www.udemy.com/course/net-core-curso-orientado-para-mercado-de-trabalho/?referralCode=C0850BF224055DE39722

<!-- Images -->
[hero-project-image]: images/heroProjectImage.png
[hero-course-image]: images/heroCourseImage.png

<!-- Badges -->
[badge-c#]: https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white&style=for-the-badge
[badge-sqlserver]: https://custom-icon-badges.demolab.com/badge/SQL%20Server-CC2927?logo=mssqlserver-white&logoColor=white&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-windows]: https://custom-icon-badges.demolab.com/badge/Windows-0078D6?logo=windows11&logoColor=white&style=for-the-badge
[badge-visual-studio]: https://custom-icon-badges.demolab.com/badge/Visual%20Studio-5C2D91.svg?&logo=visualstudio&logoColor=white&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[badge-docker]: https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff&style=for-the-badge
[badge-azure-devops]: https://custom-icon-badges.demolab.com/badge/Azure%20DevOps-0078D7?logo=azure-devops-white&logoColor=fff&style=for-the-badge
[badge-azure]: https://custom-icon-badges.demolab.com/badge/Microsoft%20Azure-0089D6?logo=msazure&logoColor=white&style=for-the-badge
[badge-gmail]: https://img.shields.io/badge/Gmail-D14836?logo=gmail&logoColor=white&style=for-the-badge
[badge-openai]: https://custom-icon-badges.demolab.com/badge/OpenAI-74aa9c?logo=openai&logoColor=white&style=for-the-badge
[badge-yaml]: https://img.shields.io/badge/YAML-CB171E?logo=yaml&logoColor=fff&style=for-the-badge