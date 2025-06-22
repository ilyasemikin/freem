import {TagEntity} from "../../entities/TagEntity.ts";
import {useState} from "react";
import {AutoComplete, AutoCompleteChangeEvent, AutoCompleteCompleteEvent} from "primereact/autocomplete";
import {useEntitiesCache} from "../../contexts/cache/useEntitiesCache.ts";
import {ValidationError} from "../ValidationError.tsx";

export interface ITagsPickerProps {
  loading?: boolean;
  tags?: TagEntity[];
  error?: string;
  onChange: (tags: TagEntity[]) => void;
}

export function TagsPicker(props: ITagsPickerProps) {
  const {loading, tags, error, onChange} = props;

  const [suggestions, setSuggestions] = useState<TagEntity[]>(tags || []);

  const cache = useEntitiesCache();

  async function search(e: AutoCompleteCompleteEvent) {
    if (e.query.trim().length === 0) {
      return undefined;
    }

    const name = e.query.trim();
    const entities = await cache.tags.findByNameRequest(name);

    const ids = tags?.map(tag => tag.id);
    const suggestions = entities.filter(tag => !ids?.find(id => id === tag.id));
    setSuggestions(suggestions);
  }

  function update(e: AutoCompleteChangeEvent) {
    if (onChange !== undefined) {
      onChange(e.value);
    }
  }

  function itemTemplate(tag: TagEntity) {
    return (<span>{tag.name}</span>)
  }

  function selectedItemTemplate(tag: TagEntity) {
    return (<span>{tag.name}</span>)
  }

  return (
      <>
        <AutoComplete
            style={{display: "block", width: "100%", marginBottom: "3px"}}
            placeholder="Tags"
            disabled={loading}
            value={tags}
            suggestions={suggestions}
            itemTemplate={itemTemplate}
            selectedItemTemplate={selectedItemTemplate}
            completeMethod={search}
            onChange={update}
            multiple/>
        <ValidationError error={error}/>
      </>
  )
}