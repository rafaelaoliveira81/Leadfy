# Notas De Payload Do Kanban

## Regras Conhecidas Deste Projeto

- o Kanban salva card de oportunidade via update com payload completo
- editar apenas parte dos campos na UI exige preservar valores atuais como `LeadId`, `ProductId`, `Stage`, `Amount` e `ExpectedCloseDate`
- o endpoint de otimizar interacao usa payload `{ content }`
- a resposta de optimize interaction deve atualizar `interactionDescription` com o campo correto retornado pela API

## Regra Pratica

Antes de alterar update ou optimize interaction no Kanban:

1. confirme o contrato atual do endpoint
2. preserve campos nao editados, mas obrigatorios
3. mantenha a leitura da resposta alinhada ao payload real retornado pela API
