# 💸 Payment Gateway Simulator

Uma API RESTful robusta desenvolvida para simular o processamento de transações financeiras (Cartão de Crédito e Pix), focada em escalabilidade, idempotência e resiliência.

O projeto demonstra a aplicação de boas práticas de engenharia de software modernas em um contexto de pagamentos.

## 🚀 Tecnologias & Ferramentas

* **Linguagem:** C# (.NET 9)
* **Banco de Dados:** SQL Server 2022
* **Cache & Performance:** Redis (Implementação de Idempotência)
* **Containerização:** Docker & Docker Compose
* **Documentação:** Swagger / OpenAPI
* **Arquitetura:** Clean Architecture (DDD inspired)

## 🏗️ Estrutura do Projeto

O projeto segue os princípios de separação de responsabilidades:

* **Domain:** Entidades e Interfaces (Core do negócio, sem dependências externas).
* **Infrastructure:** Implementação de acesso a dados (EF Core), integrações (Redis) e serviços externos.
* **API:** Camada de entrada (Controllers) e configuração de injeção de dependência.

## 🛠️ Como Rodar o Projeto

### Pré-requisitos
* Docker e Docker Compose instalados.

### Passo a Passo

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/SEU-USUARIO/PaymentGateway.git](https://github.com/SEU-USUARIO/PaymentGateway.git)
    cd PaymentGateway
    ```

2.  **Configure o Ambiente:**
    Renomeie o arquivo `.env.example` para `.env` e defina suas credenciais (opcional para rodar localmente, pois já vem com defaults).

3.  **Suba os Containers:**
    Execute o comando abaixo para provisionar o SQL Server, Redis e a API:
    ```bash
    docker compose up --build
    ```

4.  **Acesse a Documentação:**
    Aguarde os logs de inicialização e acesse:
    👉 http://localhost:8080/swagger

## 🗺️ Roadmap (Próximos Passos)

- [x] Configuração do Ambiente Docker (SQL + Redis + .NET 9)
- [x] Modelagem do Domínio (Transaction, CreditCard)
- [x] Implementação do Repositório com EF Core
- [x] Serviço de Processamento com Idempotência (Redis)
- [ ] Testes Unitários com xUnit

---
Desenvolvido por Mayrton Eduardo.