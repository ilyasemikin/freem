import {Button} from "primereact/button";

export interface IDialogFooterProps {
  loading?: boolean;
  actionText?: string;
  onAction?: () => void;
  onCancel?: () => void;
}

export function DialogFooter(props: IDialogFooterProps) {
  const {loading, actionText, onAction, onCancel} = props;

  const text = actionText || "Action";

  return (
      <div style={{display: "flex", justifyContent: "space-between", gap: "8px", marginTop: "12px"}}>
        <Button
            style={{flex: 1}}
            size="small"
            label={text}
            loading={loading}
            onClick={onAction}/>
        <Button
            style={{flex: 1}}
            size="small"
            severity="secondary"
            outlined
            label="Cancel"
            onClick={onCancel}/>
      </div>
  );
}