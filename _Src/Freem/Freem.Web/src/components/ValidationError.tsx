export interface IValidationErrorProps {
  error?: string;
}

export function ValidationError(props: IValidationErrorProps) {
  const {error} = props;

  return (
      <>
        {error !== undefined &&
            <span style={{textAlign: "left", margin: "1px 0 3px 0", fontSize: "13px", color: "red"}}>
              {error}
            </span>}
      </>
  );
}