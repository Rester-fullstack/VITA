import {
  LayoutDashboard,
  Users,
  CalendarDays,
  FileText,
  Menu,
  X,
  LogOut,
  Stethoscope,
  UserCog,
  Settings,
  CircleHelp
} from "lucide-react";

import {
  NavLink,
  useNavigate
} from "react-router-dom";

import {
  useState
} from "react";

import {
  logout,
  getToken
} from "../../storage/auth";

import {
  getRoleFromToken
} from "../../utils/jwt";

import "./Sidebar.css";

export default function Sidebar() {

  const [open, setOpen] = useState(false);

  const navigate = useNavigate();

  const token = getToken();

  const role = getRoleFromToken(token || "");

  function handleLogout() {
    logout();
    navigate("/");
  }

  function closeMenu() {
    setOpen(false);
  }

  return (
    <>
      <button
        className="mobile-menu"
        onClick={() => setOpen(true)}
      >
        <Menu size={24} />
      </button>

      {open && (
        <div
          className="overlay"
          onClick={closeMenu}
        />
      )}

      <aside
        className={`sidebar ${open ? "open" : ""}`}
      >
        <div className="top">

          <div className="logo">
            VITA
          </div>

          <button
            className="close-btn"
            onClick={closeMenu}
          >
            <X size={22} />
          </button>

        </div>

        <nav>

          {role === "Admin" && (
            <>

              <NavLink
                to="/dashboard"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <LayoutDashboard size={18} />
                Dashboard
              </NavLink>

              <NavLink
                to="/pacientes"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <Users size={18} />
                Pacientes
              </NavLink>

              <NavLink
                to="/medicos"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <Stethoscope size={18} />
                Médicos
              </NavLink>

              <NavLink
                to="/consultas"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <CalendarDays size={18} />
                Consultas
              </NavLink>

              <NavLink
                to="/exames"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <FileText size={18} />
                Exames
              </NavLink>

              <NavLink
                to="/configuracoes"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <Settings size={18} />
                Configurações
              </NavLink>

            </>
          )}

          {role === "Medico" && (
            <>

              <NavLink
                to="/dashboard-medico"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <LayoutDashboard size={18} />
                Dashboard Médico
              </NavLink>

              <NavLink
                to="/agenda"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <CalendarDays size={18} />
                Agenda
              </NavLink>

              <NavLink
                to="/meus-pacientes"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <Users size={18} />
                Meus Pacientes
              </NavLink>

              <NavLink
                to="/minhas-consultas"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <CalendarDays size={18} />
                Minhas Consultas
              </NavLink>

              <NavLink
                to="/exames"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <FileText size={18} />
                Exames
              </NavLink>

              <NavLink
                to="/meu-perfil"
                className={({ isActive }) =>
                  isActive ? "active" : ""
                }
              >
                <UserCog size={18} />
                Meu Perfil
              </NavLink>


              <NavLink
                to="/ajuda"
                className={({ isActive }) =>
                  isActive
                    ? "sidebar-link active"
                    : "sidebar-link"
                }
              >
                <CircleHelp size={20} />

                <span>
                  Ajuda e Suporte
                </span>
              </NavLink>

            </>

            
          )}

          <button
            className="logout-btn"
            onClick={handleLogout}
          >
            <LogOut size={18} />
            Sair
          </button>

        </nav>
      </aside>
    </>
  );
}