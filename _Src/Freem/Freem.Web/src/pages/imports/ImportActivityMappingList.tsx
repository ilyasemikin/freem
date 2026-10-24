import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";
import {ColorDot} from "../../components/ColorDot.tsx";
import {ImportActivityResponse} from "../../clients/models/imports/ImportActivityResponse.ts";
import {ActivityMappingPicker} from "./ActivityMappingPicker.tsx";

// DataTable memoizes cells by row data, so the highlight is put into it.
interface IImportActivityView {
  activity: ImportActivityResponse;
  unmapped: boolean;
}

export interface IImportActivityMappingListProps {
  activities: ImportActivityResponse[];
  unmappedIds: Set<string>;
  onChange: (importActivityId: string, activityId: string | null) => Promise<void>;
}

export function ImportActivityMappingList(props: IImportActivityMappingListProps) {
  const {activities, unmappedIds, onChange} = props;

  const mappedCount = activities.filter(item => item.mappedActivityId !== null).length;
  const views = activities.map<IImportActivityView>(item => ({activity: item, unmapped: unmappedIds.has(item.id)}));

  function nameTemplate(view: IImportActivityView) {
    const item = view.activity;
    return (
        <span className="item-title">
          {item.color !== null && <ColorDot color={item.color}/>}
          {item.name}
        </span>
    );
  }

  function pickerTemplate(view: IImportActivityView) {
    const item = view.activity;
    return (
        <ActivityMappingPicker
            mappedActivityId={item.mappedActivityId}
            invalid={view.unmapped}
            onChange={activityId => onChange(item.id, activityId)}/>
    );
  }

  return (
      <>
        <h3 className="group-title">Activities: {mappedCount} of {activities.length} mapped</h3>
        <DataTable
            value={views}
            dataKey="activity.id"
            size="small"
            paginator={activities.length > 10}
            rows={10}
            emptyMessage="No activities in the file">
          <Column header="In the file" body={nameTemplate} style={{width: "40%"}}/>
          <Column header="Existing activity" body={pickerTemplate}/>
        </DataTable>
      </>
  );
}
