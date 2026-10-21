import {MouseEvent, useEffect, useRef, useState} from "react";
import {Button} from "primereact/button";
import {OverlayPanel} from "primereact/overlaypanel";
import {useNavigate} from "react-router";
import {useAuth} from "../authorization/context/useAuth.ts";
import {useBackendClients} from "../clients/context/useBackendClients.ts";
import {MeResponse} from "../clients/models/users/MeResponse.ts";

export function AppHeader() {
  const {logout} = useAuth();
  const {user} = useBackendClients();
  const navigate = useNavigate();

  const overlayRef = useRef<OverlayPanel>(null);
  const [nickname, setNickname] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      const response = await user.me();
      if (!response.ok) {
        return;
      }

      const body: MeResponse = await response.json();
      if (current) {
        setNickname(body.nickname);
      }
    }

    load().catch(() => undefined);

    return () => {
      current = false;
    };
  }, [user]);

  function toggle(e: MouseEvent) {
    overlayRef.current?.toggle(e);
  }

  function openSettings() {
    overlayRef.current?.hide();
    navigate("/settings");
  }

  async function signOut() {
    overlayRef.current?.hide();
    await logout();
  }

  return (
      <header className="app-header">
        <span className="app-header-title">Freem</span>
        <Button text rounded severity="secondary" icon="pi pi-user" aria-label="User menu" onClick={toggle}/>
        <OverlayPanel ref={overlayRef}>
          <div className="user-menu">
            <div className="user-menu-name">
              <span className="muted" style={{fontSize: "13px"}}>Signed in as</span>
              <span className="item-title">{nickname ?? "…"}</span>
            </div>
            <hr className="divider"/>
            <Button text size="small" severity="secondary" icon="pi pi-cog" label="Settings" onClick={openSettings}
                    style={{width: "100%", justifyContent: "flex-start"}}/>
            <Button text size="small" severity="secondary" icon="pi pi-sign-out" label="Log out" onClick={signOut}
                    style={{width: "100%", justifyContent: "flex-start"}}/>
          </div>
        </OverlayPanel>
      </header>
  );
}
