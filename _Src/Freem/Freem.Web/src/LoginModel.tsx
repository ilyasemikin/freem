import { ChangeEvent, useState } from "react";
import "./PasswordCredentials.css";
import { Button } from 'primereact/button';
import { UserClient } from "./clients/UserClient";
import { PasswordCredentialsInputText } from "./components/PasswordCredentialsInputText";
import { LoginPasswordCredentialsRequest } from "./clients/models/users/LoginPassword/LoginPasswordCredentialsRequest";

interface AccountData {
  login: string;
  password: string;
}

interface AccountValidationResult {
  loginError?: string;
  passwordError?: string;
}

export function LoginModel() {
  const [data, setData] = useState<AccountData>({
    login: "",
    password: ""
  });

  const [validationResult, setValidationResult] = useState<AccountValidationResult>({});

  const client = new UserClient("http://localhost:30000");

  function handleChange(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
    };

    setData(newData);
    setValidationResult({});
  }

  async function handleSingIn() {
    const newValidationResult = { ...validationResult };
    var valid = true;

    if (data.login === "") {
      newValidationResult.loginError = "Login required";
      valid = false;
    }

    if (data.password === "") {
      newValidationResult.passwordError = "Password required";
      valid = false;
    }

    if (!valid) {
      setValidationResult(newValidationResult);
      return;
    }

    const request = new LoginPasswordCredentialsRequest(data.login, data.password);
    await client.login(request);
  }

  return (
    <>
      <div className="container">
        <PasswordCredentialsInputText
          name="login"
          placeholder="Login"
          value={data.login}
          error={validationResult.loginError}
          onChange={handleChange} />
        <PasswordCredentialsInputText
          name="password"
          placeholder="Password"
          value={data.password}
          error={validationResult.passwordError}
          onChange={handleChange} />
        <Button
          size="small"
          label="Sign In"
          onClick={handleSingIn} />
      </div>
      <div className="footer-splitted">
        <div className="footer-left">
          <a href="">Forgot credentials?</a>
        </div>
        <div className="footer-right">
          <a href="">Sign Up</a>
        </div>
      </div>
    </>
  );
}