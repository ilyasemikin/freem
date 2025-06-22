import {TimeDuration} from "../data/TimeDuration.ts";

export interface INamedTimeDurationSummaryProps {
  name: string;
  duration: TimeDuration;
}

export function NamedTimeDurationSummary(props: INamedTimeDurationSummaryProps) {
  const {name, duration} = props;

  return (
      <div style={{display: "flex", flexDirection: "column", alignItems: "center"}}>
        <span>{name}</span>
        <span>{duration.toString()}</span>
      </div>
  )
}