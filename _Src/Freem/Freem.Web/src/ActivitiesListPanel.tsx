import {ActivityEntity} from "./entities/ActivityEntity.ts";

export interface IActivitiesListPanelProps {
  activities?: ActivityEntity[];
}

export function ActivitiesListPanel(props: IActivitiesListPanelProps) {
  const {activities} = props;

  const name = activities?.map(activity => activity.name).join(', ') || "Unknown";

  return (
      <span>{name}</span>
  );
}