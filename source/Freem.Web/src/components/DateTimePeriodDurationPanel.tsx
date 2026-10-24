import {DateTimePeriod} from "../data/DateTimePeriod.ts";

export interface IDateTimePeriodDurationPanelProps {
  period?: DateTimePeriod;
  align?: "center" | "right";
}

export function DateTimePeriodDurationPanel(props: IDateTimePeriodDurationPanelProps) {
  const {period, align} = props;

  const duration = period?.getDurationFormatted() || "00:00:00";
  return (
      <span className="numeric" style={{marginRight: "10px", width: "9ch", textAlign: align || "center"}}>
        {duration}
      </span>
  );
}
