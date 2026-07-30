import {
  useEffect,
  useState
} from "react";

import {
  useParams
} from "react-router-dom";

import api from "../../api/axios";

import "./Odontologia.css";

const dentes = [
  18,17,16,15,14,13,12,11,
  21,22,23,24,25,26,27,28,
  48,47,46,45,44,43,42,41,
  31,32,33,34,35,36,37,38
];

const statusOptions = [
  "Saudável",
  "Cárie",
  "Tratamento",
  "Extraído",
  "Implante"
];

export default function Odontologia(){

  const [registros, setRegistros] =
    useState<any[]>([]);

  const [denteSelecionado,
    setDenteSelecionado] =
      useState<number | null>(null);

  const [status, setStatus] =
    useState("Saudável");

  const [observacoes, setObservacoes] =
    useState("");

  const { id } = useParams();

  const [consulta, setConsulta] =
    useState<any>(null);

  async function loadRegistros(){
    try{
      const response =
        await api.get("/Odontograma");

      setRegistros(
        response.data.data ?? []
      );

    }catch(error){
      console.error(error);
    }
  }

  async function loadConsulta(){
    try{
      const response =
        await api.get(`/Consulta/${id}`);

      setConsulta(
        response.data.data
      );

    }catch(error){
      console.error(error);
    }
  }

  useEffect(() => {
    loadRegistros();
    loadConsulta();
  }, []);

  function getStatusDente(
    dente:number
  ){
    const registro =
      registros
        .filter(r => r.dente === dente)
        .sort(
          (a,b) =>
            new Date(b.dataRegistro).getTime() -
            new Date(a.dataRegistro).getTime()
        )[0];

    return registro?.status || "Saudável";
  }

  async function salvarRegistro(){
    if(
       !denteSelecionado ||
       !consulta
    ){
      alert(
       "Selecione um dente"
      );
      return;
    }

    try{
      await api.post(
        "/Odontograma",
        {
          dente:denteSelecionado,
          status,
          observacoes,
          consultaId:consulta.id,
          pacienteId:consulta.pacienteId
        }
      );

      setObservacoes("");
      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao salvar odontograma");
    }
  }

  async function excluirRegistro(
    id:number
  ){
    const confirmar =
      window.confirm(
        "Deseja excluir este registro?"
      );

    if(!confirmar) return;

    try{
      await api.delete(
        `/Odontograma/${id}`
      );

      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao excluir registro");
    }
  }

  return(
    <div className="odontologia-page">

      <div className="odontologia-header">

        <div>
          <h1>
            Odontograma
          </h1>

          <p>
            Registro visual da condição odontológica do paciente
          </p>
        </div>

      </div>

      <div className="odontologia-layout">

        <div className="odontograma-card">

          <h2>
            Mapa Dentário
          </h2>

          <div className="dentes-grid">

            {
              dentes.map(dente => {

                const statusAtual =
                  getStatusDente(dente);

                return(
                  <button
                    key={dente}
                    className={`dente ${
                      statusAtual
                        .toLowerCase()
                        .replace("á","a")
                    } ${
                      denteSelecionado === dente
                        ? "selected"
                        : ""
                    }`}
                    onClick={() => {
                      setDenteSelecionado(dente);
                      setStatus(statusAtual);
                    }}
                  >
                    🦷
                    <span>
                      {dente}
                    </span>
                  </button>
                );
              })
            }

          </div>

        </div>

        <div className="odontograma-form">

          <h2>
            Registro Clínico
          </h2>

          <label>
            Dente selecionado
          </label>

          <input
            value={
              denteSelecionado
                ? `Dente ${denteSelecionado}`
                : ""
            }
            disabled
            placeholder="Selecione um dente"
          />

         <div className="consulta-context-card">
            <span>Paciente</span>
            <strong>
              {consulta?.pacienteNome || "Carregando..."}
            </strong>

            <small>
              Consulta #{consulta?.id}
            </small>
          </div>

          <label>
            Status
          </label>

          <select
            value={status}
            onChange={(e) =>
              setStatus(e.target.value)
            }
          >
            {
              statusOptions.map(option => (
                <option
                  key={option}
                  value={option}
                >
                  {option}
                </option>
              ))
            }
          </select>

          <label>
            Observações
          </label>

          <textarea
            value={observacoes}
            onChange={(e) =>
              setObservacoes(e.target.value)
            }
            placeholder="Ex: lesão cariosa na face oclusal"
          />

          <button
            onClick={salvarRegistro}
          >
            Salvar Registro
          </button>

        </div>

      </div>

      <div className="odontograma-history">

        <h2>
          Histórico Odontológico
        </h2>

        {
          registros.length === 0 ? (
            <p className="empty">
              Nenhum registro encontrado.
            </p>
          ) : (
            registros.map(registro => (
              <div
                key={registro.id}
                className="history-item"
              >
                <div>
                  <strong>
                    Dente {registro.dente}
                  </strong>

                  <span>
                    {registro.status}
                  </span>

                  <p>
                    {registro.observacoes ||
                      "Sem observações."}
                  </p>

                  <small>
                    {
                      new Date(
                        registro.dataRegistro
                      ).toLocaleString("pt-BR")
                    }
                  </small>
                </div>

                <button
                  onClick={() =>
                    excluirRegistro(registro.id)
                  }
                >
                  Excluir
                </button>
              </div>
            ))
          )
        }

      </div>

    </div>
  );
}