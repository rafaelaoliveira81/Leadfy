import { MdSearch } from 'react-icons/md';
import { Button } from '../../../components/ui/Button/Button';
import { OPPORTUNITY_STATUS_OPTIONS } from '../constants/opportunity.constants';
import style from './_opportunityFilters.module.css';

/**
 * Barra de filtros da listagem de oportunidades.
 */
export function OpportunityFilters({
  search,
  onSearchChange,
  statusFilter,
  onStatusFilterChange,
  onClearFilters,
  onNewOpportunity,
}) {
  return (
    <div className={style.filters}>
      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Filtrar por título da oportunidade..."
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          aria-label="Filtrar por título da oportunidade"
        />
      </div>

      <select
        className={style.statusSelect}
        value={statusFilter}
        onChange={(e) => onStatusFilterChange(e.target.value)}
        aria-label="Filtrar por status"
      >
        {OPPORTUNITY_STATUS_OPTIONS.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>

      <Button variant="ghost" size="sm" onClick={onClearFilters}>
        Limpar filtros
      </Button>

      <div className={style.spacer} />

      <Button onClick={onNewOpportunity}>+ Nova</Button>
    </div>
  );
}
