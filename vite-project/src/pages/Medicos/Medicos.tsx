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


import CreateMedicoModal from
"../../components/modals/CreateMedicoModal";


import EditMedicoModal from
"../../components/modals/EditMedicoModal";


import "./Medicos.css";

type Medico = {
  id:number;
  nome:string;
  crm:string;
  especialidade:string;
  email:string;
};

export default function Medicos(){

  const [medicos, setMedicos] =
    useState<Medico[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [openCreateModal,
    setOpenCreateModal] =
    useState(false);
  
  const [openEditModal,
    setOpenEditModal] =
      useState(false);

  const [selectedMedico,
    setSelectedMedico] =
      useState<any>(null);

  const [search, setSearch] =
   useState("");



  async function loadMedicos(){

    try{

      const response =
        await api.get(
          "/Medicos"
        );

     setMedicos(
        response.data.data
      );

    }catch(error){

      toast.error(
        "Erro ao carregar médicos"
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
        "Deseja excluir este médico?"
      );

    if(!confirmDelete) return;

    try{

      await api.delete(
        `/Medicos/${id}`
      );

      toast.success(
        "Médico removido"
      );

      loadMedicos();

    }catch(error){

      toast.error(
        "Erro ao excluir médico"
      );

    }
  }

  useEffect(() => {
    loadMedicos();
  }, []);

  if(loading){
    return (
      <div className="loading">
        Carregando médicos...
      </div>
    );
  }

  const filteredMedicos =
    medicos.filter(medico =>
      medico.nome
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      medico.crm
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      medico.especialidade
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      medico.email
        .toLowerCase()
        .includes(search.toLowerCase())
    );

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
            Médicos
          </h1>

          <p className="subtitle">
            Gestão de médicos
          </p>

        </div>


        <button
          className="new-btn"
          onClick={() =>
            setOpenCreateModal(true)
          }
        >
          Novo Médico
        </button>



      </div>

      <div className="medicos-stats">

        <div className="medico-stat-card">
          <span>Total de médicos</span>
          <strong>{medicos.length}</strong>
        </div>

        <div className="medico-stat-card">
          <span>Resultado da busca</span>
          <strong>{filteredMedicos.length}</strong>
        </div>

      </div>

      <div className="search-box">
        <input
          type="text"
          placeholder="Buscar por nome, CRM, especialidade ou email..."
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
              <th>CRM</th>
              <th>Especialidade</th>
              <th>Email</th>
              <th>Ações</th>
            </tr>

          </thead>

          <tbody>

            {
              filteredMedicos.map((medico) => (

                <tr key={medico.id}>

                  <td>
                    {medico.id}
                  </td>

                  <td>
                    {medico.nome}
                  </td>

                  <td>
                    {medico.crm}
                  </td>

                  <td>
                    {medico.especialidade}
                  </td>

                  <td>
                    {medico.email}
                  </td>

                  <td className="actions">

           
                    <button
                      className="edit-btn"
                      onClick={() =>
                        {
                          setSelectedMedico(medico);
                          setOpenEditModal(true);
                        }
                      }
                    >
                      Editar
                    </button>



                    <button
                      className="delete-btn"
                      onClick={() =>
                        handleDelete(
                          medico.id
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

      <CreateMedicoModal
        open={openCreateModal}
        onClose={() =>
          setOpenCreateModal(false)
        }
        onSuccess={loadMedicos}
      />

      <EditMedicoModal
        open={openEditModal}
        onClose={() =>
          setOpenEditModal(false)
        }
        onSuccess={loadMedicos}
        medico={selectedMedico}
      />


      

    </motion.div>
  );

}