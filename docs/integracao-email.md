Para a integração com o e-mail estou utilizando o Gmail API (Google).

Para isso, foi utilizada as seguintes dependências:

- Biblioteca da api do gmail
dotnet add package Google.Apis.Gmail.v1 

- Biblioteca para construção facilitada do email
dotnet add package MimeKit


---
Scopos utilizados:
GmailService.Scope.GmailSend

Até o momento a integração será somente para o envio de e-mail, por isso somente foi adicionado o scopo de Send.

Para as credenciais gere a Credencial OAuth client ID para Aplicação Web.

Configure as variáveis:
    "gmail":{
        "client_id":"__CLIENT_ID__",
        "client_secret":"__CLIENT_SECRET__"
    }