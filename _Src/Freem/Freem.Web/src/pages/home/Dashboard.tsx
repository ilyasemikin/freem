import {ActivitiesDashboard} from "../activities/ActivitiesDashboard.tsx";
import {DashboardPanel} from "../../components/DashboardPanel.tsx";
import {RecordsDashboard} from "../records/RecordsDashboard.tsx";
import {useState} from "react";
import {TagsDashboard} from "../tags/TagsDashboard.tsx";
import {StatisticsDashboard} from "../statistics/StatisticsDashboard.tsx";
import {Button} from "primereact/button";
import {useAuth} from "../../authorization/context/useAuth.ts";

enum Tab {
  Tags,
  Records,
  Statistics
}

export function Dashboard() {
  const [tab, setTab] = useState<Tab>(Tab.Records);
  const {logout} = useAuth();

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
          <Button size="small" severity="secondary" onClick={logout}>Log out</Button>
        </div>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
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