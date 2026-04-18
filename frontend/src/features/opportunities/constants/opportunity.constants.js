export const OPPORTUNITY_PAGE_SIZES = [10, 25, 50, 100];

export const OPPORTUNITY_DEFAULT_PAGE_SIZE = 10;

export const OPPORTUNITY_STATUS_OPTIONS = [
  { value: 'all', label: 'Todos' },
  { value: 'active', label: 'Ativos' },
  { value: 'inactive', label: 'Inativos' },
];

export const OPPORTUNITY_STAGE_OPTIONS = [
  { value: 0, label: 'Novo' },
  { value: 1, label: 'Qualificado' },
  { value: 2, label: 'Proposta' },
  { value: 3, label: 'Negociação' },
  { value: 4, label: 'Fechado' },
];

export const OPPORTUNITY_STAGE_MAP = {
  New: 'Novo',
  Qualified: 'Qualificado',
  Proposal: 'Proposta',
  Negotiation: 'Negociação',
  Closed: 'Fechado',
};

export const OPPORTUNITY_SEARCH_DEBOUNCE_MS = 400;
