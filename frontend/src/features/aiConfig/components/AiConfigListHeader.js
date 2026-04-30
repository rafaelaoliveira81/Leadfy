import style from './_aiConfigListHeader.module.css';

export function AiConfigListHeader() {
  return (
    <div className={style.header}>
      <h1>Configuração de IA</h1>
      <p>Gerencie as configurações de modelos de inteligência artificial e templates de prompt</p>
    </div>
  );
}
