import { Button } from "../Button/Button";
import style from "./_listingHeader.module.css";

export function ListingHeader({
  title,
  description,
  buttonLabel,
  onButtonClick,
}) {
  return (
    <header className={style["listagem-header"]}>
      <div>
        <h1>{title}</h1>
        <p>{description}</p>
      </div>

      <Button
        variant={"success"}
        buttonLabel={buttonLabel}
        onButtonClick={onButtonClick}
      />
    </header>
  );
}
