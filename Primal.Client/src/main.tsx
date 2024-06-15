import React from 'react'
import ReactDOM from 'react-dom/client'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import './index.css'
import "./App.css";
import { EncounterBuilder } from './EncounterBuilder/EncounterBuilder.tsx';
import { SplashScreen } from './SplashScreen/SplashScreen.tsx';
import { MainMenu } from './MainMenu/MainMenu.tsx';
import { Game } from './Game/components/Game.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <SplashScreen />,
  },
  {
    path: "/main-menu",
    element: <MainMenu />,
  },
  {
    path: "/game",
    element: <Game />,
  },
  {
    path: "/encounter-builder",
    element: <EncounterBuilder/>
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
