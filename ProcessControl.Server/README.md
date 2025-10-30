# ProcessControl.Server

Este projeto é uma API RESTful desenvolvida em .NET 8 para gerenciar processos e seus históricos. Ele segue uma arquitetura limpa e modular para facilitar a manutenção e escalabilidade.

## Arquitetura

A arquitetura do projeto é organizada em camadas, promovendo a separação de responsabilidades e a modularidade:

*   **ProcessControl.API (Camada de Apresentação):** Contém os controladores (Controllers) que expõem os endpoints da API. É responsável por receber as requisições HTTP, orquestrar as chamadas para a camada de aplicação e retornar as respostas.

*   **ProcessControl.Application (Camada de Aplicação):** Contém a lógica de negócio específica da aplicação. Inclui DTOs (Data Transfer Objects), interfaces de serviço (Services) e suas implementações, e validadores. Esta camada coordena as operações e interage com a camada de domínio e infraestrutura.

*   **ProcessControl.Domain (Camada de Domínio):** O coração da aplicação, contendo as entidades de negócio (Entities) e a lógica de domínio. É independente de qualquer tecnologia ou framework específico, focando puramente nas regras de negócio.

*   **ProcessControl.Infrastructure (Camada de Infraestrutura):** Responsável pela persistência de dados (usando Entity Framework Core), implementações de repositórios (Repositories) e outras preocupações de infraestrutura, como acesso a serviços externos ou configurações.

## Tecnologias Utilizadas

*   **Framework:** .NET 8
*   **Linguagem:** C#
*   **Banco de Dados:** PostgreSQL (via Entity Framework Core)
*   **ORM:** Entity Framework Core

## Domínio da API

A API gerencia duas entidades principais:

### Processo
Representa um processo que está sendo monitorado ou gerenciado. Possui as seguintes características:
*   Identificador único.
*   Nome do processo.
*   Descrição.
*   Status atual.

### Histórico de Processo (HistoricoProcesso)
Registra as mudanças e eventos relacionados a um `Processo` específico. Cada entrada de histórico está associada a um `Processo` e contém:
*   Identificador único.
*   Referência ao `Processo` ao qual pertence.
*   Data e hora do evento.
*   Descrição do evento ou mudança.
*   Novo status (se aplicável).

---
