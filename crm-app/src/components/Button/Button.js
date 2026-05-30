import style from "./_button.module.css";

export function Button({
  variant = "primary",
  type = "button",
  buttonLabel,
  onButtonClick,
  disabled = false,
}) {
  return (
    <button
      type={type}
      className={`${style.botao} ${style[variant]}`}
      onClick={onButtonClick}
      disabled={disabled}
    >
      {buttonLabel}
    </button>
  );
}
