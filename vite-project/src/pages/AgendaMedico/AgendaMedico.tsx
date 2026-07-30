import {
  useEffect,
  useState
} from "react";

import {
  useNavigate
} from "react-router-dom";

import api from "../../api/axios";

import "./AgendaMedico.css";

export default function AgendaMedico(){

  const navigate = useNavigate();

  const [consultas, setConsultas] =
    useState<any[]>([]);

  const [loading, setLoading] =
    useState(true);

  async function loadAgenda(){

    try{

      const response =
        await api.get("/Consulta/minhas");

      const lista =
        response.data.data ?? [];

      const ordenadas =
        [...lista].sort(
          (a,b) =>
            new Date(a.dataConsulta).getTime() -
            new Date(b.dataConsulta).getTime()
        );

      setConsultas(ordenadas);

    }catch(error){

      console.error(error);

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {
    loadAgenda();
  }, []);

  if(loading){
    return(
      <div className="loading">
        Carregando agenda...
      </div>
    );
  }

  const hoje =
    new Date().toDateString();

  const consultasHoje =
    consultas.filter(c =>
      new Date(c.dataConsulta)
        .toDateString() === hoje
    );

  const proximas =
    consultas.filter(c =>

      new Date(c.dataConsulta) > new Date()

      &&

      c.status === "Confirmada"

    );

  const finalizadas =
    consultas.filter(
      c => c.status === "Finalizada"
    );

  const canceladas =
    consultas.filter(
      c => c.status === "Cancelada"
    );

  const consultasAtivas =
    consultas.filter(c =>
      c.status === "Agendada"
      ||
      c.status === "Confirmada"
    ).length;

  return(
    <div className="agenda-medico-page">

      <div className="page-header">
        <div>
          <h1 className="title">
            Agenda Médica
          </h1>

          <p className="subtitle">
            Consultas e retornos vinculados ao médico
          </p>
        </div>
      </div>

      <div className="agenda-grid">

        <div className="agenda-card destaque">
          <span>Consultas hoje</span>
          <strong>{consultasHoje.length}</strong>
        </div>

        <div className="agenda-card">
          <span>Próximas consultas</span>
          <strong>{proximas.length}</strong>
        </div>

        <div className="agenda-card">
          <span>Consultas Ativas</span>
          <strong>{consultasAtivas}</strong>
        </div>


      </div>

      <div className="agenda-section">

        <h2>Hoje</h2>

        {
          consultasHoje.length === 0 ? (
            <p className="empty">
              Nenhuma consulta para hoje.
            </p>
          ) : (
            consultasHoje.map(consulta => (
              <div
                key={consulta.id}
                className="agenda-item"
              >
                <div>
                  <strong>
                    {consulta.pacienteNome}
                  </strong>

                  <span>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleTimeString(
                        "pt-BR",
                        {
                          hour:"2-digit",
                          minute:"2-digit"
                        }
                      )
                    }
                  </span>

                  <small>
                    {consulta.status}
                  </small>
                </div>

                <button
                  onClick={() =>
                    navigate(`/consulta/${consulta.id}`)
                  }
                >
                  Abrir
                </button>
              </div>
            ))
          )
        }

      </div>

      <div className="agenda-section">

        <h2>Próximas Consultas</h2>

        {
          proximas.length === 0 ? (
            <p className="empty">
              Nenhuma consulta futura encontrada.
            </p>
          ) : (
            proximas.map(consulta => (
              <div
                key={consulta.id}
                className="agenda-item"
              >
                <div>
                  <strong>
                    {consulta.pacienteNome}
                  </strong>

                  <span>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleString("pt-BR")
                    }
                  </span>

                  <small>
                    {consulta.status}
                  </small>
                </div>

                <button
                  onClick={() =>
                    navigate(`/consulta/${consulta.id}`)
                  }
                >
                  Abrir
                </button>
              </div>
            ))
          )
        }

      </div>

      <div className="agenda-section">

        <h2>
          ✅ Finalizadas
        </h2>

        {
          finalizadas.length === 0 ? (
            <p className="empty">
              Nenhuma consulta finalizada.
            </p>
          ) : (
            finalizadas.map(consulta => (
              <div
                key={consulta.id}
                className="agenda-item"
              >
                <div>
                  <strong>
                    {consulta.pacienteNome}
                  </strong>

                  <span>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleString("pt-BR")
                    }
                  </span>
                </div>
              </div>
            ))
          )
        }

      </div>

      <div className="agenda-section">

        <h2>
          ❌ Canceladas
        </h2>

        {
          canceladas.length === 0 ? (
            <p className="empty">
              Nenhuma consulta cancelada.
            </p>
          ) : (
            canceladas.map(consulta => (
              <div
                key={consulta.id}
                className="agenda-item"
              >
                <div>
                  <strong>
                    {consulta.pacienteNome}
                  </strong>

                  <span>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleString("pt-BR")
                    }
                  </span>
                </div>
              </div>
            ))
          )
        }

      </div>

    </div>
  );
}