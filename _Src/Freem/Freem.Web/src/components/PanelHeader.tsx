import {CustomButton} from "./inputs/CustomButton.tsx";

export interface IPanelHeaderProps {
  title: string;

  onAdd?: () => void;
}

export function PanelHeader(props: IPanelHeaderProps) {
  const {title, onAdd} = props;

  return (
      <div className="panel-header">
        <h2 className="panel-title">{title}</h2>
        {onAdd &&
            <CustomButton label={`Add ${title}`} onClick={onAdd}>
              <span className="pi pi-plus"/>
            </CustomButton>}
      </div>
  );
}
