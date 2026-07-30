import {
  motion
} from "framer-motion";

import {
  useEffect,
  useState
} from "react";

import toast from
"react-hot-toast";

import api from
"../../api/axios";

import "./Consultas.css";

import EditConsultaModal from
"../../components/modals/EditConsultaModal";

import CreateConsultaModal from
"../../components/modals/CreateConsultaModal";

import ViewConsultaModal from
"../../components/modals/ViewConsultaModal";

type Consulta = {
  id:number;
  dataConsulta:string;
  status:string;
  observacoes:string;
  pacienteNome:string;
  medicoNome:string;
  podeExcluir:boolean;
};

export default function Consultas(){

  const [consultas, setConsultas] =
    useState<Consulta[]>([]);

  const [statusFilter,
    setStatusFilter] =
      useState("Todos");

  const [search,
    setSearch] =
      useState("");

  const [loading, setLoading] =
    useState(true);

  const [openCreateModal,
    setOpenCreateModal] =
      useState(false);

  const [openEditModal,
    setOpenEditModal] =
      useState(false);

  const [openViewModal,
    setOpenViewModal] =
      useState(false);

  const [selectedConsulta,
    setSelectedConsulta] =
      useState<any>(null);

  const [menuOpenId,
    setMenuOpenId] =
      useState<number | null>(null);

  async function loadConsultas(){

    try{

      const response =
        await api.get(
          "/Consulta"
        );

      setConsultas(
        response.data.data ?? []
        
      );

    }catch(error){

      toast.error(
        "Erro ao carregar consultas"
      );

    }finally{

      setLoading(false);

    }
  }

  async function handleDelete(
    id:number
  ){

    const confirmDelete =
      window.confirm(
        "Deseja excluir esta consulta?"
      );

    if(!confirmDelete) return;

    try{

      await api.delete(
        `/Consulta/${id}`
      );

      toast.success(
        "Consulta removida"
      );

      loadConsultas();

    }catch(error){

      toast.error(
        "Erro ao excluir consulta"
      );

    }
  }

  useEffect(() => {
    loadConsultas();
  }, []);

  const filteredConsultas =
    consultas.filter((consulta) => {

      const matchesStatus =
        statusFilter === "Todos"
        || consulta.status ===
        statusFilter;

      const matchesSearch =

        consulta.pacienteNome
          .toLowerCase()
          .includes(
            search.toLowerCase()
          )

        ||

        consulta.medicoNome
          .toLowerCase()
          .includes(
            search.toLowerCase()
          );

      return (
        matchesStatus &&
        matchesSearch
      );

    });

  if(loading){
    return (
      <div className="loading">
        Carregando consultas...
      </div>
    );
  }

  const totalConsultas =
   consultas.length;

  const consultasHoje =
    consultas.filter(c =>
      new Date(c.dataConsulta)
        .toDateString() ===
      new Date().toDateString()
    ).length;

  const consultasAgendadas =
    consultas.filter(c =>
      c.status === "Agendada"
    ).length;

  const consultasFinalizadas =
    consultas.filter(c =>
      c.status === "Finalizada"
    ).length;

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
            Consultas
          </h1>

          <p className="subtitle">
            Gestão de consultas
          </p>

        </div>

        <button
          className="new-btn"
          onClick={() =>
            setOpenCreateModal(true)
          }
        >
          Nova Consulta
        </button>

      </div>

      <div className="consultas-stats">

        <div className="consulta-stat-card">
          <span>Total de consultas</span>
          <strong>{totalConsultas}</strong>
        </div>

        <div className="consulta-stat-card">
          <span>Consultas hoje</span>
          <strong>{consultasHoje}</strong>
        </div>

        <div className="consulta-stat-card">
          <span>Agendadas</span>
          <strong>{consultasAgendadas}</strong>
        </div>

        <div className="consulta-stat-card">
          <span>Finalizadas</span>
          <strong>{consultasFinalizadas}</strong>
        </div>

      </div>

      <div className="filters">

        <input
          type="text"
          placeholder="Buscar paciente ou médico"
          value={search}
          onChange={(e) =>
            setSearch(
              e.target.value
            )
          }
        />

        <select
          value={statusFilter}
          onChange={(e) =>
            setStatusFilter(
              e.target.value
            )
          }
        >

          <option value="Todos">
            Todos
          </option>

          <option value="Agendada">
            Agendada
          </option>

          <option value="Confirmada">
            Confirmada
          </option>

          <option value="Cancelada">
            Cancelada
          </option>

          <option value="Finalizada">
            Finalizada
          </option>

        </select>

      </div>

      <div className="table-container">

        <table>

          <thead>

            <tr>
              <th>ID</th>
              <th>Paciente</th>
              <th>Médico</th>
              <th>Data</th>
              <th>Status</th>
              <th>Observações</th>
              <th>Ações</th>
            </tr>

          </thead>

          <tbody>

            {
              filteredConsultas.map((consulta) => (

                <tr key={consulta.id}>

                  <td>
                    {consulta.id}
                  </td>

                  <td>
                    {consulta.pacienteNome}
                  </td>

                  <td>
                    {consulta.medicoNome}
                  </td>

                  <td>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleString()
                    }
                  </td>

                  <td>

                    <span
                      className={`status ${
                        consulta.status
                          ?.trim()
                          .toLowerCase()
                      }`}
                    >
                      {consulta.status || "Sem status"}
                    </span>
                  </td>

                  <td>
                    {consulta.observacoes}
                  </td>

                  <td className="actions">

                    <button
                      className="edit-btn"
                      onClick={() => {

                        setSelectedConsulta(consulta);

                        setOpenEditModal(true);

                      }}
                    >
                      Editar
                    </button>

                    <div className="dropdown">

                      <button
                        className="more-btn"
                        onClick={() =>
                          setMenuOpenId(
                            menuOpenId === consulta.id
                              ? null
                              : consulta.id
                          )
                        }
                      >
                        ⋮
                      </button>

                      {
                        menuOpenId === consulta.id && (

                          <div className="dropdown-menu">

                           <button
                              className="dropdown-item"
                              onClick={() => {

                                setSelectedConsulta(consulta);

                                setOpenViewModal(true);

                                setMenuOpenId(null);

                              }}
                            >
                              👁 Visualizar
                            </button>

                            {
                              consulta.podeExcluir
                              ?

                              <button
                                className="dropdown-danger"
                                onClick={() =>
                                  handleDelete(
                                    consulta.id
                                  )
                                }
                              >
                                🗑 Excluir permanentemente
                              </button>

                              :

                              <button
                                className="dropdown-disabled"
                                disabled
                              >
                                🔒 Excluir permanentemente
                              </button>

                            }

                          </div>

                        )
                      }

                    </div>

                  </td>

                </tr>

              ))
            }

          </tbody>

        </table>

      </div>

      <CreateConsultaModal
        open={openCreateModal}
        onClose={() =>
          setOpenCreateModal(false)
        }
        onSuccess={loadConsultas}
      />

      <EditConsultaModal
        open={openEditModal}
        onClose={() =>
          setOpenEditModal(false)
        }
        onSuccess={loadConsultas}
        consulta={selectedConsulta}
      />

      <ViewConsultaModal
        open={openViewModal}
        onClose={() =>
            setOpenViewModal(false)
        }
        consulta={selectedConsulta}
      />

    </motion.div>
  );
}

