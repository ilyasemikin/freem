import {ChangeEvent, forwardRef, useImperativeHandle, useState} from "react";
import {InputText} from "./components/inputs/InputText.tsx";

export interface ILoginEditorHandle {
  validate: () => boolean;
  data: () => ILoginEditorData;
}

export interface ILoginEditorProps {
  loading?: boolean;
}

export interface ILoginEditorData {
  login: string;
  password: string;
}

interface ILoginEditorValidationResult {
  loginError?: string;
  passwordError?: string;
}

export const LoginEditor = forwardRef<ILoginEditorHandle, ILoginEditorProps>((props, ref) => {
  const {loading} = props;

  const [data, setData] = useState<ILoginEditorData>({
    login: "",
    password: ""
  });

  const [validationResult, setValidationResult] = useState<ILoginEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (data.login === "") {
        newValidationResult.loginError = "Login required";
      }

      if (data.password === "") {
        newValidationResult.passwordError = "Password required";
      }

      setValidationResult(newValidationResult);
      return newValidationResult.loginError === undefined && newValidationResult.passwordError === undefined;
    },
    data() {
      return data;
    }
  }));

  function change(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
    };

    setData(newData);
    setValidationResult({});
  }

  return (
      <div>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            name="login"
            placeholder="Login"
            value={data.login}
            error={validationResult.loginError}
            onChange={change}/>
        <InputText
            style={{margin: "10px 0 0 0"}}
            loading={loading}
            type="password"
            name="password"
            placeholder="Password"
            value={data.password}
            error={validationResult.passwordError}
            onChange={change}/>
      </div>
  );
})