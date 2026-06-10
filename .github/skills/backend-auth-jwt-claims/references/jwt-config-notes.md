# Notas De Configuracao JWT

Estas notas resumem o padrao atual de JWT no backend do Leadfy.

## Onde Fica A Configuracao

- o registro de autenticacao fica em `backend/Api/Program.cs`
- a configuracao usa `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)`
- a validacao do token e feita em `AddJwtBearer` com `TokenValidationParameters`
- `JwtSettings` e carregado a partir da configuracao da aplicacao

## O Que O Token Carrega Hoje

O `TokenService` emite atualmente estas claims:

- `usuarioId`
- `nome`
- `email`

Se uma acao consome `usuarioId`, ela deve continuar alinhada com esse nome ou a mudanca precisa ajustar emissao e consumo em conjunto.

## Regra Pratica Para Mudancas

- nao altere nome de claim isoladamente
- nao altere issuer, audience ou secret sem necessidade objetiva
- nao replique leitura de claim em varios controllers
- se a mudanca for apenas obter o usuario autenticado, prefira reaproveitar o helper existente

## Integração Com Swagger

O projeto ja documenta o esquema Bearer no Swagger.
Se a tarefa tocar autenticacao e documentacao ao mesmo tempo, preserve a definicao e o requirement de seguranca existentes em `Program.cs`, alterando apenas o necessario.
