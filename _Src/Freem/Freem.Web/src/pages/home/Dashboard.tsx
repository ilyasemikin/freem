import {ActivitiesDashboard} from "../activities/ActivitiesDashboard.tsx";
import {DashboardPanel} from "../../components/DashboardPanel.tsx";
import {RecordsDashboard} from "../records/RecordsDashboard.tsx";
import {useState} from "react";
import {TagsDashboard} from "../tags/TagsDashboard.tsx";
import {StatisticsDashboard} from "../statistics/StatisticsDashboard.tsx";
import {Button} from "primereact/button";
import {AppHeader} from "../../components/AppHeader.tsx";

enum Tab {
  Tags,
  Records,
  Statistics
}

export function Dashboard() {
  const [tab, setTab] = useState<Tab>(Tab.Records);
  const [recordsVersion, setRecordsVersion] = useState(0);

  function recordsChanged() {
    setRecordsVersion(value => value + 1);
  }

  function active(buttonTab: Tab) {
    return buttonTab === tab;
  }

  function tabButton(buttonTab: Tab, label: string) {
    return (
        <Button text={!active(buttonTab)} size="small" aria-pressed={active(buttonTab)}
                onClick={() => setTab(buttonTab)}>{label}</Button>
    );
  }

  function renderRight() {
    switch (tab) {
      case Tab.Tags:
        return <TagsDashboard/>;
      case Tab.Records:
        return <RecordsDashboard version={recordsVersion}/>;
      case Tab.Statistics:
        return <StatisticsDashboard version={recordsVersion}/>;
    }
  }

  function renderRightFooter() {
    return (
        <div className="segmented">
          {tabButton(Tab.Tags, "Tags")}
          {tabButton(Tab.Records, "Records")}
          {tabButton(Tab.Statistics, "Statistics")}
        </div>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <AppHeader/>
        <div className="dashboard">
          <DashboardPanel>
            <ActivitiesDashboard onRecordsChanged={recordsChanged}/>
          </DashboardPanel>
          <DashboardPanel
              children={renderRight()}
              footer={(renderRightFooter())}/>
        </div>
      </div>
)
}