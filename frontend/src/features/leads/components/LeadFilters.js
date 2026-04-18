import { MdSearch } from 'react-icons/md';
import { Button } from '../../../components/ui/Button/Button';
import { LEAD_STATUS_OPTIONS } from '../constants/lead.constants';
import style from './_leadFilters.module.css';

/**
 * Barra de filtros da listagem de leads.
 * @param {object} props
 * @param {string} props.search
 * @param {(value: string) => void} props.onSearchChange
 * @param {string} props.statusFilter
 * @param {(value: string) => void} props.onStatusFilterChange
 * @param {() => void} props.onClearFilters
 * @param {() => void} props.onNewLead
 */
export function LeadFilters({
  search,
  onSearchChange,
  statusFilter,
  onStatusFilterChange,
  onClearFilters,
  onNewLead,
}) {
  return (
    <div className={style.filters}>
      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Filtrar por nome do lead..."
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          aria-label="Filtrar por nome do lead"
        />
      </div>

      <select
        className={style.statusSelect}
        value={statusFilter}
        onChange={(e) => onStatusFilterChange(e.target.value)}
        aria-label="Filtrar por status"
      >
        {LEAD_STATUS_OPTIONS.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>

      <Button variant="ghost" size="sm" onClick={onClearFilters}>
        Limpar filtros
      </Button>

      <div className={style.spacer} />

      <Button onClick={onNewLead}>+ Novo</Button>
    </div>
  );
}
