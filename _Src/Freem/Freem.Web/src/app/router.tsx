import {createBrowserRouter} from 'react-router';
import {ActivitiesDashboard} from "../pages/activities/ActivitiesDashboard.tsx";
import {TagsDashboard} from "../pages/tags/TagsDashboard.tsx";
import {RecordsDashboard} from "../pages/records/RecordsDashboard.tsx";
import {StatisticsDashboard} from "../pages/statistics/StatisticsDashboard.tsx";
import {Dashboard} from "../pages/home/Dashboard.tsx";

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
  }
]);
