ProcessControl (cliente)

Este repositório contém a parte cliente (front-end) da aplicação ProcessControl. Abaixo há um resumo da arquitetura, das principais tecnologias utilizadas e instruções rápidas para desenvolvimento.

## Visão geral da arquitetura

- Front-end: Single Page Application (SPA) desenvolvida em Angular. A aplicação é composta por componentes, serviços e módulos que seguem as convenções do Angular.
- Comunicação com backend: via HTTP usando o `HttpClient` do Angular. O serviço `ProcessService` centraliza chamadas relacionadas a processos e histórico de movimentos.
- Notificações: sistema de toasts (notificações) centralizado através de um `ToastService` e um `ToastContainerComponent` que renderiza toasts no canto inferior-direito.
- Paginação do histórico: o histórico de movimentos é carregado em páginas (page/limit) e a interface do `ProcessDetail` implementa carregamento incremental (infinite scroll) ao descer a página.

## Estrutura principal do projeto (resumo)

- `src/` - código fonte Angular
  - `app/` - módulos e componentes principais
    - `components/` - componentes da aplicação (process-list, process-detail, process-modal, shared, ...)
    - `models/` - modelos/DTOs TypeScript
    - `services/` - serviços HTTP e utilitários (ex.: `process.service.ts`, `toast.service.ts`)
  - `main.ts`, `index.html`, `styles.css` - bootstrap da aplicação
- `public/` - ativos públicos
- arquivos de configuração: `angular.json`, `tsconfig.json`, `package.json`, configs de lint/format (ESLint/Prettier)

## Principais tecnologias e frameworks

- Angular (versão 19.x indicada no projeto) — framework front-end para SPA.
- TypeScript — linguagem principal do projeto.
- RxJS — gestão de streams e composição de requisições reativas.
- Bootstrap 5 — estilos e componentes visuais (toasts, botões, layout).
- Angular CLI — geração, build e serve da aplicação.
- Node.js / npm — gestor de pacotes e execução de scripts (build, lint, test).
- ESLint + @angular-eslint + typescript-eslint — lint para TypeScript/Angular.
- Prettier — formatação de código.
- Karma/Jasmine — configuração de testes unitários (existe `karma.conf.js` e `app.component.spec.ts`).

## Padrões e decisões arquiteturais relevantes

- Serviços: responsabilidades de chamadas HTTP e tratamento de erros centralizados (ex.: `ProcessService` usa `catchError` para tratar falhas e integra com o `ToastService`).
- Componentes: UI dividida em componentes pequenos e reutilizáveis (modal compartilhado, toast container, formulários de movimento).
- Notificações: `ToastService` expõe uma API simples (`show`, `showError`) e o container exibe e remove toasts automaticamente.
- Paginação/infinite scroll: o histórico de movimentos é carregado com paginação via query params (`page`, `limit`) — método cliente: `getMovements(processId, page, limit)`. O `ProcessDetail` gerencia o estado da página atual e faz append dos itens carregados.

## Scripts úteis (npm)

Executar no diretório do projeto (Windows - cmd/powershell):

```cmd
npm install
npm start        # inicia o servidor de desenvolvimento (ng serve)
npm run lint     # executa ESLint
npm run lint:fix # tenta corrigir problemas automaticamente
npm run format   # formata com Prettier
npm test         # executa testes (Karma)
```

## Como rodar localmente

1. Instale dependências:

```cmd
npm install
```

2. Inicie a aplicação:

```cmd
npm start
```

3. Abra o navegador em `http://localhost:4200/`.

## Pontos de atenção e próximos passos recomendados

- Lint: o projeto já tem ESLint e Prettier configurados; ainda podem existir avisos (ex.: `any`, diferenças de EOL). Posso limpar esses avisos se desejar.
- Infinite scroll: atualmente implementado via `window:scroll`; é recomendado migrar para `IntersectionObserver` para comportamentos mais previsíveis em listas internas.
- Tipagem: há alguns `any` no código (modais, DTOs parciais) que valem a pena tipar para melhorar robustez e reduzir warnings do linter.

Se quiser, eu posso aplicar automaticamente uma das melhorias acima (converter para `IntersectionObserver`, tipar os pontos com `any`, ou limpar avisos do linter) — diga qual prefere.
