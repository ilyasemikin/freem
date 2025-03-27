import "./PasswordCredentials.css";
import { ChangeEvent } from "react";
import { Button } from "primereact/button";
import { useState } from "react";
import { UserClient } from "./clients/UserClient";
import { RegisterPasswordCredentialsRequest } from "./clients/models/users/LoginPassword/RegisterPasswordCredentialsRequest";
import { PasswordCredentialsInputText } from "./components/PasswordCredentialsInputText";

interface AccountData {
  nickname: string;
  login: string;
  password: string;
  confirmPassword: string;
}

interface AccountValidationResult {
  nicknameError?: string;
  loginError?: string;
  passwordError?: string;
  confirmPasswordError?: string;
}

export function RegisterModel() {
  const [data, setData] = useState<AccountData>({
    nickname: "",
    login: "",
    password: "",
    confirmPassword: ""
  });

  const [validationResult, setValidationResult] = useState<AccountValidationResult>({});

  const client = new UserClient("http://localhost:30000");

  function validate(data: AccountData): AccountValidationResult {
    const result: AccountValidationResult = {};

    if (data.confirmPassword !== "" && data.password !== data.confirmPassword) {
      result.confirmPasswordError = "Passwords not match";
    }

    return result;
  }

  function handleChange(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
    };

    const validationResult = validate(newData);

    setData(newData);
    setValidationResult(validationResult);
  }

  async function handleSignUp() {
    const newValidationResult = { ...validationResult };
    var valid = true;

    if (data.login === "") {
      newValidationResult.loginError = "Login required";
      valid = false;
    }

    if (data.nickname === "") {
      newValidationResult.nicknameError = "Nickname required";
      valid = false;
    }

    if (data.password === "") {
      newValidationResult.passwordError = "Password required";
      valid = false;
    }

    if (data.confirmPassword === "") {
      newValidationResult.confirmPasswordError = "Confirm password required";
      valid = false;
    }

    if (!valid) {
      setValidationResult(newValidationResult);
      return;
    }

    const request = new RegisterPasswordCredentialsRequest(
      data.nickname,
      data.login,
      data.password);

    await client.register(request);
  }

  return (
    <>
      <div className="container">
        <PasswordCredentialsInputText
          name="nickname"
          placeholder="Nickname"
          value={data.nickname}
          error={validationResult.nicknameError}
          onChange={handleChange} />
        <PasswordCredentialsInputText
          name="login"
          placeholder="Login"
          value={data.login}
          error={validationResult.loginError}
          onChange={handleChange} />
        <PasswordCredentialsInputText
          type="password"
          name="password"
          placeholder="Password"
          value={data.password}
          error={validationResult.passwordError}
          onChange={handleChange} />
        <PasswordCredentialsInputText
          type="password"
          name="confirmPassword"
          placeholder="Confirm password"
          value={data.confirmPassword}
          error={validationResult.confirmPasswordError}
          onChange={handleChange} />
        <Button
          size="small"
          label="Sign Up"
          onClick={handleSignUp} />
      </div>
      <div className="footer-oneline">
        <p>Have an account?</p>
        <a href="">Sign In</a>
      </div>
    </>
  )
}