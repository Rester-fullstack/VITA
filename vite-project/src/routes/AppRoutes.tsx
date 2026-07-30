import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";

import MainLayout from
"../components/layout/MainLayout";

import PrivateRoute from
"./PrivateRoute";

import RoleRoute from
"./RoleRoute";

import Login from
"../pages/Login/Login";

import Dashboard from
"../pages/Dashboard/Dashboard";

import Pacientes from
"../pages/Pacientes/Pacientes";

import Consultas from
"../pages/Consultas/Consultas";

import Exames from
"../pages/Exames/Exames";

import Medicos from
"../pages/Medicos/Medicos";

import MedicoDashboard from
"../pages/MedicoDashboard/MedicoDashboard";

import MinhasConsultas from 
"../pages/MinhasConsultas/MinhasConsultas";

import ConsultaDetalhes from
"../pages/ConsultaDetalhes/ConsultaDetalhes";

import PacienteDetalhes from
"../pages/PacienteDetalhes/PacienteDetalhes";

import Odontologia from "../pages/Odontologia/Odontologia";

import Psicologia from "../pages/Psicologia/Psicologia";

import Nutricao from "../pages/Nutricao/Nutricao";

import MeusPacientes from "../pages/MeusPacientes/MeusPacientes";

import AgendaMedico from "../pages/AgendaMedico/AgendaMedico";

import Configuracoes from "../pages/Configuracoes/Configuracoes";

import MeuPerfil from "../pages/MeuPerfil/MeuPerfil";

import Ajuda from "../pages/Ajuda/Ajuda";

export default function AppRoutes() {

  return (

    <BrowserRouter>

      <Routes>     

        <Route
          path="/"
          element={<Login />}
        />


        <Route
          path="/dashboard"
          element={
            <RoleRoute role="Admin">

              <MainLayout>
                <Dashboard />
              </MainLayout>

            </RoleRoute>
          }
        />

        <Route
          path="/pacientes"
          element={
            <RoleRoute role="Admin">

              <MainLayout>
                <Pacientes />
              </MainLayout>

            </RoleRoute>
          }
        />


        <Route
          path="/medicos"
          element={
            <RoleRoute role="Admin">

              <MainLayout>
                <Medicos />
              </MainLayout>

            </RoleRoute>
          }
        />


        <Route
          path="/consultas"
          element={
            <PrivateRoute>

              <MainLayout>
                <Consultas />
              </MainLayout>

            </PrivateRoute>
          }
        />


        <Route
          path="/exames"
          element={
            <PrivateRoute>

              <MainLayout>
                <Exames />
              </MainLayout>

            </PrivateRoute>
          }
        />


        <Route
          path="/dashboard-medico"
          element={
            <RoleRoute role="Medico">

              <MainLayout>
                <MedicoDashboard />
              </MainLayout>

            </RoleRoute>
          }
        />


        <Route
          path="/minhas-consultas"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <MinhasConsultas />
              </MainLayout>
            </RoleRoute>
          }
        />


        <Route
          path="/consulta/:id"
          element={
            <RoleRoute role="Medico">

              <MainLayout>
                <ConsultaDetalhes />
              </MainLayout>

            </RoleRoute>
          }
        />


        <Route
          path="/paciente/:id"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <PacienteDetalhes />
              </MainLayout>
            </RoleRoute>
          }
        />

        <Route
          path="/consulta/:id/odontologia"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <Odontologia />
              </MainLayout>
            </RoleRoute>
          }
        />

        <Route
          path="/consulta/:id/psicologia"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <Psicologia />
              </MainLayout>
            </RoleRoute>
          }
        />

        <Route
          path="/consulta/:id/nutricao"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <Nutricao />
              </MainLayout>
            </RoleRoute>
          }
        />

        <Route
          path="/meus-pacientes"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <MeusPacientes />
              </MainLayout>
            </RoleRoute>
          }
        />


        <Route
          path="/agenda"
          element={
            <RoleRoute role="Medico">
              <MainLayout>
                <AgendaMedico />
              </MainLayout>
            </RoleRoute>
          }
        />

        <Route
          path="/configuracoes"
          element={
            <RoleRoute role="Admin">

              <MainLayout>
                <Configuracoes />
              </MainLayout>

            </RoleRoute>
          }
        />

        <Route
          path="/meu-perfil"
          element={
            <RoleRoute role="Medico">

              <MainLayout>
                <MeuPerfil />
              </MainLayout>

            </RoleRoute>
          }
        />

        <Route
          path="/ajuda"
          element={
            <RoleRoute role="Medico">

              <MainLayout>
                <Ajuda />
              </MainLayout>

            </RoleRoute>
          }
        />

      </Routes>

    </BrowserRouter>

  );
}