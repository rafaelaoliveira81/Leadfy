---
name: backend-feature-flow
description: "Cria ou altera uma feature completa no backend ASP.NET Core do Leadfy. Use quando precisar implementar ou ajustar endpoint, DTO, interface de App, Application, Service, Repository, DI em Program.cs e validacao seguindo o fluxo Api -> Application -> Service -> Repository com propagacao correta de tenantId."
argument-hint: "Descreva a feature backend, endpoint ou fluxo a implementar"
user-invocable: true
---

# Backend Feature Flow

Use esta skill quando a tarefa exigir alteracoes coordenadas em mais de uma camada do backend.
Ela serve para criar uma feature nova ou expandir uma feature existente sem quebrar o desenho arquitetural do projeto.
No contexto atual do projeto, assuma que entidades principais sao tenant-scoped e carregam `tenantId`; apenas entidades explicitamente globais nao devem receber esse contexto.
No projeto atual, identificadores de entidades e claims relevantes sao `Guid`; em bordas HTTP e DTOs eles podem aparecer como `string`, mas devem continuar representando um `Guid` valido.

## Papel Desta Skill No Conjunto

Esta e a skill guarda-chuva do backend.
Use-a quando a entrega atravessar varias camadas e nao houver um unico ponto dominante, ou quando a tarefa claramente exigir coordenacao entre controller, Application, Repository e DI.
Se o centro da mudanca estiver nitidamente em contrato HTTP, Application, Repository ou autenticacao, prefira a skill especializada correspondente.

## Quando Usar

- Criar um endpoint novo com DTO, App, repositorio e DI
- Expandir uma feature backend ja existente em varias camadas
- Implementar fluxo completo tenant-scoped sem perder o contexto do tenant autenticado
- Ajustar contrato HTTP e logica de aplicacao na mesma entrega
- Implementar um novo caso de uso que depende de persistencia e mapeamento
- Organizar uma mudanca que hoje esta espalhada entre controller, Application e Repository

## Nao Use Quando

- A tarefa for apenas documentacao Swagger de um endpoint existente
- A mudanca for apenas em JWT, claims ou autorizacao
- A mudanca for apenas em acesso a dados sem impacto nas demais camadas
- A mudanca estiver concentrada em uma unica camada com ponto de controle dominante

Nesses casos, prefira uma skill mais especializada.

## Antes De Editar

1. Localize a feature equivalente mais proxima no backend antes de propor estrutura nova.
2. Identifique onde o comportamento realmente e decidido: controller, App, Service ou Repository.
3. Confirme quais arquivos precisam mudar juntos para entregar o fluxo completo.
4. Classifique a entidade principal da feature como tenant-scoped ou global.
5. Reuse DTOs, interfaces e helpers existentes antes de criar novos artefatos.

Consulte:

- [Escopo da feature](./references/feature-scope.md)
- [Checklist por camada](./references/layer-checklist.md)
- [Template de implementacao](./assets/feature-template.md)

## Procedimento

1. Identifique o ponto de entrada da feature.

Comece pelo controller existente, por um App relacionado ou por um fluxo semelhante na mesma entidade.
Se o arquivo inicial apenas encaminha a chamada, de um salto para a camada que decide validacao, mapeamento ou persistencia.

2. Defina o contrato da feature.

Confirme rota, verbo HTTP, DTOs de entrada e saida, necessidade de autenticacao, origem do `tenantId` e codigos de resposta esperados.
Quando a feature expuser identificador de entidade por rota, siga o padrao do projeto com constraint `:guid` e trate qualquer `string id` do contrato como representacao textual de um `Guid`.

3. Ajuste a camada Application primeiro quando a mudanca for de fluxo.

