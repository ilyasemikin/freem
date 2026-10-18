import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {ItemPanel} from "./components/ItemPanel.tsx";
import {TimeDuration} from "./data/TimeDuration.ts";

export interface IStatisticsPerActivityPanelProps {
  recorded: TimeDuration;
  activity: ActivityEntity;
}

export function StatisticsPerActivityPanel(props: IStatisticsPerActivityPanelProps) {
  const {recorded, activity} = props;

  return (
      <ItemPanel>
        <span>{activity.name}</span>
        <span>{recorded.toString()}</span>
      </ItemPanel>
  );
}