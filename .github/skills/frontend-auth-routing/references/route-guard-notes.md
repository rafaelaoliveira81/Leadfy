# Notas De Guardas De Rota

## Regra Conhecida Deste Projeto

Os guardas de rota dependem de `AuthContext.isLoading` para evitar redirecionar para `/login` enquanto o token esta sendo restaurado apos refresh.

## Regra Pratica

- se `isLoading` for verdadeiro, o guarda nao deve decidir redirecionamento ainda
- `ProtectedRoute` decide entre `Outlet` e `Navigate` apenas depois da restauracao
- `PublicRoute` tambem deve respeitar esse loading antes de mandar o usuario para `/dashboard`
