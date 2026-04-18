import { OPPORTUNITY_STAGE_MAP } from '../constants/opportunity.constants';

/**
 * Traduz o nome do stage da API para exibição.
 * @param {string} stageName
 * @returns {string}
 */
export function formatStageName(stageName) {
  if (!stageName) return '—';
  return OPPORTUNITY_STAGE_MAP[stageName] || stageName;
}

/**
 * Converte o valor de string do formulário para number do payload.
 * @param {string} amountStr
 * @returns {number}
 */
export function amountToNumber(amountStr) {
  return Number(String(amountStr).replace(',', '.'));
}

/**
 * Formata valor numérico para exibição em BRL.
 * @param {number} amount
 * @returns {string}
 */
export function formatAmount(amount) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(amount);
}

/**
 * Converte valor numérico da API para string do formulário.
 * @param {number} amount
 * @returns {string}
 */
export function amountToFormValue(amount) {
  return String(amount).replace('.', ',');
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

/**
 * Converte data ISO para formato de input date (YYYY-MM-DD).
 * @param {string} dateStr
 * @returns {string}
 */
export function dateToInputValue(dateStr) {
  if (!dateStr) return '';
  return new Date(dateStr).toISOString().split('T')[0];
}
