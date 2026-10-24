import {useEffect, useState} from "react";
import {AutoComplete, AutoCompleteCompleteEvent, AutoCompleteSelectEvent} from "primereact/autocomplete";
import {Button} from "primereact/button";
import {TagEntity} from "../../../entities/TagEntity.ts";
import {useEntitiesCache} from "../../../cache/context/useEntitiesCache.ts";

export interface ITagMappingPickerProps {
  mappedTagId: string | null;
  invalid?: boolean;
  onChange: (tagId: string | null) => Promise<void>;
}

// Picks one existing tag of the user by name.
export function TagMappingPicker(props: ITagMappingPickerProps) {
  const {mappedTagId, invalid, onChange} = props;

  const cache = useEntitiesCache();

  const [mapped, setMapped] = useState<TagEntity | undefined>();
  const [value, setValue] = useState<TagEntity | string | undefined>();
  const [suggestions, setSuggestions] = useState<TagEntity[]>([]);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    let current = true;

    async function load() {
      const entities = mappedTagId !== null
          ? await cache.tags.list([mappedTagId])
          : [];
      const entity = entities[0];
      if (current) {
        setMapped(entity);
        setValue(entity);
      }
    }

    load().catch(() => undefined);

    return () => {
      current = false;
    };
  }, [mappedTagId, cache.tags]);

  async function search(e: AutoCompleteCompleteEvent) {
    const text = e.query.trim();
    if (text.length === 0) {
      setSuggestions([]);
      return;
    }

    const entities = await cache.tags.findByNameRequest(text);
    setSuggestions(entities);
  }

  async function save(tagId: string | null) {
    setSaving(true);
    try {
      await onChange(tagId);
    } finally {
      setSaving(false);
    }
  }

  async function select(e: AutoCompleteSelectEvent) {
    const entity: TagEntity = e.value;
    await save(entity.id);
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
