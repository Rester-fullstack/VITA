import {
  useState
} from "react";

import toast from
"react-hot-toast";

import api from
"../../api/axios";

import {
  examesPorEspecialidade
} from "../../data/examePorEspecialidade";

import "./SolicitacaoExameModal.css";

type Props = {
    open: boolean;
    consultaId: number;
    especialidade: string;
    onClose: () => void;
    onSuccess: () => void;
};

export default function SolicitacaoExameModal({
  open,
  consultaId,
  especialidade,
  onClose,
  onSuccess
}:Props){

  if(!open) return null;

  const sugestoes =
    examesPorEspecialidade[
      especialidade
    ] || examesPorEspecialidade.Geral;

  const [selecionados, setSelecionados] =
    useState<string[]>([]);

  const [outros, setOutros] =
    useState("");

  const [justificativa, setJustificativa] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  function toggleExame(exame:string){
    if(selecionados.includes(exame)){
      setSelecionados(
        selecionados.filter(
          item => item !== exame
        )
      );
    }else{
      setSelecionados([
        ...selecionados,
        exame
      ]);
    }
  }

  async function salvar(){

    const examesFinais = [
      ...selecionados,
      ...outros
        .split("\n")
        .map(x => x.trim())
        .filter(x => x.length > 0)
    ];

    if(examesFinais.length === 0){
      toast.error(
        "Selecione ou digite pelo menos um exame"
      );
      return;
    }

    try{
      setLoading(true);

      await api.post(
        "/SolicitacaoExame",
        {
          consultaId,
          examesSolicitados:
            examesFinais.join("\n"),
          justificativa
        }
      );

      toast.success(
        "Solicitação de exame criada"
      );

      onSuccess();
      onClose();

    }catch(error){
      console.error(error);

      toast.error(
        "Erro ao criar solicitação"
      );

    }finally{
      setLoading(false);
    }
  }

  return(
    <div className="modal-overlay">

      <div className="solicitacao-modal">

        <h2>
          Solicitação de Exame
        </h2>

        <p>
          Sugestões para {especialidade}
        </p>

        <div className="exames-grid">
          {
            sugestoes.map(exame => (
              <label
                key={exame}
                className="exame-check"
              >
                <input
                  type="checkbox"
                  checked={
                    selecionados.includes(exame)
                  }
                  onChange={() =>
                    toggleExame(exame)
                  }
                />

                <span>
                  {exame}
                </span>
              </label>
            ))
          }
        </div>

        <label>
          Outros exames
        </label>

        <textarea
          placeholder="Digite um exame por linha..."
          value={outros}
          onChange={(e) =>
            setOutros(e.target.value)
          }
        />

        <label>
          Justificativa
        </label>

        <textarea
          placeholder="Justificativa clínica..."
          value={justificativa}
          onChange={(e) =>
            setJustificativa(e.target.value)
          }
        />

        <div className="modal-actions">

          <button
            className="cancel-btn"
            onClick={onClose}
          >
            Cancelar
          </button>

          <button
            className="save-btn"
            onClick={salvar}
          >
            {
              loading
                ? "Salvando..."
                : "Emitir Solicitação"
            }
          </button>

        </div>

      </div>

    </div>
  );
}