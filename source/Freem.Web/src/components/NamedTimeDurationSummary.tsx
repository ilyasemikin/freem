import {TimeDuration} from "../data/TimeDuration.ts";

export interface INamedTimeDurationSummaryProps {
  name: string;
  duration: TimeDuration;
}

export function NamedTimeDurationSummary(props: INamedTimeDurationSummaryProps) {
  const {name, duration} = props;

  return (
      <div style={{display: "flex", flexDirection: "column", alignItems: "center"}}>
        <span className="muted" style={{fontSize: "13px"}}>{name}</span>
        <span className="numeric" style={{fontSize: "1.25rem", fontWeight: 600}}>{duration.toString()}</span>
      </div>
  )
}