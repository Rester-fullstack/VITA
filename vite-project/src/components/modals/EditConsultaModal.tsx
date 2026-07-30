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
  consulta:any;
};

export default function EditConsultaModal({
  open,
  onClose,
  onSuccess,
  consulta
}:Props){

  const [dataConsulta,
    setDataConsulta] =
      useState("");

  const [status,
    setStatus] =
      useState("");

  const [observacoes,
    setObservacoes] =
      useState("");

  const [loading,
    setLoading] =
      useState(false);

  

  async function handleEdit(){

    try{

      setLoading(true);

      await api.put(
        `/Consulta/${consulta.id}`,
        {
         pacienteId:
          consulta.pacienteId,

         medicoId:
          consulta.medicoId,

          dataConsulta,

          status,

          observacoes
        }
      );

      toast.success(
        "Consulta atualizada"
      );

      onSuccess();

      onClose();

    }catch(error){

      toast.error(
        "Erro ao atualizar consulta"
      );

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {

    if(!open || !consulta) return;

    setDataConsulta(
      consulta.dataConsulta?.slice(0,16) || ""
    );

    setStatus(
      consulta.status || ""
    );

    setObservacoes(
      consulta.observacoes || ""
    );

  }, [open, consulta]);

  if(!open) return null;

  return(

    <div className="modal-overlay">

      <div className="modal">

        <h2>
          Editar Consulta
        </h2>

        <div className="form-group">

          <label>
            Paciente
          </label>

          <input
            type="text"
            value={consulta.pacienteNome}
            disabled
          />

        </div>


       <div className="form-group">

          <label>
            Médico
          </label>

          <input
            type="text"
            value={consulta.medicoNome}
            disabled
          />

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
            onClick={handleEdit}
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

