# Checklist De Service E Integracao API

- service em `src/services/*Api.js`
- `HTTPClient` usado para requests Axios
- `mapApiError` preservado quando o modulo ja usa esse padrao
- payload alinhado ao backend
- query string e parametros coerentes com a API
- response coerente com o consumo da page ou component
- sem logica de UI dentro do service
- normalizacao movida para `utils` quando fizer sentido no modulo
- `npm run build` executado apos a alteracao
