import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {ItemPanel} from "../../components/ItemPanel.tsx";
import {TimeDuration} from "../../data/TimeDuration.ts";
import {ActivityColor} from "../../helpers/ActivityColor.ts";

export interface IStatisticsPerActivityPanelProps {
  recorded: TimeDuration;
  activity: ActivityEntity;
}

export function StatisticsPerActivityPanel(props: IStatisticsPerActivityPanelProps) {
  const {recorded, activity} = props;

  return (
      <ItemPanel>
        <span className="item-title">
          <span style={{display: "inline-block", width: "10px", height: "10px", marginRight: "10px", borderRadius: "50%", background: ActivityColor.fromId(activity.id)}}/>
          {activity.name}
        </span>
        <span className="numeric muted">{recorded.toString()}</span>
      </ItemPanel>
  );
}