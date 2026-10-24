import {TimeDuration} from "../data/TimeDuration.ts";

export interface ITimeDurationPanelProps {
  duration?: TimeDuration;
  align?: "center" | "right";
}

export function TimeDurationPanel(props: ITimeDurationPanelProps) {
  const {duration, align} = props;

  const text = duration?.toString() || "00:00:00";
  return (
      <span className="numeric" style={{marginRight: "10px", width: "9ch", textAlign: align || "center"}}>
        {text}
      </span>
  );
}
