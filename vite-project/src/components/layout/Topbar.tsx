import { useEffect, useRef, useState } from "react";
import { Bell } from "lucide-react";

import api from "../../api/axios";

import "./Topbar.css";

type Notificacao = {
  id: number;
  titulo: string;
  descricao: string;
  icone: string;
  cor: string;
  dataHora: string;
};

export default function Topbar() {
  const user =
    JSON.parse(localStorage.getItem("user") || "{}");

  const nome = user.nome;
  const role = user.role;
  const especialidade = user.especialidade;
  const crm = user.crm;

  const primeiroNome =
    (nome || "Usuário").trim().split(" ")[0];

  const letra =
    primeiroNome.charAt(0).toUpperCase();

  const [open, setOpen] =
    useState(false);

  const [notificacoes, setNotificacoes] =
    useState<Notificacao[]>([]);

  const dropdownRef =
    useRef<HTMLDivElement | null>(null);

  async function loadNotificacoes() {
    try {
      const response =
        await api.get("/Notificacao");

      setNotificacoes(
        response.data.data ?? []
      );

    } catch (error) {
      console.error(error);
    }
  }

  useEffect(() => {
    loadNotificacoes();
  }, []);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(
          event.target as Node
        )
      ) {
        setOpen(false);
      }
    }

    document.addEventListener(
      "mousedown",
      handleClickOutside
    );

    return () =>
      document.removeEventListener(
        "mousedown",
        handleClickOutside
      );
  }, []);

  return (
    <header className="topbar">
      <div>
        Bem-vinda ao Vita
      </div>

      <div className="topbar-right">

        <div
          className="notification-wrapper"
          ref={dropdownRef}
        >
          <button
            className="notification-btn"
            onClick={() =>
              setOpen(!open)
            }
          >
            <Bell size={20} />

            {
              notificacoes.length > 0 && (
                <span className="notification-badge">
                  {notificacoes.length}
                </span>
              )
            }
          </button>

          {
            open && (
              <div className="notification-dropdown">
                <div className="notification-header">
                  <strong>
                    Notificações
                  </strong>

                  <span>
                    {notificacoes.length}
                  </span>
                </div>

                <div className="notification-list">
                  {
                    notificacoes.length === 0 ? (
                      <p className="notification-empty">
                        Nenhuma notificação.
                      </p>
                    ) : (
                      notificacoes.map(item => (
                        <div
                          key={item.id}
                          className="notification-item"
                        >
                          <div
                            className="notification-icon"
                            style={{
                              background: `${item.cor || "#2563EB"}22`,
                              color: item.cor || "#2563EB"
                            }}
                          >
                            {item.icone || "🔔"}
                          </div>

                          <div>
                            <strong>
                              {item.titulo}
                            </strong>

                            <p>
                              {item.descricao}
                            </p>

                            <small>
                              {
                                new Date(
                                  item.dataHora
                                ).toLocaleString("pt-BR")
                              }
                            </small>
                          </div>
                        </div>
                      ))
                    )
                  }
                </div>
              </div>
            )
          }
        </div>

        <div className="user-info">
          <div className="user-text">
            <span className="user-name">
              {nome || "Usuário"}
            </span>

            <span className="user-role">
              {
                role === "Medico"
                  ? `${especialidade || "Médico"} • CRM ${crm || ""}`
                  : role
              }
            </span>
          </div>

          <div className="profile">
            {letra}
          </div>
        </div>

      </div>
    </header>
  );
}