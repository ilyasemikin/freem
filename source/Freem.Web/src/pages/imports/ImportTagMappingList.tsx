import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";
import {ImportTagResponse} from "../../clients/models/imports/ImportTagResponse.ts";
import {TagMappingPicker} from "./TagMappingPicker.tsx";

// DataTable memoizes cells by row data, so the highlight is put into it.
interface IImportTagView {
  tag: ImportTagResponse;
  unmapped: boolean;
}

export interface IImportTagMappingListProps {
  tags: ImportTagResponse[];
  unmappedIds: Set<string>;
  onChange: (importTagId: string, tagId: string | null) => Promise<void>;
}

export function ImportTagMappingList(props: IImportTagMappingListProps) {
  const {tags, unmappedIds, onChange} = props;

  const mappedCount = tags.filter(item => item.mappedTagId !== null).length;
  const views = tags.map<IImportTagView>(item => ({tag: item, unmapped: unmappedIds.has(item.id)}));

  function nameTemplate(view: IImportTagView) {
    return (<span className="item-title">{view.tag.name}</span>);
  }

  function pickerTemplate(view: IImportTagView) {
    const item = view.tag;
    return (
        <TagMappingPicker
            mappedTagId={item.mappedTagId}
            invalid={view.unmapped}
            onChange={tagId => onChange(item.id, tagId)}/>
    );
  }

  return (
      <>
        <h3 className="group-title">Tags: {mappedCount} of {tags.length} mapped</h3>
        <DataTable
            value={views}
            dataKey="tag.id"
            size="small"
            paginator={tags.length > 10}
            rows={10}>
          <Column header="In the file" body={nameTemplate} style={{width: "40%"}}/>
          <Column header="Existing tag" body={pickerTemplate}/>
        </DataTable>
      </>
  );
}
