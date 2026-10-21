import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {ColorDot} from "../../components/ColorDot.tsx";

export interface IActivitiesListPanelProps {
  activities?: ActivityEntity[];
}

export function ActivitiesListPanel(props: IActivitiesListPanelProps) {
  const {activities} = props;

  const name = activities?.map(activity => activity.name).join(', ') || "Unknown";

  return (
      <span className="item-title">
        {activities?.map(activity => <ColorDot key={activity.id} color={activity.color}/>)}
        {name}
      </span>
  );
}