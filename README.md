## Sobre o projeto

![sonarcloud-qualityGate]
![sonarcloud-bugs]
![sonarcloud-vulnerabilities]
![sonarcloud-code-smells]
![sonarcloud-coverage]
![sonarcloud-duplicated-lines]

Apresentando o **Meu Livro de Receitas** - uma aplicação para quem adora cozinhar e compartilhar receitas! O Meu Livro de Receitas foi projetado para tornar sua vida na cozinha mais fácil, ajudando você a se organizar, gerenciar suas receitas e tornar sua experiência culinária mais agradável.

Este projeto consiste em uma **API** desenvolvida em **.NET** para o gerenciamento de receitas culinárias. A **API** permite que os usuários se cadastrem fornecendo nome, e-mail e senha. Após o cadastro, os usuários podem criar, editar, filtrar e deletar receitas. Cada receita deve incluir um título, ingredientes e instruções. Adicionalmente, os usuários têm a opção de adicionar o tempo de preparo, nível de dificuldade e uma imagem ilustrativa à receita.

A **API** oferece suporte para **MySQL** e **SQLServer** como opções de banco de dados, proporcionando flexibilidade na escolha do ambiente de armazenamento de dados. A configuração de pipelines **CI/CD** e a integração com **Sonarcloud** garantem uma análise contínua do código, promovendo um desenvolvimento mais robusto e seguro.

Seguindo os princípios de **Domain-Driven Design (DDD)** e **SOLID**, a arquitetura do projeto busca manter um design modular e sustentável. A validação dos dados é realizada utilizando **FluentValidation**, assegurando que todas as entradas de dados atendam aos critérios estabelecidos.

Para garantir a qualidade do código, são implementados **testes de unidade e de integração**. A utilização de **injeção de dependências** promove uma melhor modularidade e testabilidade do código, facilitando a manutenção e evolução do projeto.

Outras tecnologias e práticas adotadas incluem o **Entity Framework** para o mapeamento objeto-relacional, a metodologia ágil **SCRUM** para o gerenciamento do projeto, e a implementação de **Tokens JWT & Refresh Token** para autenticação segura. As migrações do banco de dados são gerenciadas para assegurar uma evolução controlada do esquema de dados. Além disso, o uso de **Git** e a estratégia de ramificação **GitFlow** auxiliam na organização e controle das versões do código.

# **Curso na Udemy**:
### [.NET Core: um curso orientado para o mercado de trabalho][curso-udemy]

Este projeto inclui um curso **abrangente** que guia os alunos e alunas sobre **como criar uma API** robusta de Gerenciamento de Receitas usando a tecnologia **.NET**. Ao longo do curso, os alunos e alunas aprenderão a projetar e desenvolver a funcionalidade de uma API, **implementar segurança, realizar testes, configurar pipelines de CI/CD e publicar o projeto no Azure**.

O curso é destinado a desenvolvedores que já possuem experiência com programação e conhecimento em C# (.NET). Ele fornece todas as informações e recursos necessários para ajudar os alunos a aperfeiçoarem suas habilidades, desde a configuração do ambiente de desenvolvimento até a implementação e teste de APIs seguras e eficientes.

Durante o curso, você aprenderá:

- Cadastro de Usuários: Como permitir o registro de usuários com validação de email e senha.
- Gerenciamento de Receitas: Criação, edição, exclusão e filtro de receitas.
- Login com Google: Integração para autenticação via conta Google.
- Integração com ChatGPT: Utilização de IA para melhorar a experiência do usuário.
- Mensageria: Utilização de mensageria para gerenciar a exclusão de contas.
- Segurança: Implementação de JWT e Refresh Token para segurança de autenticação.
- Banco de Dados: Configuração e uso de MySQL ou SQLServer.
- DevOps: Configuração de pipelines CI/CD e integração com Sonarcloud para análise contínua.
- Arquitetura: Princípios de Domain-Driven Design (DDD) e SOLID.
- Validação: Utilização de FluentValidation para validação de dados.
- Testes: Implementação de testes de unidade e de integração para garantir a qualidade do código.
- Injeção de Dependências: Uso de injeção de dependências para melhor modularidade e testabilidade do código.

