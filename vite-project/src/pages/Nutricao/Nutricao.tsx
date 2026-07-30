import {
  useEffect,
  useState
} from "react";

import {
  useParams
} from "react-router-dom";
import api from "../../api/axios";

import "./Nutricao.css";

export default function Nutricao(){

  const [registros, setRegistros] =
    useState<any[]>([]);

  const [peso, setPeso] =
    useState("");

  const [altura, setAltura] =
    useState("");

  const [objetivo, setObjetivo] =
    useState("");

  const [planoAlimentar, setPlanoAlimentar] =
    useState("");

  const [evolucao, setEvolucao] =
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

      setConsulta(response.data.data);

    }catch(error){
      console.error(error);
    }
  }  
  async function loadRegistros(){
    try{
      const response =
        await api.get("/Nutricao");

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

  const imcPreview =
    Number(peso) > 0 &&
    Number(altura) > 0
      ? (
          Number(peso) /
          (Number(altura) * Number(altura))
        ).toFixed(2)
      : "0.00";

  async function salvarRegistro(){
    if(
      !peso ||
      !altura ||
      !objetivo.trim() ||
      !planoAlimentar.trim() ||
      !consulta
    ){
      alert("Preencha os campos obrigatórios");
      return;
    }

    try{
      await api.post(
        "/Nutricao",
        {
          peso: Number(peso),
          altura: Number(altura),
          objetivo,
          planoAlimentar,
          evolucao,
          observacoes,
          consultaId: (consulta.id),
          pacienteId: (consulta.pacienteId)
        }
      );

      setPeso("");
      setAltura("");
      setObjetivo("");
      setPlanoAlimentar("");
      setEvolucao("");
      setObservacoes("");

      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao salvar registro nutricional");
    }
  }

  async function excluirRegistro(id:number){
    const confirmar =
      window.confirm(
        "Deseja excluir este registro?"
      );

    if(!confirmar) return;

    try{
      await api.delete(
        `/Nutricao/${id}`
      );

      await loadRegistros();

    }catch(error){
      console.error(error);
      alert("Erro ao excluir registro");
    }
  }

  return(
    <div className="nutricao-page">

      <div className="nutricao-header">
        <div>
          <h1>
            Nutrição
          </h1>

          <p>
            Acompanhamento nutricional, IMC, plano alimentar e evolução
          </p>
        </div>
      </div>

      <div className="nutricao-layout">

        <div className="nutricao-form-card">

          <h2>
            Novo Registro Nutricional
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

          <div className="nutricao-row">

            <div>
              <label>Peso</label>
              <input
                type="number"
                step="0.01"
                placeholder="Ex: 82.5"
                value={peso}
                onChange={(e) =>
                  setPeso(e.target.value)
                }
              />
            </div>

            <div>
              <label>Altura</label>
              <input
                type="number"
                step="0.01"
                placeholder="Ex: 1.75"
                value={altura}
                onChange={(e) =>
                  setAltura(e.target.value)
                }
              />
            </div>

          </div>

          <div className="imc-preview">
            <span>IMC calculado</span>
            <strong>{imcPreview}</strong>
          </div>

          <label>Objetivo</label>
          <input
            type="text"
            placeholder="Ex: Perda de gordura corporal"
            value={objetivo}
            onChange={(e) =>
              setObjetivo(e.target.value)
            }
          />

          <label>Plano alimentar</label>
          <textarea
            placeholder="Descreva o plano alimentar..."
            value={planoAlimentar}
            onChange={(e) =>
              setPlanoAlimentar(e.target.value)
            }
          />

          <label>Evolução</label>
          <textarea
            placeholder="Evolução do paciente..."
            value={evolucao}
            onChange={(e) =>
              setEvolucao(e.target.value)
            }
          />

          <label>Observações</label>
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
            Salvar Registro
          </button>

        </div>

        <div className="nutricao-summary-card">

          <h2>
            Resumo Nutricional
          </h2>

          <div className="summary-item">
            <span>Total de registros</span>
            <strong>{registros.length}</strong>
          </div>

          <div className="summary-item">
            <span>Último IMC</span>
            <strong>
              {
                registros[0]?.imc ||
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

      <div className="nutricao-history">

        <h2>
          Histórico Nutricional
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
                className="nutricao-item"
              >

                <div className="nutricao-item-top">

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
                    IMC {registro.imc}
                  </small>

                </div>

                <div className="nutricao-metrics">

                  <div>
                    <span>Peso</span>
                    <strong>{registro.peso} kg</strong>
                  </div>

                  <div>
                    <span>Altura</span>
                    <strong>{registro.altura} m</strong>
                  </div>

                  <div>
                    <span>Objetivo</span>
                    <strong>{registro.objetivo}</strong>
                  </div>

                </div>

                <div className="nutricao-content">

                  <p>
                    <b>Plano alimentar:</b>{" "}
                    {registro.planoAlimentar}
                  </p>

                  {
                    registro.evolucao && (
                      <p>
                        <b>Evolução:</b>{" "}
                        {registro.evolucao}
                      </p>
                    )
                  }

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