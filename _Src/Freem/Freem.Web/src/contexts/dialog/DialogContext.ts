import {createContext, ReactNode} from "react";

export interface IDialogOptions {
  content: ReactNode;
}

export interface IDialogContext {
  openDialog: (options: IDialogOptions) => void;
  closeDialog: () => void;
}

export const DialogContext = createContext<IDialogContext>({
  openDialog: undefined!,
  closeDialog: undefined!
});