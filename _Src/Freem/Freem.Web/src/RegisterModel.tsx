import {useRef, useState} from "react";
import {Button} from "primereact/button";
import {
  RegisterPasswordCredentialsRequest
} from "./clients/models/users/LoginPassword/RegisterPasswordCredentialsRequest";
import {useNavigate} from "react-router";
import {IRegisterEditorHandle, RegisterEditor, RegisterEditorError} from "./RegisterEditor.tsx";
import { NavLink } from "react-router";
import {AuthorizationPanel} from "./AuthorizationPanel.tsx";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";

export function RegisterModel() {
  const [loading, setLoading] = useState(false);

  const navigator = useNavigate();
  const clients = useBackendClients();

  const editorRef = useRef<IRegisterEditorHandle>(null);

  async function signUp() {
    if (editorRef.current === null) {
      throw new Error();
    }

    const valid = editorRef.current.validate();
    if (!valid) {
      return;
    }

    const data = editorRef.current.data();

    const request = new RegisterPasswordCredentialsRequest(
        data.nickname,
        data.login,
        data.password)

    setLoading(true);

    const response = await clients.authorization.register(request);
    if (response.ok) {
      navigator("/login");
    } else if (response.status === 422) {
      editorRef.current.setError(RegisterEditorError.LoginAlreadyExist);
    }

    setLoading(false);
  }

  return (
      <AuthorizationPanel>
        <div>
          <RegisterEditor
              ref={editorRef}
              loading={loading}/>
          <Button
              style={{ display: "block", width: "100%", margin: "10px 0 0 0" }}
              size="small"
              label="Sign Up"
              loading={loading}
              onClick={signUp}/>
        </div>
        <div style={{margin: "15px", textAlign: "center"}}>
          <span style={{fontSize: "13px", margin: "0 5px"}}>Have an account?</span>
          <NavLink style={{fontSize: "13px", margin: "0 5px"}} to="/login">Sign In</NavLink>
        </div>
      </AuthorizationPanel>
  )
}