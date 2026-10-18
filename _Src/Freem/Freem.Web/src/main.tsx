import "primereact/resources/themes/lara-light-indigo/theme.css";
import 'primeicons/primeicons.css';
import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {PrimeReactProvider} from 'primereact/api';
import {RouterProvider} from 'react-router';
import {router} from "./app/router.tsx";
import {BackendClientsProvider} from "./clients/context/BackendClientsProvider.tsx";
import {DialogProvider} from "./dialog/context/DialogProvider.tsx";
import {EntitiesCacheProvider} from "./cache/context/EntitiesCacheProvider.tsx";
import {AuthProvider} from "./authorization/context/AuthProvider.tsx";
import {AuthGate} from "./authorization/components/AuthGate.tsx";
import "./main.css"

const root = document.getElementById('root')!;

createRoot(root).render(
    <StrictMode>
      <PrimeReactProvider>
        <BackendClientsProvider>
          <AuthProvider>
            <AuthGate>
              <EntitiesCacheProvider>
                <DialogProvider>
                  <RouterProvider router={router}/>
                </DialogProvider>
              </EntitiesCacheProvider>
            </AuthGate>
          </AuthProvider>
        </BackendClientsProvider>
      </PrimeReactProvider>
    </StrictMode>
)
