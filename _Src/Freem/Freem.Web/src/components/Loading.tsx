import { ProgressBar } from "primereact/progressbar";

export function Loading() {
  return (
    <ProgressBar
        style={{ height: "2px" }}
        mode="indeterminate"/>
  );
}