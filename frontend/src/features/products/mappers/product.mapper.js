/**
 * Converte o preço de string do formulário para number do payload.
 * @param {string} priceStr
 * @returns {number}
 */
export function priceToNumber(priceStr) {
  return Number(String(priceStr).replace(',', '.'));
}

/**
 * Formata o preço numérico para exibição em BRL.
 * @param {number} price
 * @returns {string}
 */
export function formatPrice(price) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(price);
}

/**
 * Converte preço numérico da API para string do formulário.
 * @param {number} price
 * @returns {string}
 */
export function priceToFormValue(price) {
  return String(price).replace('.', ',');
}
