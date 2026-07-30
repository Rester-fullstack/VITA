import {
  motion
} from "framer-motion";

import {
  useState
} from "react";

import toast from
"react-hot-toast";

import api from "../../api/axios";

import "./PacienteModal.css";

type Paciente = {
  id:number;
  nome:string;
  cpf:string;
  telefone:string;
  endereco:string;
};

type Props = {
  paciente:Paciente;
  onClose:() => void;
  onUpdated:() => void;
};

export default function EditPacienteModal({
  paciente,
  onClose,
  onUpdated
}:Props){

  const [nome, setNome] =
    useState(paciente.nome);

  const [telefone, setTelefone] =
    useState(paciente.telefone);

  const [endereco, setEndereco] =
    useState(paciente.endereco);

  const [loading, setLoading] =
    useState(false);

  async function handleUpdate(
    e:React.FormEvent
  ){

    e.preventDefault();

    try{

      setLoading(true);

      await api.put(
        `/pacientes/${paciente.id}`,
        {
          nome,
          telefone,
          endereco,
          dataNascimento:
            "2000-01-01"
        }
      );

      toast.success(
        "Paciente atualizado"
      );

      onUpdated();

      onClose();

    }catch(error){

      toast.error(
        "Erro ao atualizar"
      );

    }finally{

      setLoading(false);

    }
  }

  return(
    <div className="modal-overlay">

      <motion.div
        className="modal"
        initial={{
          opacity:0,
          scale:0.9
        }}
        animate={{
          opacity:1,
          scale:1
        }}
      >

        <h2>
          Editar Paciente
        </h2>

        <form
          onSubmit={handleUpdate}
        >

          <input
            type="text"
            value={nome}
            onChange={(e)=>
              setNome(e.target.value)
            }
          />

          <input
            type="text"
            value={telefone}
            onChange={(e)=>
              setTelefone(e.target.value)
            }
          />

          <input
            type="text"
            value={endereco}
            onChange={(e)=>
              setEndereco(e.target.value)
            }
          />

          <div className="actions">

            <button
              type="button"
              className="cancel"
              onClick={onClose}
            >
              Cancelar
            </button>

            <button type="submit">

              {
                loading
                  ? "Salvando..."
                  : "Salvar"
              }

            </button>

          </div>

        </form>

      </motion.div>

    </div>
  );
}
