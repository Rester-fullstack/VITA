import {
  useEffect,
  useState
} from "react";

import {
  motion
} from "framer-motion";

import api from "../../api/axios";

import "./Pacientes.css";

import CreatePacienteModal from"../../components/modals/CreatePacienteModal";

import EditPacienteModal from"../../components/modals/EditPacienteModal";

import toast from "react-hot-toast";

type Paciente = {
  id: number;
  nome: string;
  cpf: string;
  telefone: string;
};

export default function Pacientes(){


  const [pacientes, setPacientes] =
    useState<Paciente[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [search,setSearch] =
    useState("");

  const [modalOpen, setModalOpen] =
   useState(false);


  const [editModalOpen, setEditModalOpen] =
    useState(false);

  const [selectedPaciente, setSelectedPaciente] =
    useState<any>(null);




  async function loadPacientes(){

    try{

      const response =
        await api.get(
          "/pacientes"
        );

      setPacientes(
        response.data.data
      );

    }catch(error){

      console.log(error);

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {

    loadPacientes();

  }, []);

  if(loading){
    return (
      <div className="loading">
        Carregando pacientes...
      </div>
    );
  }


  const filteredPacientes =
    pacientes.filter((paciente) =>
      paciente.nome
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      paciente.cpf
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      paciente.telefone
        .toLowerCase()
        .includes(search.toLowerCase())
    );


  
  async function handleDelete(
    id:number
  ){

    const confirmDelete =
      confirm(
        "Deseja excluir este paciente?"
      );

    if(!confirmDelete)
      return;

    try{

      await api.delete(
        `/pacientes/${id}`
      );

      toast.success(
        "Paciente removido"
      );

      loadPacientes();

    }catch(error){

      toast.error(
        "Erro ao remover paciente"
      );

    }
  }


  return (
    <motion.div
      className="pacientes-page"
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

        <h1>
          Pacientes
        </h1>


        <button
          onClick={() =>
            setModalOpen(true)
          }
        >
          Novo Paciente
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

      <div className="pacientes-stats">

        <div className="paciente-stat-card">
          <span>Total de pacientes</span>
          <strong>{pacientes.length}</strong>
        </div>

        <div className="paciente-stat-card">
          <span>Resultado da busca</span>
          <strong>{filteredPacientes.length}</strong>
        </div>

      </div>



      <div className="table-container">

        <table>

          <thead>

            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>CPF</th>
              <th>Telefone</th>
              <th>Ações</th>
            </tr>

          </thead>

      
          <tbody>

            {filteredPacientes.length > 0 ? (

              filteredPacientes.map((paciente) => (

                <tr key={paciente.id}>

                  <td>
                    #{paciente.id}
                  </td>

                  <td>
                    {paciente.nome}
                  </td>

                  <td>
                    {paciente.cpf}
                  </td>

                  <td>
                    {paciente.telefone}
                  </td>

               
                <td>

                  <div className="actions-buttons">


                    <button
                      className="edit-btn"
                      onClick={() => {

                        setSelectedPaciente(
                          paciente
                        );

                        setEditModalOpen(true);

                      }}
                    >
                      Editar
                    </button>

                    <button
                      className="delete-btn"
                      onClick={() =>
                        handleDelete(
                          paciente.id
                        )
                      }
                    >
                      Excluir
                    </button>

                  </div>

                </td>

                </tr>

              ))

            ) : (

              <tr>

                <td
                  colSpan={5}
                  className="empty"
                >
                  Nenhum paciente encontrado
                </td>

              </tr>

            )}

          </tbody>



        </table>

      </div>

      
      {
        editModalOpen &&
        selectedPaciente && (

          <EditPacienteModal
            paciente={selectedPaciente}
            onClose={() =>
              setEditModalOpen(false)
            }
            onUpdated={loadPacientes}
          />

        )
      }



      { modalOpen && ( 
        <CreatePacienteModal 
        onClose={() => 
        setModalOpen(false) 
        } 
        onCreated={loadPacientes}
         /> 
         
       ) 
      }
      

    </motion.div>
  );
}
