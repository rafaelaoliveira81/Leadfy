export function formatCreatedAt(dateStr) {
  if (!dateStr) return '—';
  return new Intl.DateTimeFormat('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  }).format(new Date(dateStr));
}

export function truncatePrompt(text, maxLength = 80) {
  if (!text) return '—';
  if (text.length <= maxLength) return text;
  return text.slice(0, maxLength) + '...';
}
