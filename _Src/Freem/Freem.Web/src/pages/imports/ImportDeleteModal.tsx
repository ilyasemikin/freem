import {useState} from "react";
import {DialogWindow} from "../../dialog/DialogWindow.tsx";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";

export interface IImportDeleteModalProps {
  fileName: string;
  onDelete: () => Promise<void>;
  onCancel: () => void;
}

export function ImportDeleteModal(props: IImportDeleteModalProps) {
  const {fileName, onDelete, onCancel} = props;

  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | undefined>();

  async function remove() {
    setDeleting(true);
    try {
      await onDelete();
    } catch (e) {
      setError(e instanceof Error ? e.message : "Failed to delete the import");
      setDeleting(false);
    }
  }

  return (
      <DialogWindow>
        <h3 style={{marginTop: 0}}>Delete import</h3>
        <p style={{maxWidth: "360px", overflowWrap: "anywhere"}}>
          The import of "{fileName}" and its rows will be deleted. Records already created from it are kept.
        </p>
        <ValidationError error={error}/>
        <DialogFooter loading={deleting} actionText="Delete" onAction={remove} onCancel={onCancel}/>
      </DialogWindow>
  );
}
