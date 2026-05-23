import { MdSearch } from "react-icons/md";
import { Button } from "../../../components/ui/Button/Button";
import { OWNER_STATUS_OPTIONS } from "../constants/owner.constants";
import style from "./_ownerFilters.module.css";

/**
 * Barra de filtros da listagem de owners.
 */
export function OwnerFilters({
  search,
  onSearchChange,
  statusFilter,
  onStatusFilterChange,
  onClearFilters,
  onNewOwner,
}) {
  return (
    <div className={style.filters}>
      <div className={style.searchWrapper}>
        <MdSearch className={style.searchIcon} />
        <input
          type="text"
          className={style.searchInput}
          placeholder="Filtrar por nome do responsável..."
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          aria-label="Filtrar por nome do responsável"
        />
      </div>

      <select
        className={style.statusSelect}
        value={statusFilter}
        onChange={(e) => onStatusFilterChange(e.target.value)}
        aria-label="Filtrar por status"
      >
        {OWNER_STATUS_OPTIONS.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>

      <Button variant="ghost" size="sm" onClick={onClearFilters}>
        Limpar filtros
      </Button>

      <div className={style.spacer} />

      <Button onClick={onNewOwner}>+ Novo</Button>
    </div>
  );
}
