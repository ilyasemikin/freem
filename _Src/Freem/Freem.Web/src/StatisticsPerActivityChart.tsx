import {StatisticsPerActivityDonutChart} from "./StatisticsPerActivityDonutChart.tsx";
import {StatisticsPerActivityEntity} from "./entities/StatisticsPerActivityEntity.ts";
import {useState} from "react";
import {TimeDuration} from "./data/TimeDuration.ts";
import {NamedTimeDurationSummary} from "./components/NamedTimeDurationSummary.tsx";

export interface IStatisticsPerActivityChartProps {
  total?: TimeDuration;
  entities?: StatisticsPerActivityEntity[];
}

export function StatisticsPerActivityChart(props: IStatisticsPerActivityChartProps) {
  const {total, entities} = props;

  const [selected, setSelected] = useState<StatisticsPerActivityEntity | undefined>();

  if (entities === undefined || entities.length === 0) {
    return (<></>);
  }

  const name = selected?.activity.name || "Total";
  const duration = selected?.recordedTime || total || new TimeDuration(0);

  return (
      <StatisticsPerActivityDonutChart activities={entities} onSelected={setSelected}>
        <NamedTimeDurationSummary name={name} duration={duration}/>
      </StatisticsPerActivityDonutChart>
  );
}