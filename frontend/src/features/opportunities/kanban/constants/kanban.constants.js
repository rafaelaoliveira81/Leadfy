/**
 * Constantes exclusivas do Kanban de Oportunidades.
 * Mapeiam diretamente os stages do enum backend (OpportunityStage).
 * Isoladas das constantes da tela CRUD para evitar acoplamento.
 */

export const KANBAN_STAGES = [
  { value: 1, key: 'NewLead',      label: 'Novo Lead',    accent: '#a2d2ff' },
  { value: 2, key: 'Contacted',    label: 'Contactado',   accent: '#48cae4' },
  { value: 3, key: 'Qualified',    label: 'Qualificado',  accent: '#fb8500' },
  { value: 4, key: 'ProposalSent', label: 'Proposta',     accent: '#8338ec' },
  { value: 5, key: 'Negotiation',  label: 'Negociação',   accent: '#ffb703' },
  { value: 6, key: 'Won',          label: 'Ganho',        accent: '#2a9d8f' },
  { value: 7, key: 'Lost',         label: 'Perdido',      accent: '#ef233c' },
];

export const KANBAN_STAGE_MAP = Object.fromEntries(
  KANBAN_STAGES.map((s) => [s.key, s])
);

export const KANBAN_STAGE_BY_VALUE = Object.fromEntries(
  KANBAN_STAGES.map((s) => [s.value, s])
);

/**
 * Formata o valor monetário em BRL.
 */
export function formatCurrency(value) {
  if (value == null) return 'R$ 0,00';
  return Number(value).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  });
}

/**
 * Formata data para exibição pt-BR curta.
 */
export function formatDateShort(dateStr) {
  if (!dateStr) return '—';
  const d = new Date(dateStr);
  return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric' });
}

/**
 * Retorna as iniciais do nome para exibição em avatar.
 */
export function getInitials(name) {
  if (!name) return '?';
  const parts = name.trim().split(/\s+/);
  if (parts.length === 1) return parts[0][0].toUpperCase();
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}
