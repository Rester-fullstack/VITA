import {
  useEffect,
  useState
} from "react";

import toast from
"react-hot-toast";

import api from
"../../api/axios";

import "./MedicoModal.css";

type Props = {
  open:boolean;
  onClose:() => void;
  onSuccess:() => void;
};

type Especialidade = {
  id:number;
  nome:string;
};

export default function CreateMedicoModal({
  open,
  onClose,
  onSuccess
}:Props){

  const [nome, setNome] =
    useState("");

  const [email, setEmail] =
    useState("");

  const [password, setPassword] =
    useState("");

  const [crm, setCrm] =
    useState("");

  const [especialidadeId,
    setEspecialidadeId] =
      useState("");

  const [especialidades,
    setEspecialidades] =
      useState<Especialidade[]>([]);

  const [loading, setLoading] =
    useState(false);

  async function loadEspecialidades(){

    try{

      const response =
        await api.get(
          "/Especialidade"
        );

      setEspecialidades(
         response.data.data ?? []
      );

    }catch(error){

      toast.error(
        "Erro ao carregar especialidades"
      );

    }
  }

  async function handleCreate(){

    if(
      !nome.trim() ||
      !email.trim() ||
      !password.trim() ||
      !crm.trim() ||
      !especialidadeId
    ){
      toast.error("Preencha todos os campos");
      return;
    }

    try{

      setLoading(true);

      const registerResponse =
        await api.post(
          "/auth/register",
          {
            nome,
            email,
            password,
            role:"Medico"
          }
        );

      const user =
        registerResponse.data.data;

      await api.post(
        "/Medicos",
        {
          crm,
          userId:user.id,
          especialidadeId:Number(especialidadeId)
        }
      );

      toast.success("Médico criado");

      onSuccess();
      onClose();

    }catch(error:any){

      console.error(
        "ERRO AO CRIAR MÉDICO:",
        error.response?.data || error
      );

      toast.error(
        error.response?.data?.message ||
        "Erro ao criar médico"
      );

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {

    if(open){
      loadEspecialidades();
    }

  }, [open]);

  if(!open) return null;

  return(

    <div className="modal-overlay">

      <div className="modal">

        <h2>
          Novo Médico
        </h2>

        <div className="form-group">

          <label>
            Nome
          </label>

          <input
            value={nome}
            onChange={(e) =>
              setNome(
                e.target.value
              )
            }
          />

        </div>

        <div className="form-group">

          <label>
            Email
          </label>

          <input
            value={email}
            onChange={(e) =>
              setEmail(
                e.target.value
              )
            }
          />

        </div>

        <div className="form-group">

          <label>
            Senha
          </label>

          <input
            type="password"
            value={password}
            onChange={(e) =>
              setPassword(
                e.target.value
              )
            }
          />

        </div>

        <div className="form-group">

          <label>
            CRM
          </label>

          <input
            value={crm}
            onChange={(e) =>
              setCrm(
                e.target.value
              )
            }
          />

        </div>

        <div className="form-group">

          <label>
            Especialidade
          </label>

          <select
            value={especialidadeId}
            onChange={(e) =>
              setEspecialidadeId(
                e.target.value
              )
            }
          >

            <option value="">
              Selecione
            </option>

           {
              (especialidades ?? []).map((esp) => (
                <option
                  key={esp.id}
                  value={esp.id}
                >
                  {esp.nome}
                </option>
              ))
           }

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
