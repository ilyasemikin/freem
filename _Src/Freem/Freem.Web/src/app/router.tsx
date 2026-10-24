import {createBrowserRouter} from 'react-router';
import {ActivitiesDashboard} from "../pages/activities/ActivitiesDashboard.tsx";
import {TagsDashboard} from "../pages/tags/TagsDashboard.tsx";
import {RecordsDashboard} from "../pages/records/RecordsDashboard.tsx";
import {StatisticsDashboard} from "../pages/statistics/StatisticsDashboard.tsx";
import {Dashboard} from "../pages/home/Dashboard.tsx";
import {SettingsPage} from "../pages/settings/SettingsPage.tsx";
import {ExportPage} from "../pages/export/ExportPage.tsx";
import {ImportsPage} from "../pages/imports/ImportsPage.tsx";
import {ImportPage} from "../pages/imports/ImportPage.tsx";

export const router = createBrowserRouter([
  {
    path: "/",
    element:
      <Dashboard/>
  },
  {
    path: "/activities",
    element:
        <ActivitiesDashboard/>
  },
  {
    path: "/records",
    element:
        <RecordsDashboard/>
  },
  {
    path: "/records/statistics",
    element:
        <StatisticsDashboard/>
  },
  {
    path: "/tags",
    element:
        <TagsDashboard/>
  },
  {
    path: "/settings",
    element:
        <SettingsPage/>
  },
  {
    path: "/imports",
    element:
        <ImportsPage/>
  },
  {
    path: "/imports/:id",
    element:
        <ImportPage/>
  },
  {
    path: "/export",
    element:
        <ExportPage/>
  }
]);
