import {
  motion
} from "framer-motion";

import {
  useState
} from "react";

import api from "../../api/axios";

import "./PacienteModal.css";

import toast from
"react-hot-toast";

type Props = {
  onClose: () => void;
  onCreated: () => void;
};

export default function CreatePacienteModal({
  onClose,
  onCreated
}: Props){

  const [nome, setNome] =
    useState("");

  const [cpf, setCpf] =
    useState("");

  const [telefone, setTelefone] =
    useState("");

  const [endereco, setEndereco] =
    useState("");

  const [dataNascimento, setDataNascimento] =
    useState("");

  const [dataConsulta, setDataConsulta] =
    useState("");

  const [observacoes, setObservacoes] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  async function handleCreate(
    e: React.FormEvent
  ){

    e.preventDefault();

    if(!dataConsulta){
      toast.error(
        "Agende a primeira consulta"
      );
      return;
    }

    try{

      setLoading(true);

      const pacienteResponse =
        await api.post(
          "/pacientes",
          {
            nome,
            cpf,
            telefone,
            endereco,
            dataNascimento:
              dataNascimento || "2000-01-01"
          }
        );

      const pacienteCriado =
        pacienteResponse.data.data;

      await api.post(
        "/Consulta/minha",
        {
          pacienteId:
            pacienteCriado.id,

          dataConsulta,

          observacoes:
            observacoes ||
            "Primeira consulta"
        }
      );

      toast.success(
        "Paciente cadastrado e primeira consulta agendada"
      );

      onCreated();

      onClose();

    }catch(error){

      console.log(error);

      toast.error(
        "Erro ao cadastrar paciente e consulta"
      );

    }finally{

      setLoading(false);

    }
  }

  return (
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
          Novo Paciente
        </h2>

        <p className="modal-subtitle">
          Cadastre o paciente e agende a primeira consulta.
        </p>

        <form onSubmit={handleCreate}>

          <input
            type="text"
            placeholder="Nome"
            value={nome}
            onChange={(e) =>
              setNome(e.target.value)
            }
            required
          />

          <input
            type="text"
            placeholder="CPF"
            value={cpf}
            onChange={(e) =>
              setCpf(e.target.value)
            }
            required
          />

          <input
            type="text"
            placeholder="Telefone"
            value={telefone}
            onChange={(e) =>
              setTelefone(e.target.value)
            }
            required
          />

          <input
            type="date"
            value={dataNascimento}
            onChange={(e) =>
              setDataNascimento(e.target.value)
            }
          />

          <input
            type="text"
            placeholder="Endereço"
            value={endereco}
            onChange={(e) =>
              setEndereco(e.target.value)
            }
            required
          />

          <hr />

          <h3>
            Primeira Consulta
          </h3>

          <input
            type="datetime-local"
            value={dataConsulta}
            onChange={(e) =>
              setDataConsulta(e.target.value)
            }
            required
          />

          <textarea
            placeholder="Observações da primeira consulta..."
            value={observacoes}
            onChange={(e) =>
              setObservacoes(e.target.value)
            }
          />

          <div className="actions">

            <button
              type="button"
              onClick={onClose}
              className="cancel"
            >
              Cancelar
            </button>

            <button type="submit">
              {
                loading
                  ? "Salvando..."
                  : "Salvar e Agendar"
              }
            </button>

          </div>

        </form>

      </motion.div>

    </div>
  );
}