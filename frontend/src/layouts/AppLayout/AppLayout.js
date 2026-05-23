import { useState } from "react";
import style from "./_appLayout.module.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { MdMenu } from "react-icons/md";

export function AppLayout({ children }) {
  const [isCollapsed, setIsCollapsed] = useState(false);
  const [isMobileOpen, setIsMobileOpen] = useState(false);

  return (
    <div className={style.layout}>
      <Sidebar
        isCollapsed={isCollapsed}
        onToggle={() => setIsCollapsed((prev) => !prev)}
        isMobileOpen={isMobileOpen}
        onCloseMobile={() => setIsMobileOpen(false)}
      />
      <div className={`${style.main} ${isCollapsed ? style.collapsed : ""}`}>
        <button
          className={style.mobileMenuBtn}
          onClick={() => setIsMobileOpen(true)}
          aria-label="Abrir menu"
        >
          <MdMenu />
        </button>
        <div className={style.pageContent}>{children}</div>
      </div>
    </div>
  );
}
