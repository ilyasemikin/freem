import {ReactNode, useState} from "react";
import {Dialog} from "primereact/dialog";
import {DialogWindow} from "../../components/DialogWindow.tsx";
import {DialogContext, IDialogOptions} from "./DialogContext.ts";

interface IDialogProviderProps {
  children: ReactNode;
}

export function DialogProvider(props: IDialogProviderProps) {
  const {children} = props;

  const [visible, setVisible] = useState(false);
  const [content, setContent] = useState<ReactNode>(null);

  function openDialog(options: IDialogOptions) {
    setContent(options.content);
    setVisible(true);
  }

  function closeDialog() {
    setVisible(false);
  }

  return (
      <DialogContext.Provider value={{openDialog, closeDialog}}>
        <>
          <Dialog
              style={{width: "500px"}}
              visible={visible}
              onHide={closeDialog}
              content={<DialogWindow children={content}/>}>
          </Dialog>
        </>
        {children}
      </DialogContext.Provider>
  )
}