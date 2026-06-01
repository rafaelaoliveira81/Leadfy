import { Button } from "../Button/Button";
import { Select } from "../Select/Select";
import style from "./_listingHeader.module.css";

export function ListingHeader({
  title,
  description,
  buttonLabel,
  onButtonClick,
  selectLabel,
  selectOptions,
  selectValue,
  onSelectChange,
}) {
  return (
    <header className={style["listagem-header"]}>
      <div>
        <h1>{title}</h1>
        <p>{description}</p>
      </div>

      <div className={style["header-acoes"]}>
        {selectOptions && (
          <Select
            label={selectLabel}
            options={selectOptions}
            value={selectValue}
            onChange={onSelectChange}
          />
        )}

        {buttonLabel && (
          <Button
            variant="success"
            buttonLabel={buttonLabel}
            onButtonClick={onButtonClick}
          />
        )}
      </div>
    </header>
  );
}