Para acessar o curso, basta clicar [neste link][curso-udemy]. 

Tenho certeza de que este projeto e curso serão fundamentais para aprimorar suas habilidades e elevar sua proficiência como desenvolvedor Backend.


![hero-image]

### Features

- **Gerenciamento de Receitas**: Criação, edição, exclusão e filtro de receitas. 🍲✏️🗑️🔍
- **Login com Google**: Integração para autenticação via conta Google. 🔑🔗🟦
- **Integração com ChatGPT**: Utilização de IA para melhorar a experiência dos usuários na geração de receitas a partir de ingredientes fornecidos. 🤖🍳
- **Mensageria**: Utilização de mensageria (Service Bus - Queue), para gerenciar a exclusão de contas. 📩🗂️🚫
- **Upload de Imagem**: Permite aos usuários enviar uma imagem para ilustrar suas receitas. 📸⬆️🖼️

### Construído com

![badge-dot-net]
![badge-windows]
![badge-visual-studio]
![badge-mysql]
![badge-sqlserver]
![badge-swagger]
![badge-docker]
![badge-azure-devops]
![badge-azure]
![badge-azure-pipelines]
![badge-google]
![badge-openai]
![badge-sonarcloud]

## Getting Started

Para obter uma cópia local funcionando, siga estes passos simples.

### Requisitos

* Visual Studio versão 2022+ ou Visual Studio Code
* Windows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk] instalado
* MySql Server ou SqlServer

### Instalação

1. Clone o repositório:
    ```sh
    git clone https://github.com/welissonArley/MyRecipeBook.git
    ```

2. Preencha as informações no arquivo `appsettings.Development.json`.
3. Execute a API e aproveite o seu teste :)

## License

Sinta-se à vontade para usar este projeto para estudar e aprender. No entanto, a distribuição ou comercialização não é permitida.

<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[curso-udemy]: https://www.udemy.com/course/net-core-curso-orientado-para-mercado-de-trabalho/?referralCode=C0850BF224055DE39722

<!-- Images -->
[hero-image]: images/heroheader.png

<!-- Sonarcloud -->
[sonarcloud-dashboard]: https://sonarcloud.io/summary/overall?id=welissonArley_MyRecipeBook
[sonarcloud-qualityGate]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=alert_status
[sonarcloud-bugs]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=bugs
[sonarcloud-vulnerabilities]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=vulnerabilities
[sonarcloud-code-smells]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=code_smells
[sonarcloud-coverage]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=coverage
[sonarcloud-duplicated-lines]: https://sonarcloud.io/api/project_badges/measure?project=welissonArley_MyRecipeBook&metric=duplicated_lines_density

<!-- Badges -->
[badge-sqlserver]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-windows]: https://img.shields.io/badge/Windows-0078D4?logo=windows&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[badge-docker]: https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff&style=for-the-badge
[badge-azure-devops]: https://img.shields.io/badge/Azure%20DevOps-0078D7?logo=azuredevops&logoColor=fff&style=for-the-badge
[badge-azure]: https://img.shields.io/badge/Microsoft%20Azure-0078D4?logo=microsoftazure&logoColor=fff&style=for-the-badge
[badge-azure-pipelines]: https://img.shields.io/badge/Azure%20Pipelines-2560E0?logo=azurepipelines&logoColor=fff&style=for-the-badge
[badge-google]: https://img.shields.io/badge/Google-4285F4?logo=google&logoColor=fff&style=for-the-badge
[badge-openai]: https://img.shields.io/badge/OpenAI-412991?logo=openai&logoColor=fff&style=for-the-badge
[badge-sonarcloud]: https://img.shields.io/badge/SonarCloud-F3702A?logo=sonarcloud&logoColor=fff&style=for-the-badge