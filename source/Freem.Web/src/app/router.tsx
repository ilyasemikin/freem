import {createBrowserRouter} from 'react-router';
import {ActivitiesDashboard} from "../pages/activities/ActivitiesDashboard.tsx";
import {TagsDashboard} from "../pages/tags/TagsDashboard.tsx";
import {RecordsDashboard} from "../pages/records/RecordsDashboard.tsx";
import {StatisticsDashboard} from "../pages/statistics/StatisticsDashboard.tsx";
import {Dashboard} from "../pages/home/Dashboard.tsx";
import {SettingsPage} from "../pages/settings/SettingsPage.tsx";
import {ExportPage} from "../pages/dataTransfer/export/ExportPage.tsx";
import {ImportListPage} from "../pages/dataTransfer/import/ImportListPage.tsx";
import {ImportPage} from "../pages/dataTransfer/import/ImportPage.tsx";

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
    path: "/data-transfer/imports",
    element:
        <ImportListPage/>
  },
  {
    path: "/data-transfer/imports/:id",
    element:
        <ImportPage/>
  },
  {
    path: "/data-transfer/exports",
    element:
        <ExportPage/>
  }
], {
  basename: import.meta.env.BASE_URL
});
