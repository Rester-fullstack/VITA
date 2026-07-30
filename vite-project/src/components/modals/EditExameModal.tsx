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
  exame:any;
};

export default function EditExameModal({
  open,
  onClose,
  onSuccess,
  exame
}:Props){

  const [nome, setNome] =
    useState("");

  const [resultado,
    setResultado] =
      useState("");

  const [loading,
    setLoading] =
      useState(false);


  const [arquivo,
    setArquivo] =
      useState<File | null>(null);

  async function handleEdit(){

    try{

      setLoading(true);

      await api.put(
        `/Exame/${exame.id}`,
        {
          ...exame,
          nome,
          resultado
        }
      );

      // UPLOAD PDF

      if(arquivo){

        const formData =
          new FormData();

        formData.append(
          "file",
          arquivo
        );

        await api.post(
          `/Exame/upload/${exame.id}`,
          formData,
          {
            headers:{
              "Content-Type":
                "multipart/form-data"
            }
          }
        );
      }

      toast.success(
        "Exame atualizado"
      );

      onSuccess();

      onClose();

    }catch{

      toast.error(
        "Erro ao atualizar exame"
      );

    }finally{

      setLoading(false);

    }
  }

  useEffect(() => {

    if(open && exame){

      setNome(
        exame.nome || ""
      );

      setResultado(
        exame.resultado || ""
      );
    }

  }, [open, exame]);

  if(!open) return null;

  return(

    <div className="exame-modal-overlay">

      <div className="exame-modal">

        <h2>
          Editar Exame
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
            PDF do exame
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

        <div className="exame-modal-actions">

          <button
            className="exame-cancel-btn"
            onClick={onClose}
          >
            Cancelar
          </button>

          <button
            className="exame-save-btn"
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