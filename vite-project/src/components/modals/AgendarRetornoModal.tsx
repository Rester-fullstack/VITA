import {
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

export default function AgendarRetornoModal({
  open,
  onClose,
  onSuccess,
  consulta
}:Props){

  const [dataConsulta, setDataConsulta] =
    useState("");

  const [observacoes, setObservacoes] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  if(!open) return null;

  async function handleCreate(){

    if(!dataConsulta){
      toast.error("Informe a data do retorno");
      return;
    }

    try{

      setLoading(true);

      await api.post(
        "/Consulta",
        {
          pacienteId: consulta.pacienteId,
          medicoId: consulta.medicoId,
          dataConsulta,
          status: "Agendada",
          observacoes:
            observacoes ||
            `Retorno da consulta #${consulta.id}`
        }
      );

      toast.success("Retorno agendado");

      onSuccess();
      onClose();

    }catch(error){

      console.error(error);

      toast.error(
        "Erro ao agendar retorno"
      );

    }finally{

      setLoading(false);

    }
  }

  return(
    <div className="modal-overlay">

      <div className="modal">

        <h2>
          Agendar Retorno
        </h2>

        <div className="form-group">
          <label>
            Paciente
          </label>

          <input
            value={consulta?.pacienteNome || ""}
            disabled
          />
        </div>

        <div className="form-group">
          <label>
            Médico
          </label>

          <input
            value={consulta?.medicoNome || ""}
            disabled
          />
        </div>

        <div className="form-group">
          <label>
            Data do retorno
          </label>

          <input
            type="datetime-local"
            value={dataConsulta}
            onChange={(e) =>
              setDataConsulta(e.target.value)
            }
          />
        </div>

        <div className="form-group">
          <label>
            Observações
          </label>

          <textarea
            placeholder="Ex: retorno para acompanhamento..."
            value={observacoes}
            onChange={(e) =>
              setObservacoes(e.target.value)
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
                ? "Agendando..."
                : "Agendar"
            }
          </button>

        </div>

      </div>

    </div>
  );
}