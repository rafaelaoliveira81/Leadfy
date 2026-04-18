import { MdSearch } from 'react-icons/md';
import { Button } from '../../../components/ui/Button/Button';
import { USER_STATUS_OPTIONS } from '../constants/user.constants';
import style from './_userFilters.module.css';

/**
 * Barra de filtros da listagem de usuários.
 */
export function UserFilters({
  search,
  onSearchChange,
  statusFilter,
  onStatusFilterChange,
  onClearFilters,
  onNewUser,
}) {
  return (
    <div className={style.filters}>
      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Filtrar por nome do usuário..."
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          aria-label="Filtrar por nome do usuário"
        />
      </div>

      <select
        className={style.statusSelect}
        value={statusFilter}
        onChange={(e) => onStatusFilterChange(e.target.value)}
        aria-label="Filtrar por status"
      >
        {USER_STATUS_OPTIONS.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>

      <Button variant="ghost" size="sm" onClick={onClearFilters}>
        Limpar filtros
      </Button>

      <div className={style.spacer} />

      <Button onClick={onNewUser}>+ Novo</Button>
    </div>
  );
}
