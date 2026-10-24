export interface IColorDotProps {
  color: string;
}

export function ColorDot(props: IColorDotProps) {
  const {color} = props;

  return (
      <span className="color-dot" style={{background: color}} aria-hidden="true"/>
  );
}
