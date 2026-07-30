import {
  useEffect,
  useState
} from "react";

import {
  useNavigate
} from "react-router-dom";

import api from "../../api/axios";

import "./MeusPacientes.css";

import CreatePacienteModal from
"../../components/modals/CreatePacienteModal";

export default function MeusPacientes(){

  const navigate = useNavigate();

  const [pacientes, setPacientes] =
    useState<any[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [search, setSearch] =
    useState("");

  const [openCreateModal, setOpenCreateModal] =
    useState(false);

  async function loadPacientes(){

    try{

      const response =
        await api.get(
          "/Consulta/minhas"
        );

      const consultas =
        response.data.data ?? [];

      const pacientesMap =
        new Map();

      consultas.forEach((consulta:any) => {

        const pacienteAtual =
          pacientesMap.get(
            consulta.pacienteId
          );

        const consultasPaciente =
          consultas.filter(
            (c:any) =>
              c.pacienteId ===
              consulta.pacienteId
          );

        const consultasOrdenadas =
          [...consultasPaciente].sort(
            (a:any,b:any) =>
              new Date(b.dataConsulta).getTime() -
              new Date(a.dataConsulta).getTime()
          );

        const proximaConsulta =
          consultasPaciente
            .filter((c:any) =>
              new Date(c.dataConsulta) >
              new Date()
            )
            .sort(
              (a:any,b:any) =>
                new Date(a.dataConsulta).getTime() -
                new Date(b.dataConsulta).getTime()
            )[0];

        if(!pacienteAtual){
          pacientesMap.set(
            consulta.pacienteId,
            {
              id: consulta.pacienteId,
              nome: consulta.pacienteNome,
              totalConsultas:
                consultasPaciente.length,
              ultimaConsulta:
                consultasOrdenadas[0],
              proximaConsulta:
                proximaConsulta || null
            }
          );
        }

      });

      setPacientes(
        Array.from(
          pacientesMap.values()
        )
      );

    }catch(error){

      console.error(error);

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {
    loadPacientes();
  }, []);

  const pacientesFiltrados =
    pacientes.filter(p =>
      (p.nome || "")
        .toLowerCase()
        .includes(
          search.toLowerCase()
        )
    );

  if(loading){
    return(
      <div className="loading">
        Carregando pacientes...
      </div>
    );
  }

  return(
    <div className="meus-pacientes-page">

      <div className="page-header">

        <div>
          <h1 className="title">
            Meus Pacientes
          </h1>

          <p className="subtitle">
            Pacientes vinculados às suas consultas
          </p>
        </div>

        <button
          className="new-btn"
          onClick={() =>
            setOpenCreateModal(true)
          }
        >
          + Novo Paciente
        </button>

      </div>

      <div className="search-box">
        <input
          type="text"
          placeholder="Buscar paciente..."
          value={search}
          onChange={(e) =>
            setSearch(e.target.value)
          }
        />
      </div>

      <div className="table-container">

        <table>

          <thead>
            <tr>
              <th>Paciente</th>
              <th>Total de consultas</th>
              <th>Última consulta</th>
              <th>Próxima consulta</th>
              <th>Ações</th>
            </tr>
          </thead>

          <tbody>

            {
              pacientesFiltrados.length === 0 && (
                <tr>
                  <td
                    colSpan={5}
                    className="empty-state"
                  >
                    Nenhum paciente encontrado
                  </td>
                </tr>
              )
            }

            {
              pacientesFiltrados.map(paciente => (

                <tr key={paciente.id}>

                  <td>
                    {paciente.nome}
                  </td>

                  <td>
                    {paciente.totalConsultas}
                  </td>

                  <td>
                    {
                      paciente.ultimaConsulta
                        ? new Date(
                            paciente.ultimaConsulta.dataConsulta
                          ).toLocaleString("pt-BR")
                        : "-"
                    }
                  </td>

                  <td>
                    {
                      paciente.proximaConsulta
                        ? new Date(
                            paciente.proximaConsulta.dataConsulta
                          ).toLocaleString("pt-BR")
                        : "Sem retorno"
                    }
                  </td>

                  <td className="actions">

                    <button
                      className="view-btn"
                      onClick={() =>
                        navigate(
                          `/paciente/${paciente.id}`
                        )
                      }
                    >
                      Prontuário
                    </button>

                    {
                      paciente.ultimaConsulta && (
                        <button
                          className="edit-btn"
                          onClick={() =>
                            navigate(
                              `/consulta/${paciente.ultimaConsulta.id}`
                            )
                          }
                        >
                          Última
                        </button>
                      )
                    }

                  </td>

                </tr>

              ))
            }

          </tbody>

        </table>

        {openCreateModal && (
          <CreatePacienteModal
            onClose={() =>
              setOpenCreateModal(false)
            }
            onCreated={loadPacientes}
          />
        )}

      </div>

    </div>
  );
}