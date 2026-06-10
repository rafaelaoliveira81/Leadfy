# Checklist De AuthContext E Rotas

- `AuthContext` continua sendo a fonte principal de token e claims
- `isLoading` preservado durante a restauracao do token
- `ProtectedRoute` nao redireciona cedo demais para `/login`
- `PublicRoute` continua bloqueando acesso a telas publicas quando ja autenticado
- token invalido ou expirado e tratado com limpeza segura do storage
- pages recebem apenas o contexto autenticado necessario
