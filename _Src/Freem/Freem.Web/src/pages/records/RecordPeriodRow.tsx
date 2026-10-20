import {ReactNode} from "react";
import {DatePicker} from "../../components/inputs/DatePicker.tsx";
import {InputText} from "../../components/inputs/InputText.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {IPeriodRowState} from "../../helpers/RecordPeriodsHelper.ts";

export interface IRecordPeriodRowProps {
  row: IPeriodRowState;
  loading?: boolean;
  error?: string;
  actions?: ReactNode;

  onChange: (row: IPeriodRowState) => void;
}

export function RecordPeriodRow(props: IRecordPeriodRowProps) {
  const {row, loading, error, actions, onChange} = props;

  return (
      <div style={{display: "flex", flexDirection: "column", marginBottom: "6px"}}>
        <div style={{display: "flex", flexDirection: "row", alignItems: "flex-start"}}>
          <DatePicker
              style={{width: "50%", marginRight: "3px"}}
              placeholder="Start at"
              showTime
              disabled={loading}
              value={row.startAt}
              onChange={value => onChange({...row, startAt: value})}/>
          <DatePicker
              style={{width: "50%"}}
              placeholder="End at"
              showTime
              disabled={loading}
              value={row.endAt}
              onChange={value => onChange({...row, endAt: value})}/>
        </div>
        <div style={{display: "flex", flexDirection: "row", alignItems: "flex-start", gap: "6px"}}>
          <div style={{flex: 1}}>
            <InputText
                size="medium"
                placeholder="Period description"
                loading={loading}
                value={row.description}
                onChange={e => onChange({...row, description: e.target.value})}/>
          </div>
          {actions}
        </div>
        <ValidationError error={error}/>
      </div>
  );
}
