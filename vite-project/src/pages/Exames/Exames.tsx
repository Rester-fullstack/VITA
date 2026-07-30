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

import "./Exames.css";
import EditExameModal from "../../components/modals/EditExameModal";
import CreateExameModal from "../../components/modals/CreateExameModal";

type Exame = {
  id:number;
  nome:string;
  resultado:string;
  dataExame:string;
  pacienteNome:string;
  consultaId:number;
  pdfUrl:string;
};

export default function Exames(){

  const [exames,
    setExames] =
      useState<Exame[]>([]);

  const [loading,
    setLoading] =
      useState(true);

  const [openCreateModal,
    setOpenCreateModal] =
      useState(false);

  const [openEditModal,
    setOpenEditModal] =
      useState(false);

  const [selectedExame,
    setSelectedExame] =
      useState<any>(null);

  const [search, setSearch] =
   useState("");

  const user =
  JSON.parse(
    localStorage.getItem("user") || "{}"
  );

  const role = user.role;


  async function loadExames(){

    try{

      const examesResponse =
        await api.get("/Exame");

      if(role === "Admin"){

        setExames(
          examesResponse.data.data ?? []
        );

        return;
      }

      const consultasResponse =
        await api.get("/Consulta/minhas");

      const consultasMedico =
        consultasResponse.data.data ?? [];

      const idsConsultasMedico =
        consultasMedico.map(
          (c:any) => c.id
        );

      const examesFiltrados =
        (examesResponse.data.data ?? [])
          .filter((e:any) =>
            idsConsultasMedico.includes(e.consultaId)
          );

      setExames(examesFiltrados);

    }catch(error){

      toast.error(
        "Erro ao carregar exames"
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
        "Deseja excluir este exame?"
      );

    if(!confirmDelete) return;

    try{

      await api.delete(
        `/Exame/${id}`
      );

      toast.success(
        "Exame removido"
      );

      loadExames();

    }catch(error){

      toast.error(
        "Erro ao excluir exame"
      );

    }
  }

  useEffect(() => {
    loadExames();
  }, []);

  if(loading){

    return(
      <div className="loading">
        Carregando exames...
      </div>
    );
  }

  const examesFiltrados =
    exames.filter(exame =>
      (exame.nome || "")
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      (exame.pacienteNome || "")
        .toLowerCase()
        .includes(search.toLowerCase())
    );

  const examesComPdf =
    exames.filter(e => e.pdfUrl).length;



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

      <div className="exames-page">

        <div className="page-header">

          <div>
            <h1 className="title">
              Exames
            </h1>

            <p className="subtitle">
              Gestão de exames vinculados às consultas
            </p>
          </div>

          <button
            className="new-btn"
            onClick={() =>
              setOpenCreateModal(true)
            }
          >
            + Novo Exame
          </button>

        </div>

        <div className="exames-stats">

          <div className="exam-stat-card">
            <span>Total de exames</span>
            <strong>{exames.length}</strong>
          </div>

          <div className="exam-stat-card">
            <span>Com PDF</span>
            <strong>{examesComPdf}</strong>
          </div>

        </div>

        <div className="search-box">
          <input
            type="text"
            placeholder="Buscar por exame ou paciente..."
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
                <th>ID</th>
                <th>Nome</th>
                <th>Paciente</th>
                <th>Consulta</th>
                <th>Resultado</th>
                <th>Data</th>
                <th>Ações</th>
              </tr>

            </thead>

            <tbody>

              {
                examesFiltrados.length === 0 && (
                  <tr>
                    <td
                      colSpan={7}
                      className="empty-state"
                    >
                      Nenhum exame encontrado
                    </td>
                  </tr>
                )
              }

              {
                examesFiltrados.map((exame) => (

                  <tr key={exame.id}>

                    <td>
                      {exame.id}
                    </td>

                    <td>
                      {exame.nome}
                    </td>

                    <td>
                      {exame.pacienteNome}
                    </td>

                    <td>
                      #{exame.consultaId}
                    </td>

                    <td>
                      {exame.resultado}
                    </td>

                    <td>
                      {
                        new Date(
                          exame.dataExame
                        ).toLocaleDateString()
                      }
                    </td>

                    <td className="actions">

                      {
                        exame.pdfUrl && (

                          <a
                          href={encodeURI(
                              `http://localhost:5182${exame.pdfUrl}`
                            )}
                            target="_blank"
                            rel="noreferrer"
                            className="view-btn"
                          >
                            Ver PDF
                          </a>

                        )
                      }

                      <button
                        className="edit-btn"
                        onClick={() => {

                          setSelectedExame(
                            exame
                          );

                          setOpenEditModal(true);

                        }}
                      >
                        Editar
                      </button>

                      <button
                        className="delete-btn"
                        onClick={() =>
                          handleDelete(
                            exame.id
                          )
                        }
                      >
                        Excluir
                      </button>

                    </td>

                  </tr>

                ))
              }

            </tbody>

          </table>

        </div>


        <CreateExameModal
          open={openCreateModal}
          onClose={() =>
            setOpenCreateModal(false)
          }
          onSuccess={loadExames}
        />

        <EditExameModal
          open={openEditModal}
          onClose={() =>
            setOpenEditModal(false)
          }
          onSuccess={loadExames}
          exame={selectedExame}
        />

      
    </div>
    </motion.div>
  );
}