import "primereact/resources/themes/lara-light-indigo/theme.css";
import 'primeicons/primeicons.css';
import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {PrimeReactProvider} from 'primereact/api';
import {createBrowserRouter, RouterProvider} from 'react-router';
import {BackendClientsProvider} from "./contexts/clients/BackendClientsProvider.tsx";
import {ActivitiesDashboard} from "./ActivitiesDashboard.tsx";
import {DialogProvider} from "./contexts/dialog/DialogProvider.tsx";
import {TagsDashboard} from "./TagsDashboard.tsx";
import {EntitiesCacheProvider} from "./contexts/cache/EntitiesCacheProvider.tsx";
import {RecordsDashboard} from "./RecordsDashboard.tsx";
import {StatisticsDashboard} from "./StatisticsDashboard.tsx";
import {Dashboard} from "./Dashboard.tsx";
import "./main.css"

const router = createBrowserRouter([
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

const root = document.getElementById('root')!;

createRoot(root).render(
    <StrictMode>
      <PrimeReactProvider>
        <BackendClientsProvider>
          <EntitiesCacheProvider>
            <DialogProvider>
              <RouterProvider router={router}/>
            </DialogProvider>
          </EntitiesCacheProvider>
        </BackendClientsProvider>
      </PrimeReactProvider>
    </StrictMode>
)
