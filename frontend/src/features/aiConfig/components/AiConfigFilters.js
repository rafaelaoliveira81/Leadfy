import { MdSearch } from 'react-icons/md';
import { Button } from '../../../components/ui/Button/Button';
import { AI_CONFIG_STATUS_OPTIONS } from '../constants/aiConfig.constants';
import style from './_aiConfigFilters.module.css';

export function AiConfigFilters({
  search,
  onSearchChange,
  statusFilter,
  onStatusFilterChange,
  onClearFilters,
  onNewConfig,
}) {
  return (
    <div className={style.filters}>
      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Filtrar por modelo..."
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          aria-label="Filtrar por nome do modelo"
        />
      </div>

      <select
        className={style.statusSelect}
        value={statusFilter}
        onChange={(e) => onStatusFilterChange(e.target.value)}
        aria-label="Filtrar por status"
      >
        {AI_CONFIG_STATUS_OPTIONS.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>

      <Button variant="ghost" size="sm" onClick={onClearFilters}>
        Limpar filtros
      </Button>

      <div className={style.spacer} />

      <Button onClick={onNewConfig}>+ Nova configuração</Button>
    </div>
  );
}
