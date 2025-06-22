import {ActivitiesDashboard} from "./ActivitiesDashboard.tsx";
import {DashboardPanel} from "./components/DashboardPanel.tsx";
import {RecordsDashboard} from "./RecordsDashboard.tsx";
import {Header} from "./Header.tsx";
import {useState} from "react";
import {TagsDashboard} from "./TagsDashboard.tsx";
import {StatisticsDashboard} from "./StatisticsDashboard.tsx";
import {Button} from "primereact/button";

enum Tab {
  Tags,
  Records,
  Statistics
}

export function Dashboard() {
  const [tab, setTab] = useState<Tab>(Tab.Records);

  function disabled(buttonTab: Tab) {
    return buttonTab === tab;
  }

  function renderRight() {
    switch (tab) {
      case Tab.Tags:
        return <TagsDashboard/>;
      case Tab.Records:
        return <RecordsDashboard/>;
      case Tab.Statistics:
        return <StatisticsDashboard/>;
    }
  }

  function renderRightFooter() {
    return (
        <div className="p-button-group">
          <Button disabled={disabled(Tab.Tags)} size="small" onClick={() => setTab(Tab.Tags)}>Tags</Button>
          <Button disabled={disabled(Tab.Records)} size="small" onClick={() => setTab(Tab.Records)}>Records</Button>
          <Button disabled={disabled(Tab.Statistics)} size="small" onClick={() => setTab(Tab.Statistics)}>Statistics</Button>
        </div>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <Header/>
        <div style={{display: "flex", flexDirection: "row", flex: "1", overflow: "hidden"}}>
          <DashboardPanel style={{display: "flex", flex: "1"}}>
            <ActivitiesDashboard/>
          </DashboardPanel>
          <DashboardPanel
              style={{display: "flex", flex: "1"}}
              children={renderRight()}
              footer={(renderRightFooter())}/>
        </div>
      </div>
)
}