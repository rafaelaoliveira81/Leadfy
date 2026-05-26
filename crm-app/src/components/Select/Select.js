import style from "./_select.module.css";

export function Select({ label, options, value, onChange }) {
  return (
    <div className={style.selectContainer}>
      <label className={style["filtro-status"]}>
        <span>{label}</span>
        <select value={value} onChange={onChange}>
          {options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      </label>
    </div>
  );
}
