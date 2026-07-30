import {
  useEffect,
  useState
} from "react";

import toast from
"react-hot-toast";

import api from
"../../api/axios";

import "./ExameModal.css";

type Props = {
  open:boolean;
  onClose:() => void;
  onSuccess:() => void;
};

type Consulta = {
  id:number;
  pacienteId:number;
  pacienteNome:string;
  dataConsulta:string;
};


export default function CreateExameModal({
  open,
  onClose,
  onSuccess
}:Props){

  const [nome, setNome] =
    useState("");

  const [resultado,
    setResultado] =
      useState("");

  const [dataExame,
    setDataExame] =
      useState("");

  const [consultaId,
    setConsultaId] =
      useState("");

  const [consultas,
    setConsultas] =
      useState<Consulta[]>([]);

  const [loading,
    setLoading] =
      useState(false);

  const [arquivo,
    setArquivo] =
      useState<File | null>(null);

  async function loadData(){

    try{

      const consultasResponse =
        await api.get("/Consulta/minhas");

      setConsultas(
        consultasResponse.data.data ?? []
      );

    }catch{

      toast.error(
        "Erro ao carregar consultas"
      );

    }
  }

  async function handleCreate(){

    try{

      setLoading(true);

      const consultaSelecionada =
        consultas.find(
          (c) =>
            c.id === Number(consultaId)
        );

      if(!consultaSelecionada){
        toast.error(
          "Selecione uma consulta"
        );
        return;
      }

      const formData =
        new FormData();

      formData.append(
        "nome",
        nome
      );

      formData.append(
        "resultado",
        resultado
      );

      formData.append(
        "dataExame",
        dataExame
      );

      formData.append(
        "consultaId",
        consultaId
      );

      formData.append(
        "pacienteId",
        String(consultaSelecionada.pacienteId)
      );

      if(arquivo){

        formData.append(
          "arquivo",
          arquivo
        );

      }

      await api.post(
        "/Exame",
        formData,
        {
          headers:{
            "Content-Type":
              "multipart/form-data"
          }
        }
      );

      toast.success(
        "Exame criado"
      );

      onSuccess();

      onClose();

    }catch{

      toast.error(
        "Erro ao criar exame"
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

    <div className="exame-modal-overlay">

      <div className="exame-modal">

        <h2>
          Novo Exame
        </h2>

        <div className="exame-form-group">

          <label>
            Nome
          </label>

          <input
            value={nome}
            onChange={(e)=>
              setNome(
                e.target.value
              )
            }
          />

        </div>

        <div className="exame-form-group">

          <label>
            Resultado
          </label>

          <textarea
            value={resultado}
            onChange={(e)=>
              setResultado(
                e.target.value
              )
            }
          />

        </div>

        <div className="exame-form-group">

          <label>
            Data Exame
          </label>

          <input
            type="datetime-local"
            value={dataExame}
            onChange={(e)=>
              setDataExame(
                e.target.value
              )
            }
          />

        </div>

        <div className="exame-form-group">

          <label>
            PDF do Exame
          </label>

          <input
            type="file"
            accept=".pdf"
            onChange={(e) =>
              setArquivo(
                e.target.files?.[0] || null
              )
            }
          />

        </div>

        <div className="exame-form-group">

          <label>
            Consulta
          </label>

          <select
            value={consultaId}
            onChange={(e)=>
              setConsultaId(
                e.target.value
              )
            }
          >

            <option value="">
              Selecione
            </option>

            {
              consultas.map(
                (consulta) => (

               <option
                  key={consulta.id}
                  value={consulta.id}
                >
                  {consulta.pacienteNome} - Consulta #{consulta.id}
                </option>

              ))
            }

          </select>

        </div>

        <div className="exame-modal-actions">

          <button
            className="exame-cancel-btn"
            onClick={onClose}
          >
            Cancelar
          </button>

          <button
            className="exame-save-btn"
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