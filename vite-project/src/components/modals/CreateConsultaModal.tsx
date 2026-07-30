import {
  useEffect,
  useState
} from "react";

import toast from
"react-hot-toast";

import api from
"../../api/axios";

import "./ConsultaModal.css";

type Props = {
  open:boolean;
  onClose:() => void;
  onSuccess:() => void;
};

type Paciente = {
  id:number;
  nome:string;
};

type Medico = {
  id:number;
  nome:string;
};

export default function CreateConsultaModal({
  open,
  onClose,
  onSuccess
}:Props){

  const [pacientes,
    setPacientes] =
      useState<Paciente[]>([]);

  const [medicos,
    setMedicos] =
      useState<Medico[]>([]);

  const [pacienteId,
    setPacienteId] =
      useState("");

  const [medicoId,
    setMedicoId] =
      useState("");

  const [dataConsulta,
    setDataConsulta] =
      useState("");

  const [status,
    setStatus] =
      useState("Agendada");

  const [observacoes,
    setObservacoes] =
      useState("");

  const [loading,
    setLoading] =
      useState(false);

  async function loadData(){

    try{

      const pacientesResponse =
        await api.get(
          "/pacientes"
        );

      const medicosResponse =
        await api.get(
          "/Medicos"
        );

      setPacientes(
         pacientesResponse.data.data ?? []
      );

      setMedicos(
        medicosResponse.data.data ?? []
      );

    }catch(error){

      toast.error(
        "Erro ao carregar dados"
      );

    }
  }

  async function handleCreate(){

    try{

      setLoading(true);

      await api.post(
        "/Consulta",
        {
          pacienteId:
            Number(pacienteId),

          medicoId:
            Number(medicoId),

          dataConsulta,

          status,

          observacoes
        }
      );

      toast.success(
        "Consulta criada"
      );

      onSuccess();

      onClose();

    }catch(error){

      toast.error(
        "Erro ao criar consulta"
      );

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {

    if(open){
      loadData();
    }

  }, [open]);

  if(!open) return null;

  return(

    <div className="modal-overlay">

      <div className="modal">

        <h2>
          Nova Consulta
        </h2>

        <div className="form-group">

          <label>
            Paciente
          </label>

          <select
            value={pacienteId}
            onChange={(e) =>
              setPacienteId(
                e.target.value
              )
            }
          >

            <option value="">
              Selecione
            </option>

            {
              pacientes.map(
                (paciente) => (

                <option
                  key={paciente.id}
                  value={paciente.id}
                >
                  {paciente.nome}
                </option>

              ))
            }

          </select>

        </div>

        <div className="form-group">

          <label>
            Médico
          </label>

          <select
            value={medicoId}
            onChange={(e) =>
              setMedicoId(
                e.target.value
              )
            }
          >

            <option value="">
              Selecione
            </option>

            {
              medicos.map(
                (medico) => (

                <option
                  key={medico.id}
                  value={medico.id}
                >
                  {medico.nome}
                </option>

              ))
            }

          </select>

        </div>

        <div className="form-group">

          <label>
            Data Consulta
          </label>

          <input
            type="datetime-local"
            value={dataConsulta}
            onChange={(e) =>
              setDataConsulta(
                e.target.value
              )
            }
          />

        </div>

        <div className="form-group">

          <label>
            Status
          </label>

          <select
            value={status}
            onChange={(e) =>
              setStatus(
                e.target.value
              )
            }
          >

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

        <div className="form-group">

          <label>
            Observações
          </label>

          <textarea
            value={observacoes}
            onChange={(e) =>
              setObservacoes(
                e.target.value
              )
            }
          />

        </div>

        <div className="modal-actions">

          <button
            className="cancel-btn"
            onClick={onClose}
          >
            Cancelar
          </button>

          <button
            className="save-btn"
            onClick={handleCreate}
          >

            {
              loading
                ? "Salvando..."
                : "Salvar"
            }

          </button>

        </div>

      </div>

    </div>
  );
}

