import {StatisticsPerActivityEntity} from "../../entities/StatisticsPerActivityEntity.ts";
import {StatisticsPerActivityPanel} from "./StatisticsPerActivityPanel.tsx";

export interface IStatisticsPerActivityListProps {
  entities: StatisticsPerActivityEntity[];
}

export function StatisticsPerActivityList(props: IStatisticsPerActivityListProps) {
  const {entities} = props;

  return (
      <>
        {entities.length === 0 && <div className="empty">No data for this period</div>}
        {entities.map(entity =>
            <StatisticsPerActivityPanel
                key={entity.activity.id}
                recorded={entity.recordedTime}
                activity={entity.activity}/>)}
      </>
  );
}