import {StatisticsPerDayEntity} from "./entities/StatisticsPerDayEntity.ts";
import {StatisticsPerActivityList} from "./StatisticsPerActivityList.tsx";

export interface IStatisticsListProps {
  statistics?: StatisticsPerDayEntity[];
}

export function StatisticsList(props: IStatisticsListProps) {
  const {statistics} = props;

  return (
      <>
        {statistics && statistics.map(entity =>
            <StatisticsPerActivityList
                key={entity.period.toString()}
                entities={entity.activities}/>)}
      </>
  )
}