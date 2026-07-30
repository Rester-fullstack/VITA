import {
  useEffect,
  useState
} from "react";

import {
  motion
} from "framer-motion";

import {
  useNavigate
} from "react-router-dom";

import api from "../../api/axios";

import "./MinhasConsultas.css";

import { toast } from "react-hot-toast";

export default function MinhasConsultas(){

  const navigate = useNavigate();

  const [consultas, setConsultas] =
    useState<any[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [search, setSearch] =
    useState("");

  async function loadConsultas(){

    try{

      const response =
        await api.get(
          "/Consulta/minhas"
        );

      setConsultas(
        response.data.data
      );

    }catch(error){

      console.error(error);

    }finally{

      setLoading(false);

    }
  }

  async function cancelarConsulta(id:number){

    const confirmar =
      window.confirm(
        "Deseja cancelar esta consulta?"
      );

    if(!confirmar) return;

    try{

      const consulta =
        consultas.find(
          c => c.id === id
        );

      await api.put(
        `/Consulta/${id}`,
        {
          ...consulta,
          status:"Cancelada"
        }
      );

      toast.success(
        "Consulta cancelada."
      );

      loadConsultas();

    }catch(error){

      console.error(error);

      toast.error(
        "Erro ao cancelar consulta."
      );

    }
  }

  useEffect(() => {

    loadConsultas();

  }, []);

  const filtered =
    consultas.filter(c =>
      c.pacienteNome
        ?.toLowerCase()
        .includes(
          search.toLowerCase()
        )
    );

  const consultasOrdenadas = [...filtered].sort(
    (a, b) =>
      new Date(b.dataConsulta).getTime() -
      new Date(a.dataConsulta).getTime()
  );

  const consultasHoje =
    consultas.filter(c =>
      new Date(
        c.dataConsulta
      ).toDateString() ===
      new Date().toDateString()
    ).length;

  const confirmadas =
    consultas.filter(c =>
      c.status === "Confirmada"
    ).length;

  const agendadas =
    consultas.filter(c =>
      c.status === "Agendada"
    ).length;

  const hoje =
  new Date();

  const consultasHojeLista =
    consultas
      .filter(c =>
        new Date(c.dataConsulta)
          .toDateString() ===
        hoje.toDateString()
      )
      .sort(
        (a,b) =>
          new Date(a.dataConsulta).getTime() -
          new Date(b.dataConsulta).getTime()
      );

  const proximasConsultas =
    consultas
      .filter(c =>
        new Date(c.dataConsulta) > new Date()
      )
      .sort(
        (a,b) =>
          new Date(a.dataConsulta).getTime() -
          new Date(b.dataConsulta).getTime()
      )
      .slice(0,5);
    
  if(loading){

    return (
      <div className="loading">
        Carregando consultas...
      </div>
    );
  }

  return(

    <motion.div
      initial={{
        opacity:0,
        y:20
      }}
      animate={{
        opacity:1,
        y:0
      }}
    >

      <div className="page-header">

        <div>

          <h1 className="title">
            Minhas Consultas
          </h1>

          <p className="subtitle">
            Consultas vinculadas ao médico
          </p>

        </div>

      </div>

      <div className="stats-grid">

        <div className="stat-card">

          <div className="stat-icon blue">
            📅
          </div>

          <div>
            <span>
              Hoje
            </span>

            <h2>
              {consultasHoje}
            </h2>
          </div>

        </div>

        <div className="stat-card">

          <div className="stat-icon orange">
            👥
          </div>

          <div>

            <span>
              Total
            </span>

            <h2>
              {consultas.length}
            </h2>

          </div>

        </div>

        <div className="stat-card">

          <div className="stat-icon green">
            ✅
          </div>

          <div>
            <span>
              Confirmadas
            </span>

            <h2>
              {confirmadas}
            </h2>
          </div>

        </div>

        <div className="stat-card">

          <div className="stat-icon purple">
            🕒
          </div>

          <div>
            <span>
              Agendadas
            </span>

            <h2>
              {agendadas}
            </h2>
          </div>

        </div>

      </div>

      <div className="agenda-grid">

        <div className="agenda-card">

          <h2>
            📅 Agenda de Hoje
          </h2>

          {
            consultasHojeLista.length === 0 ? (

              <p className="agenda-empty">
                Nenhuma consulta para hoje.
              </p>

            ) : (

              consultasHojeLista.map(consulta => (

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

                  </div>
                  <div className="agenda-actions">

                    <button
                      onClick={() =>
                        navigate(`/consulta/${consulta.id}`)
                      }
                    >
                      Abrir
                    </button>

                    {
                      consulta.status === "Agendada" && (

                        <button
                          className="cancel-btn"
                          onClick={() =>
                            cancelarConsulta(consulta.id)
                          }
                        >
                          Cancelar
                        </button>

                      )
                    }

                  </div>

                </div>

              ))

            )
          }

        </div>

        <div className="agenda-card">

          <h2>
            🕒 Próximas Consultas
          </h2>

          {
            proximasConsultas.length === 0 ? (

              <p className="agenda-empty">
                Nenhuma consulta futura.
              </p>

            ) : (

              proximasConsultas.map(consulta => (

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

                  <div className="agenda-actions">

                    <button
                      onClick={() =>
                        navigate(`/consulta/${consulta.id}`)
                      }
                    >
                      Abrir
                    </button>

                    {
                      consulta.status === "Agendada" && (

                        <button
                          className="cancel-btn"
                          onClick={() =>
                            cancelarConsulta(
                              consulta.id
                            )
                          }
                        >
                          Cancelar
                        </button>

                      )
                    }

                  </div>

                </div>

              ))

            )
          }

        </div>

      </div>

      <div className="table-card">

        <div className="table-header">

          <input
            type="text"
            placeholder="Buscar paciente..."
            value={search}
            onChange={(e)=>
              setSearch(
                e.target.value
              )
            }
          />

        </div>

        <div className="table-container">

          <table>

            <thead>

              <tr>

                <th>Paciente</th>
                <th>Data</th>
                <th>Status</th>
                <th>Observações</th>
                <th>Ações</th>

              </tr>

            </thead>

            <tbody>

            {
              consultasOrdenadas.length === 0 && (

                <tr>

                  <td
                    colSpan={5}
                    className="empty-state"
                  >
                    Nenhuma consulta encontrada
                  </td>

                </tr>

              )
            }

            {
              consultasOrdenadas.map(
                consulta => (

                  <tr key={consulta.id}>

                    <td>
                      {consulta.pacienteNome}
                    </td>

                    <td>
                      {
                        new Date(
                          consulta.dataConsulta
                        ).toLocaleString("pt-BR")
                      }
                    </td>

                    <td>

                      <span
                        className={`status ${
                          consulta.status
                            .toLowerCase()
                        }`}
                      >
                        {consulta.status}
                      </span>

                    </td>

                    <td>
                      {consulta.observacoes}
                    </td>

                    <td>

                      <div className="table-actions">

                        <button
                          className="open-btn"
                          onClick={() =>
                            navigate(`/consulta/${consulta.id}`)
                          }
                        >
                          Abrir
                        </button>

                        {
                          consulta.status === "Agendada" && (

                            <button
                              className="cancel-btn"
                              onClick={() =>
                                cancelarConsulta(consulta.id)
                              }
                            >
                              Cancelar
                            </button>

                          )
                        }

                      </div>

                    </td>

                  </tr>

                )
              )
            }

          </tbody>

          </table>

        </div>

      </div>

    </motion.div>
  );
}