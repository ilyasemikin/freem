import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {ItemPanel} from "../../components/ItemPanel.tsx";
import {TimeDuration} from "../../data/TimeDuration.ts";
import {ColorDot} from "../../components/ColorDot.tsx";

export interface IStatisticsPerActivityPanelProps {
  recorded: TimeDuration;
  activity: ActivityEntity;
}

export function StatisticsPerActivityPanel(props: IStatisticsPerActivityPanelProps) {
  const {recorded, activity} = props;

  return (
      <ItemPanel color={activity.color}>
        <span className="item-title">
          <ColorDot color={activity.color}/>
          {activity.name}
        </span>
        <span className="numeric muted">{recorded.toString()}</span>
      </ItemPanel>
  );
}