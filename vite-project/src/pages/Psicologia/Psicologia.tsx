import {
  useEffect,
  useState
} from "react";

import {
  useParams
} from "react-router-dom";

import api from "../../api/axios";

import "./Psicologia.css";

const humores = [
  "Estável",
  "Ansioso",
  "Triste",
  "Irritado",
  "Calmo",
  "Eufórico"
];

export default function Psicologia(){

  const [registros, setRegistros] =
    useState<any[]>([]);

  const [humor, setHumor] =
    useState("Estável");

  const [queixaPrincipal, setQueixaPrincipal] =
    useState("");

  const [evolucaoSessao, setEvolucaoSessao] =
    useState("");

  const [observacoes, setObservacoes] =
    useState("");

  const { id } = useParams();

  const [consulta, setConsulta] =
    useState<any>(null);

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

  async function loadRegistros(){
    try{
      const response =
        await api.get("/Psicologia");

      const lista =
        (response.data.data ?? [])
          .filter((r:any) =>
            Number(r.consultaId) === Number(id)
          );

      setRegistros(lista);

    }catch(error){
      console.error(error);
    }
  }

  useEffect(() => {
    if(id){
      loadConsulta();
      loadRegistros();
    }
  }, [id]);

  async function salvarRegistro(){
    if(
      !humor ||
      !queixaPrincipal.trim() ||
      !evolucaoSessao.trim() ||
      !consulta
    ){
      alert("Preencha os campos obrigatórios");
      return;
    }

    try{
      await api.post(
        "/Psicologia",
        {
          humor,
          queixaPrincipal,
          evolucaoSessao,
          observacoes,
          consultaId:Number(consulta.id),
          pacienteId:Number(consulta.pacienteId)
        }
      );

      setHumor("Estável");
      setQueixaPrincipal("");
      setEvolucaoSessao("");
      setObservacoes("");

      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao salvar registro psicológico");
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
        `/Psicologia/${id}`
      );

      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao excluir registro");
    }
  }

  return(
    <div className="psicologia-page">

      <div className="psicologia-header">
        <div>
          <h1>
            Psicologia
          </h1>

          <p>
            Registro de sessões, humor e evolução terapêutica
          </p>
        </div>
      </div>

      <div className="psicologia-layout">

        <div className="psicologia-form-card">

          <h2>
            Nova Sessão
          </h2>

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
            Humor
          </label>

          <select
            value={humor}
            onChange={(e) =>
              setHumor(e.target.value)
            }
          >
            {
              humores.map(h => (
                <option
                  key={h}
                  value={h}
                >
                  {h}
                </option>
              ))
            }
          </select>

          <label>
            Queixa principal
          </label>

          <textarea
            placeholder="Ex: dificuldade para dormir..."
            value={queixaPrincipal}
            onChange={(e) =>
              setQueixaPrincipal(e.target.value)
            }
          />

          <label>
            Evolução da sessão
          </label>

          <textarea
            placeholder="Descreva a evolução terapêutica..."
            value={evolucaoSessao}
            onChange={(e) =>
              setEvolucaoSessao(e.target.value)
            }
          />

          <label>
            Observações
          </label>

          <textarea
            placeholder="Observações adicionais..."
            value={observacoes}
            onChange={(e) =>
              setObservacoes(e.target.value)
            }
          />

          <button
            onClick={salvarRegistro}
          >
            Salvar Sessão
          </button>

        </div>

        <div className="psicologia-summary-card">

          <h2>
            Resumo Clínico
          </h2>

          <div className="summary-item">
            <span>Total de registros</span>
            <strong>{registros.length}</strong>
          </div>

          <div className="summary-item">
            <span>Último humor</span>
            <strong>
              {
                registros[0]?.humor ||
                "Sem registro"
              }
            </strong>
          </div>

          <div className="summary-item">
            <span>Último paciente</span>
            <strong>
              {
                registros[0]?.pacienteNome ||
                "Sem registro"
              }
            </strong>
          </div>

        </div>

      </div>

      <div className="psicologia-history">

        <h2>
          Histórico de Sessões
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
                className="sessao-item"
              >

                <div className="sessao-top">

                  <div>
                    <strong>
                      {registro.pacienteNome}
                    </strong>

                    <span>
                      {
                        new Date(
                          registro.dataRegistro
                        ).toLocaleString("pt-BR")
                      }
                    </span>
                  </div>

                  <small>
                    {registro.humor}
                  </small>

                </div>

                <div className="sessao-content">

                  <p>
                    <b>Queixa:</b>{" "}
                    {registro.queixaPrincipal}
                  </p>

                  <p>
                    <b>Evolução:</b>{" "}
                    {registro.evolucaoSessao}
                  </p>

                  {
                    registro.observacoes && (
                      <p>
                        <b>Observações:</b>{" "}
                        {registro.observacoes}
                      </p>
                    )
                  }

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