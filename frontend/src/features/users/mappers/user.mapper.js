/**
 * Formata o nome do perfil para exibição.
 * @param {object} role - Objeto role { id, name, displayName }
 * @returns {string}
 */
export function formatRoleName(role) {
  if (!role) return '—';
  return role.displayName || role.name || '—';
}

/**
 * Formata data ISO para exibição em pt-BR.
 * @param {string} dateStr
 * @returns {string}
 */
export function formatDate(dateStr) {
  if (!dateStr) return '—';
  return new Intl.DateTimeFormat('pt-BR').format(new Date(dateStr));
}
