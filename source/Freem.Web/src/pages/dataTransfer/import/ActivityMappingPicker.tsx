import {useEffect, useState} from "react";
import {AutoComplete, AutoCompleteCompleteEvent, AutoCompleteSelectEvent} from "primereact/autocomplete";
import {Button} from "primereact/button";
import {ActivityEntity} from "../../../entities/ActivityEntity.ts";
import {useEntitiesCache} from "../../../cache/context/useEntitiesCache.ts";
import {FindActivityByNameRequest} from "../../../clients/models/activities/FindActivityByNameRequest.ts";
import {ColorDot} from "../../../components/ColorDot.tsx";

export interface IActivityMappingPickerProps {
  mappedActivityId: string | null;
  invalid?: boolean;
  onChange: (activityId: string | null) => Promise<void>;
}

// Picks one existing activity of the user by name; archived activities are not offered.
export function ActivityMappingPicker(props: IActivityMappingPickerProps) {
  const {mappedActivityId, invalid, onChange} = props;

  const cache = useEntitiesCache();

  const [mapped, setMapped] = useState<ActivityEntity | undefined>();
  const [value, setValue] = useState<ActivityEntity | string | undefined>();
  const [suggestions, setSuggestions] = useState<ActivityEntity[]>([]);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    let current = true;

    async function load() {
      const entity = mappedActivityId !== null
          ? await cache.activities.get(mappedActivityId)
          : undefined;
      if (current) {
        setMapped(entity);
        setValue(entity);
      }
    }

    load().catch(() => undefined);

    return () => {
      current = false;
    };
  }, [mappedActivityId, cache.activities]);

  async function search(e: AutoCompleteCompleteEvent) {
    const text = e.query.trim();
    if (text.length === 0) {
      setSuggestions([]);
      return;
    }

    const request = new FindActivityByNameRequest(text);
    const entities = await cache.activities.findByNameRequest(request);
    setSuggestions(entities.filter(activity => !activity.archived));
  }

  async function save(activityId: string | null) {
    setSaving(true);
    try {
      await onChange(activityId);
    } finally {
      setSaving(false);
    }
  }

  async function select(e: AutoCompleteSelectEvent) {
    const entity: ActivityEntity = e.value;
    await save(entity.id);
  }

  function itemTemplate(activity: ActivityEntity) {
    return (<span><ColorDot color={activity.color}/>{activity.name}</span>);
  }

  return (
      <div style={{display: "flex", flexDirection: "row", gap: "4px", alignItems: "center"}}>
        <AutoComplete
            style={{flex: 1}}
            inputStyle={{width: "100%"}}
            placeholder="Not mapped"
            field="name"
            value={value}
            suggestions={suggestions}
            itemTemplate={itemTemplate}
            invalid={invalid}
            disabled={saving}
            completeMethod={search}
            onChange={e => setValue(e.value)}
            onSelect={select}
            onBlur={() => setValue(mapped)}/>
        <Button text rounded size="small" severity="secondary" icon="pi pi-times" aria-label="Remove mapping"
                disabled={mapped === undefined || saving} onClick={() => save(null)}/>
      </div>
  );
}
