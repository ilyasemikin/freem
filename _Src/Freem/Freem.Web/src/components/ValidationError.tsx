export interface IValidationErrorProps {
  error?: string;
}

export function ValidationError(props: IValidationErrorProps) {
  const {error} = props;

  return (
      <>
        {error !== undefined && <span className="error-text">{error}</span>}
      </>
  );
}
