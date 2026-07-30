import {
  useEffect,
  useState
} from "react";

import toast from "react-hot-toast";
import api from "../../api/axios";
import "./MedicoModal.css";

type Props = {
  open:boolean;
  onClose:() => void;
  onSuccess:() => void;
  medico:any;
};

type Especialidade = {
  id:number;
  nome:string;
};

export default function EditMedicoModal({
  open,
  onClose,
  onSuccess,
  medico
}:Props){

  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [crm, setCrm] = useState("");
  const [telefone, setTelefone] = useState("");

  const [cidade, setCidade] = useState("");

  const [estado, setEstado] = useState("");

  const [enderecoProfissional, setEnderecoProfissional] =
    useState("");

  const [assinatura, setAssinatura] =
    useState("");
  const [especialidadeId, setEspecialidadeId] = useState("");
  const [especialidades, setEspecialidades] = useState<Especialidade[]>([]);
  const [loading, setLoading] = useState(false);

  async function loadEspecialidades(){
    try{
      const response = await api.get("/Especialidade");
      setEspecialidades(response.data.data ?? []);
    }catch{
      toast.error("Erro ao carregar especialidades");
    }
  }

  async function handleEdit(){
    try{
      setLoading(true);

      await api.put(
        `/Medicos/${medico.id}`,
        {
          nome,
          email,
          crm,

          especialidadeId:
            Number(especialidadeId),

          telefone,

          cidade,

          estado,

          enderecoProfissional,

          assinatura
        }
      );

      toast.success("Médico atualizado");
      onSuccess();
      onClose();

    }catch{
      toast.error("Erro ao editar médico");

    }finally{
      setLoading(false);
    }
  }

  useEffect(() => {
    if(open && medico){
      setNome(medico.nome || "");
      setEmail(medico.email || "");
      setCrm(medico.crm || "");
      setTelefone(medico.telefone || "");
      setCidade(medico.cidade || "");
      setEstado(medico.estado || "");
      setEnderecoProfissional(medico.enderecoProfissional || "");
      setAssinatura(medico.assinatura || "");
      setEspecialidadeId(
        medico.especialidadeId?.toString() || ""
      );

      loadEspecialidades();
    }
  }, [open, medico]);

  if(!open) return null;

  return(
    <div className="modal-overlay">
      <div className="modal">

        <h2>Editar Médico</h2>

        <div className="form-group">
          <label>Nome</label>
          <input
            value={nome}
            onChange={(e) => setNome(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>Email</label>
          <input
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>CRM</label>
          <input
            value={crm}
            onChange={(e) => setCrm(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>Telefone</label>

          <input
            value={telefone}
            onChange={(e) =>
              setTelefone(e.target.value)
            }
          />
        </div>

        <div className="form-group">
          <label>Cidade</label>

          <input
            value={cidade}
            onChange={(e) =>
              setCidade(e.target.value)
            }
          />
        </div>

        <div className="form-group">
          <label>Estado</label>

          <input
            value={estado}
            onChange={(e) =>
              setEstado(e.target.value)
            }
          />
        </div>

        <div className="form-group">
          <label>Endereço Profissional</label>

          <input
            value={enderecoProfissional}
            onChange={(e) =>
              setEnderecoProfissional(
                e.target.value
              )
            }
          />
        </div>

        <div className="form-group">
          <label>Assinatura</label>

          <textarea
            rows={4}
            value={assinatura}
            onChange={(e) =>
              setAssinatura(
                e.target.value
              )
            }
          />
        </div>

        <div className="form-group">
          <label>Especialidade</label>

          <select
            value={especialidadeId}
            onChange={(e) => setEspecialidadeId(e.target.value)}
          >
            <option value="">Selecione</option>

            {especialidades.map((esp) => (
              <option
                key={esp.id}
                value={esp.id}
              >
                {esp.nome}
              </option>
            ))}
          </select>
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
            disabled={loading}
          >
            {loading ? "Salvando..." : "Salvar"}
          </button>
        </div>

      </div>
    </div>
  );
}