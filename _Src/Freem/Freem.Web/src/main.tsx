import "primereact/resources/themes/lara-light-indigo/theme.css";
import 'primeicons/primeicons.css';
import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {PrimeReactProvider} from 'primereact/api';
import {createBrowserRouter, RouterProvider} from 'react-router';
import {LoginModel} from './LoginModel';
import {RegisterModel} from './RegisterModel';
import {RemindPasswordModel} from './RemindPasswordModel.tsx';
import {BackendClientsProvider} from "./contexts/clients/BackendClientsProvider.tsx";
import {AuthorizationProvider} from "./contexts/authorization/AuthorizationProvider.tsx";
import {PrivateRoute} from "./components/routing/PrivateRoute.tsx";
import {PublicRoute} from "./components/routing/PublicRoute.tsx";
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
      <PrivateRoute>
        <Dashboard/>
      </PrivateRoute>
  },
  {
    path: "/login",
    element:
        <PublicRoute>
          <LoginModel/>
        </PublicRoute>
  },
  {
    path: "/register",
    element:
        <PublicRoute>
          <RegisterModel/>
        </PublicRoute>
  },
  {
    path: "/remind",
    element:
        <PublicRoute>
          <RemindPasswordModel/>
        </PublicRoute>
  },
  {
    path: "/activities",
    element:
        <PrivateRoute>
          <ActivitiesDashboard/>
        </PrivateRoute>
  },
  {
    path: "/records",
    element:
        <PrivateRoute>
          <RecordsDashboard/>
        </PrivateRoute>
  },
  {
    path: "/records/statistics",
    element:
        <PrivateRoute>
          <StatisticsDashboard/>
        </PrivateRoute>
  },
  {
    path: "/tags",
    element:
        <PrivateRoute>
          <TagsDashboard/>
        </PrivateRoute>
  }
]);

const root = document.getElementById('root')!;

createRoot(root).render(
    <StrictMode>
      <PrimeReactProvider>
        <BackendClientsProvider>
          <EntitiesCacheProvider>
            <AuthorizationProvider>
              <DialogProvider>
                <RouterProvider router={router}/>
              </DialogProvider>
            </AuthorizationProvider>
          </EntitiesCacheProvider>
        </BackendClientsProvider>
      </PrimeReactProvider>
    </StrictMode>
)
