import {FormEvent, useState} from "react";
import {InputText} from "primereact/inputtext";
import {Password} from "primereact/password";
import {Button} from "primereact/button";
import {Message} from "primereact/message";
import {useAuth} from "../../authorization/context/useAuth.ts";

type Mode = "login" | "register";

export function LoginPage() {
  const {status, login, register, completeRegistration, logout} = useAuth();

  const [mode, setMode] = useState<Mode>("login");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [nickname, setNickname] = useState("");
  const [error, setError] = useState<string>();
  const [busy, setBusy] = useState(false);

  const unregistered = status === "unregistered";

  async function submit(e: FormEvent) {
    e.preventDefault();

    setBusy(true);
    setError(undefined);

    let result: string | undefined;
    if (unregistered) {
      result = await completeRegistration(nickname.trim());
    } else if (mode === "register") {
      result = await register(username.trim(), password, nickname.trim());
    } else {
      result = await login(username.trim(), password);
    }

    setError(result);
    setBusy(false);
  }

  function renderCredentials() {
    return (
        <>
          <InputText placeholder="Username" autoComplete="username" value={username}
                     onChange={e => setUsername(e.target.value)}/>
          <Password placeholder="Password" feedback={false} toggleMask
                    autoComplete={mode === "register" ? "new-password" : "current-password"}
                    value={password} onChange={e => setPassword(e.target.value)}
                    inputStyle={{width: "100%"}}/>
        </>
    );
  }

  function renderNickname() {
    return (
        <InputText placeholder="Nickname" value={nickname} onChange={e => setNickname(e.target.value)}/>
    );
  }

  function title() {
    if (unregistered) {
      return "Finish registration";
    }

    return mode === "register" ? "Register" : "Log in";
  }

  return (
      <div style={{display: "flex", justifyContent: "center", alignItems: "center", height: "100%"}}>
        <form onSubmit={submit} className="login-card">
          <h1 style={{margin: 0, fontSize: "1.6rem", color: "var(--accent)"}}>Freem</h1>
          <h2 className="muted" style={{margin: 0, fontSize: "1rem", fontWeight: 500}}>{title()}</h2>

          {!unregistered && renderCredentials()}
          {(unregistered || mode === "register") && renderNickname()}

          {error !== undefined && <Message severity="error" text={error}/>}

          <Button type="submit" label={title()} loading={busy}/>

          {unregistered
              ? <Button type="button" text label="Log out" onClick={logout}/>
              : <Button type="button" text
                        label={mode === "login" ? "Create an account" : "I already have an account"}
                        onClick={() => setMode(mode === "login" ? "register" : "login")}/>}
        </form>
      </div>
  );
}