Coloque em `Application` a orquestracao, validacao de entrada, mapeamento entre DTO e entidade e coordenacao entre repositorios.
Quando a entidade for tenant-scoped, resolva o `tenantId` a partir do contexto autenticado ou de `ITenantProvider` e nao aceite esse valor como entrada livre do cliente sem necessidade explicita.
Quando identificadores entrarem como `string`, valide com `Guid.TryParse` cedo, lance `ArgumentException` quando o valor for invalido e converta para `Guid` antes de chamar o repository.
Se houver regra de dominio mais clara ou reutilizavel, empurre para `Service`.

4. Atualize a persistencia sem levar regra de negocio para o repositorio.

No `Repository`, implemente apenas leitura, escrita e mapeamento de dados.
Mantenha o estilo da feature ja existente: EF Core quando a feature ja segue queries via contexto; Dapper/stored procedure quando esse for o padrao do modulo.
Em entidades tenant-scoped, preserve filtros, atribuicoes e parametros de `tenantId`; so ignore esse dado em entidades globalmente compartilhadas.
Nao carregue `string id` ate o repository quando o identificador ja deveria estar tipado; nessa camada, prefira `Guid` e `Guid?` conforme o modelo da entidade.

5. Deixe o controller fino.

O controller deve receber request, chamar a camada Application, traduzir excecoes para HTTP e manter a documentacao Swagger coerente com o comportamento real.

6. Atualize contratos e DI.

Quando criar ou alterar interfaces, mantenha nomes, assinaturas e pasta alinhados ao modulo mais proximo.
Se houver novo App, Service ou Repository, registre a dependencia em `backend/Api/Program.cs` sem alterar configuracoes nao relacionadas.
Se a feature depender de contexto de tenant, preserve o uso de `ITenantProvider` e dos helpers ja existentes antes de inventar outra forma de resolver o tenant.

7. Valide a mudanca.

Prefira um teste executavel estreito quando existir.
Caso contrario, valide com `dotnet build backend/crm.sln` ou um build mais estreito no projeto afetado.

## Regras Operacionais Deste Projeto

- Respeite o fluxo `Api -> Application -> Service -> Repository`
- Nao coloque regra de negocio no controller
- Nao crie atalhos entre camadas so para reduzir codigo
- Trate entidades principais como tenant-scoped por padrao; apenas entidades explicitamente globais nao usam `tenantId`
- Em fluxos tenant-scoped, o `tenantId` deve vir do contexto autenticado ou de `ITenantProvider`
- Trate identificadores de entidade como `Guid` canonico do dominio; `string` e apenas formato de transporte em request, response ou claim
- Em endpoints por identificador, prefira rotas com `:guid` e validacao antecipada de `Guid` na entrada
- Preserve mensagens em portugues quando a feature ja usa portugues
- Reaproveite DTOs e interfaces existentes antes de criar novos contratos
- Preserve o estilo de excecoes do modulo, principalmente `ArgumentException` e `KeyNotFoundException`
- Em acesso com Dapper, use nova conexao por chamada
- Ao alterar endpoint, mantenha XML comments e `ProducesResponseType` sincronizados com o codigo

## Resultado Esperado

Ao final, a feature deve ficar consistente nestes pontos:

- contrato HTTP claro
- App com orquestracao e validacao previsiveis
- contexto de tenant consistente entre autenticacao, Application e Repository quando a feature nao for global
- repositorio com responsabilidade apenas de persistencia
- dependencias registradas corretamente
- validacao executada apos a edicao

## Validacao

1. Se houver teste estreito para a feature, execute primeiro.
2. Se nao houver, rode `dotnet build backend/crm.sln`.
3. Confira se o `tenantId` percorre corretamente controller, Application e Repository quando a entidade for tenant-scoped.
4. Confira se todo `id` textual recebido pela feature e validado como `Guid` antes de chegar ao repository.
5. Se a mudanca for isolada, um build do projeto afetado e aceitavel.

## Referencias

- [Escopo da feature](./references/feature-scope.md)
- [Checklist por camada](./references/layer-checklist.md)
- [Template de implementacao](./assets/feature-template.md)
