import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {useState} from "react";
import {AutoComplete, AutoCompleteChangeEvent, AutoCompleteCompleteEvent} from "primereact/autocomplete";
import {FindActivityByNameRequest} from "../../clients/models/activities/FindActivityByNameRequest.ts";
import {useEntitiesCache} from "../../contexts/cache/useEntitiesCache.ts";
import {ValidationError} from "../ValidationError.tsx";

export interface IActivitiesPickerProps {
  loading?: boolean;
  activities?: ActivityEntity[];
  error?: string;
  onChange?: (activities: ActivityEntity[]) => void;
}

export function ActivitiesPicker(props: IActivitiesPickerProps) {
  const {loading, activities, error, onChange} = props;

  const [suggestions, setSuggestions] = useState<ActivityEntity[]>([]);

  const cache = useEntitiesCache()

  async function search(e: AutoCompleteCompleteEvent) {
    if (e.query.trim().length === 0) {
      return undefined;
    }

    const request = new FindActivityByNameRequest(e.query.trim());
    const entities = await cache.activities.findByNameRequest(request);

    const ids = activities?.map(activity => activity.id);
    setSuggestions(entities.filter(activity => !ids?.find(id => id === activity.id)));
  }

  function update(e: AutoCompleteChangeEvent) {
    if (onChange !== undefined) {
      onChange(e.value);
    }
  }

  function itemTemplate(activity: ActivityEntity) {
    return (<span>{activity.name}</span>);
  }

  function selectedItemTemplate(activity: ActivityEntity) {
    return (<span>{activity.name}</span>)
  }

  return (
      <>
        <AutoComplete
            style={{display: "block", width: "100%", marginBottom: "3px"}}
            placeholder="Activities"
            disabled={loading}
            value={activities}
            suggestions={suggestions}
            itemTemplate={itemTemplate}
            selectedItemTemplate={selectedItemTemplate}
            completeMethod={search}
            onChange={update}
            multiple/>
        <ValidationError error={error}/>
      </>
  );
}