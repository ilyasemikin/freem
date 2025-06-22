import {useRef, useState} from "react";
import {Button} from 'primereact/button';
import {LoginPasswordCredentialsRequest} from "./clients/models/users/LoginPassword/LoginPasswordCredentialsRequest";
import {NavLink, useNavigate} from "react-router";
import {ILoginEditorHandle, LoginEditor} from "./LoginEditor.tsx";
import {AuthorizationPanel} from "./AuthorizationPanel.tsx";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";
import {useAuthorization} from "./contexts/authorization/useAuthorization.ts";

export function LoginModel() {
  const [loading, setLoading] = useState(false);

  const editorRef = useRef<ILoginEditorHandle>(null);

  const [loginError, setLoginError] = useState<string | undefined>(undefined);

  const clients = useBackendClients();
  const user = useAuthorization();
  const navigator = useNavigate();

  async function login() {
    if (editorRef.current === null) {
      return;
    }

    const valid = editorRef.current.validate();
    if (!valid) {
      return;
    }

    const data = editorRef.current.data();
    const request = new LoginPasswordCredentialsRequest(data.login, data.password);

    setLoading(true);
    const response = await clients.authorization.login(request);

    if (response.ok) {
      await user.login();
      navigator("/");
    } else if (response.status === 403) {
      setLoginError("Username and/or password is incorrect");
    } else {
      throw new Error(response.statusText);
    }

    setLoading(false);
  }

  return (
      <AuthorizationPanel>
        <div>
          <LoginEditor
              ref={editorRef}
              loading={loading}/>
          <Button
              style={{ display: "block", width: "100%", margin: "10px 0 0 0" }}
              size="small"
              label="Sign In"
              loading={loading}
              onClick={login}/>
          {loginError && <p>{loginError}</p>}
        </div>
        <div style={{display: "flex", justifyContent: "space-between"}}>
          <div style={{fontSize: "13px", margin: "15px 0 0 0"}}>
            <NavLink to="/remind">Forgot credentials?</NavLink>
          </div>
          <div style={{fontSize: "13px", margin: "15px 0 0 0"}}>
            <NavLink to="/register">Sign Up</NavLink>
          </div>
        </div>
      </AuthorizationPanel>
  );
}