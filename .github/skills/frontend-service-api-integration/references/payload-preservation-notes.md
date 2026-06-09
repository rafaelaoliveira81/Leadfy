# Notas De Preservacao De Payload

Use estas notas quando a UI editar apenas parte de uma entidade, mas o backend exigir payload completo.

## Regras Conhecidas Deste Projeto

- no Kanban, update de oportunidade precisa preservar campos como `LeadId`, `ProductId`, `Stage`, `Amount` e `ExpectedCloseDate` conforme o fluxo atual
- editar apenas um subconjunto de campos na UI nao significa que o backend aceite payload parcial
- quando houver utilitario de normalizacao no modulo, preserve esse padrao em vez de montar o payload diretamente em varios lugares

## Regra Pratica

Antes de editar um service de update:

1. confirme o contrato do backend
2. veja como a UI atual obtem os dados completos da entidade
3. preserve campos nao editados que ainda sao obrigatorios para o endpoint
