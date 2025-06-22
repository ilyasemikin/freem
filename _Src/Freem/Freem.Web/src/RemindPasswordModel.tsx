import {ChangeEvent, useState} from "react";
import {InputText} from "./components/inputs/InputText.tsx";
import {Button} from "primereact/button";
import {NavLink} from "react-router";
import {AuthorizationPanel} from "./AuthorizationPanel.tsx";

export function RemindPasswordModel() {
  const [loading] = useState(false);
  const [login, setLogin] = useState("");
  const [error, setError] = useState<string>();

  function change(e: ChangeEvent<HTMLInputElement>) {
    setLogin(e.target.value);
    setError(undefined);
  }

  function handleSend() {
    if (login === "") {
      setError("Login required");
      return;
    }
  }

  return (
      <AuthorizationPanel>
        <div>
          <InputText
              placeholder="Login"
              value={login}
              error={error}
              onChange={change}/>
          <Button
              style={{display: "block", width: "100%", margin: "10px 0 0 0"}}
              size="small"
              label="Send"
              loading={loading}
              onClick={handleSend}/>
        </div>
        <div style={{display: "flex", justifyContent: "space-between"}}>
          <div style={{fontSize: "13px", margin: "15px 0 0 0"}}>
            <NavLink to="/login">Sign In</NavLink>
          </div>
          <div style={{fontSize: "13px", margin: "15px 0 0 0"}}>
            <NavLink to="/register">Sign Up</NavLink>
          </div>
        </div>
      </AuthorizationPanel>
  );
}