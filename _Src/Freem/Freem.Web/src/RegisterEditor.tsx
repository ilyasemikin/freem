import {ChangeEvent, forwardRef, useImperativeHandle, useState} from "react";
import {InputText} from "./components/inputs/InputText.tsx";

export interface IRegisterEditorHandle {
  validate: () => boolean;
  data: () => IRegisterEditorData;
  setError: (error: RegisterEditorError) => void;
}

export interface IRegisterEditorProps {
  loading?: boolean;
}

export interface IRegisterEditorData {
  nickname: string;
  login: string;
  password: string;
}

export interface IRegisterEditorState {
  nickname: string;
  login: string;
  password: string;
  confirmPassword: string;
}

export enum RegisterEditorError {
  LoginAlreadyExist
}

interface IRegisterEditorValidationResult {
  nicknameError?: string;
  loginError?: string;
  passwordError?: string;
  confirmPasswordError?: string;
}

export const RegisterEditor = forwardRef<IRegisterEditorHandle, IRegisterEditorProps>((props, ref) => {
  const {loading} = props;

  const [state, setState] = useState<IRegisterEditorState>({
    nickname: "",
    login: "",
    password: "",
    confirmPassword: "",
  });

  const [validationResult, setValidationResult] = useState<IRegisterEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (state.nickname === "") {
        newValidationResult.nicknameError = "Nickname is required";
      }

      if (state.login === "") {
        newValidationResult.loginError = "Login required";
      }

      if (state.password === "") {
        newValidationResult.passwordError = "Password required";
      }

      setValidationResult(newValidationResult);
      return newValidationResult.nicknameError === undefined
          && newValidationResult.loginError === undefined
          && newValidationResult.passwordError === undefined
          && newValidationResult.confirmPasswordError === undefined;
    },
    data() {
      const data: IRegisterEditorData = {
        nickname: state.nickname,
        login: state.login,
        password: state.password,
      };

      return data;
    },
    setError: (error: RegisterEditorError) => {
      if (error === RegisterEditorError.LoginAlreadyExist) {
        setValidationResult({
          ...validationResult,
          loginError: "Login already exists",
        });
      }
    }
  }));

  function validate(state: IRegisterEditorState): IRegisterEditorValidationResult {
    const result: IRegisterEditorValidationResult = {};

    if (state.confirmPassword !== "" && state.password !== state.confirmPassword) {
      result.confirmPasswordError = "Passwords not match";
    }

    return result;
  }

  function change(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...state,
      [e.target.name]: e.target.value
    };

    const validationResult = validate(newData);

    setState(newData);
    setValidationResult(validationResult);
  }

  return (
      <>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            name="nickname"
            placeholder="Nickname"
            value={state.nickname}
            error={validationResult.nicknameError}
            onChange={change}/>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            name="login"
            placeholder="Login"
            value={state.login}
            error={validationResult.loginError}
            onChange={change}/>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            type="password"
            name="password"
            placeholder="Password"
            value={state.password}
            error={validationResult.passwordError}
            onChange={change}/>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            type="password"
            name="confirmPassword"
            placeholder="Confirm password"
            value={state.confirmPassword}
            error={validationResult.confirmPasswordError}
            onChange={change}/>
      </>
  );
})