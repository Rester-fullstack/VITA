import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import api from "../../api/axios";

import PatientHeader from "../../components/patient/PatientHeader/PatientHeader";
import PatientSummary from "../../components/patient/PatientsSummary/PatientsSummary";
import PatientInfoCard from "../../components/patient/PatientInfoCard/PatientInfoCard";
import PatientTabs from "../../components/patient/PatientTabs/PatientTabs";
import Timeline from "../../components/patient/Timeline/Timeline";

import "./PacienteDetalhes.css";

export default function PacienteDetalhes() {
  const { id } = useParams();
  const navigate = useNavigate();

  const user =
    JSON.parse(localStorage.getItem("user") || "{}");

  const especialidade = user.especialidade;
  const isAdmin = user.role === "Admin";

  const [paciente, setPaciente] = useState<any>(null);
  const [consultas, setConsultas] = useState<any[]>([]);
  const [historicos, setHistoricos] = useState<any[]>([]);
  const [exames, setExames] = useState<any[]>([]);
  const [receitas, setReceitas] = useState<any[]>([]);
  const [atestados, setAtestados] = useState<any[]>([]);
  const [psicologia, setPsicologia] = useState<any[]>([]);
  const [odontologia, setOdontologia] = useState<any[]>([]);
  const [nutricao, setNutricao] = useState<any[]>([]);
  const [novaEvolucao, setNovaEvolucao] = useState("");
  const [loading, setLoading] = useState(true);

  async function loadData() {
    try {
      setLoading(true);

      const [
        pacienteResponse,
        consultasResponse,
        historicosResponse,
        examesResponse,
        receitasResponse,
        atestadosResponse,
        psicologiaResponse,
        odontologiaResponse,
        nutricaoResponse
      ] = await Promise.all([
        api.get(`/pacientes/${id}`),
        api.get(isAdmin ? "/Consulta" : "/Consulta/minhas"),
        api.get("/HistoricoClinico"),
        api.get("/Exame"),
        api.get("/Receita"),
        api.get("/Atestado"),
        api.get("/Psicologia"),
        api.get("/Odontograma"),
        api.get("/Nutricao")
      ]);

      const consultasPaciente =
        consultasResponse.data.data?.filter(
          (c: any) => Number(c.pacienteId) === Number(id)
        ) || [];

      const consultasOrdenadas =
        [...consultasPaciente].sort(
          (a, b) =>
            new Date(b.dataConsulta).getTime() -
            new Date(a.dataConsulta).getTime()
        );

      const idsConsultasPaciente =
        consultasOrdenadas.map((c: any) => c.id);

      setPaciente(pacienteResponse.data.data);
      setConsultas(consultasOrdenadas);

      setHistoricos(
        historicosResponse.data.data?.filter(
          (h: any) => idsConsultasPaciente.includes(h.consultaId)
        ) || []
      );

      setExames(
        examesResponse.data.data?.filter(
          (e: any) => idsConsultasPaciente.includes(e.consultaId)
        ) || []
      );

      setReceitas(
        receitasResponse.data.data?.filter(
          (r: any) => idsConsultasPaciente.includes(r.consultaId)
        ) || []
      );

      setAtestados(
        atestadosResponse.data.data?.filter(
          (a: any) => idsConsultasPaciente.includes(a.consultaId)
        ) || []
      );

      setPsicologia(
        psicologiaResponse.data.data?.filter(
          (p: any) => idsConsultasPaciente.includes(p.consultaId)
        ) || []
      );

      setOdontologia(
        odontologiaResponse.data.data?.filter(
          (o: any) => idsConsultasPaciente.includes(o.consultaId)
        ) || []
      );

      setNutricao(
        nutricaoResponse.data.data?.filter(
          (n: any) => idsConsultasPaciente.includes(n.consultaId)
        ) || []
      );

    } catch (error) {
      console.error(error);

    } finally {
      setLoading(false);
    }
  }

  async function salvarEvolucao() {
    if (!novaEvolucao.trim()) return;

    if (consultas.length === 0) {
      alert("Este paciente ainda não possui consulta.");
      return;
    }

    try {
      await api.post(
        "/HistoricoClinico",
        {
          descricao: novaEvolucao,
          pacienteId: Number(id),
          consultaId: consultas[0].id
        }
      );

      setNovaEvolucao("");

      await loadData();

    } catch (error) {
      console.error(error);
      alert("Erro ao salvar evolução.");
    }
  }

  async function abrirPdf(url: string) {
    try {
      const response =
        await api.get(
          url,
          {
            responseType: "blob"
          }
        );

      const fileURL =
        URL.createObjectURL(
          new Blob(
            [response.data],
            {
              type: "application/pdf"
            }
          )
        );

      window.open(fileURL, "_blank");

    } catch (error) {
      console.error(error);
      alert("Erro ao abrir PDF.");
    }
  }

  async function imprimirProntuario() {
    await abrirPdf(
      `/Prontuario/paciente/${id}/pdf`
    );
  }

  useEffect(() => {
    loadData();
  }, [id]);

  if (loading) {
    return (
      <div className="loading">
        Carregando paciente...
      </div>
    );
  }

  if (!paciente) {
    return (
      <div className="not-found">
        Paciente não encontrado ou sem permissão de acesso
      </div>
    );
  }

  return (
    <div className="paciente-detalhes">

      <PatientHeader
        paciente={paciente}
        onBack={() => navigate(-1)}
        onPrint={imprimirProntuario}
      />

      <PatientSummary
        consultas={consultas.length}
        receitas={receitas.length}
        exames={exames.length}
        atestados={atestados.length}
        historicos={historicos.length}
      />

      <PatientInfoCard
        paciente={paciente}
      />

      <PatientTabs
        tabs={[
          {
            key: "timeline",
            label: "Timeline",
            icon: "🕒",
            content: (
              <Timeline
                pacienteId={Number(id)}
              />
            )
          },
          {
            key: "evolucao",
            label: "Evolução",
            icon: "🩺",
            content: (
              <div className="tab-section">
                <h2>Prontuário Médico</h2>

                <textarea
                  className="evolucao-input"
                  value={novaEvolucao}
                  onChange={(e) =>
                    setNovaEvolucao(e.target.value)
                  }
                  placeholder="Descreva a evolução clínica..."
                />

                <button
                  className="btn-primary"
                  onClick={salvarEvolucao}
                >
                  Salvar Evolução
                </button>

                <div className="records-list">
                  {
                    historicos.length === 0 ? (
                      <p className="empty">
                        Nenhuma evolução registrada.
                      </p>
                    ) : (
                      historicos.map(historico => (
                        <div
                          key={historico.id}
                          className="record-item"
                        >
                          <span>
                            {
                              new Date(
                                historico.dataRegistro
                              ).toLocaleString("pt-BR")
                            }
                          </span>

                          <p>
                            {historico.descricao}
                          </p>
                        </div>
                      ))
                    )
                  }
                </div>
              </div>
            )
          },
          {
            key: "consultas",
            label: "Consultas",
            icon: "📅",
            content: (
              <div className="records-list">
                {
                  consultas.length === 0 ? (
                    <p className="empty">
                      Nenhuma consulta encontrada.
                    </p>
                  ) : (
                    consultas.map(consulta => (
                      <div
                        key={consulta.id}
                        className="record-item"
                      >
                        <strong>
                          Consulta #{consulta.id}
                        </strong>

                        <span>
                          {
                            new Date(
                              consulta.dataConsulta
                            ).toLocaleString("pt-BR")
                          }
                        </span>

                        <p>
                          Status: {consulta.status}
                        </p>

                        <button
                          className="small-btn"
                          onClick={() =>
                            navigate(`/consulta/${consulta.id}`)
                          }
                        >
                          Abrir Consulta
                        </button>
                      </div>
                    ))
                  )
                }
              </div>
            )
          },
          {
            key: "exames",
            label: "Exames",
            icon: "🧪",
            content: (
              <div className="records-list">
                {
                  exames.length === 0 ? (
                    <p className="empty">
                      Nenhum exame encontrado.
                    </p>
                  ) : (
                    exames.map(exame => (
                      <div
                        key={exame.id}
                        className="record-item"
                      >
                        <strong>
                          {exame.nome}
                        </strong>

                        <p>
                          {exame.resultado}
                        </p>

                        {
                          exame.pdfUrl ? (
                            <a
                              href={encodeURI(
                                `http://localhost:5182${exame.pdfUrl}`
                              )}
                              target="_blank"
                              rel="noreferrer"
                              className="pdf-link"
                            >
                              📄 Ver PDF
                            </a>
                          ) : (
                            <small>
                              Nenhum PDF anexado
                            </small>
                          )
                        }
                      </div>
                    ))
                  )
                }
              </div>
            )
          },
          {
            key: "documentos",
            label: "Documentos",
            icon: "📄",
            content: (
              <div className="documents-grid">

                <div className="document-column">
                  <h2>Receitas</h2>

                  {
                    receitas.length === 0 ? (
                      <p className="empty">
                        Nenhuma receita encontrada.
                      </p>
                    ) : (
                      receitas.map(receita => (
                        <div
                          key={receita.id}
                          className="record-item"
                        >
                          <strong>
                            Receita #{receita.id}
                          </strong>

                          <p>
                            {receita.medicamento} • {receita.dosagem} • {receita.frequencia}
                          </p>

                          <small>
                            {
                              new Date(
                                receita.dataReceita
                              ).toLocaleString("pt-BR")
                            }
                          </small>

                          <button
                            className="small-btn"
                            onClick={() =>
                              abrirPdf(`/Receita/pdf/${receita.id}`)
                            }
                          >
                            📄 PDF
                          </button>
                        </div>
                      ))
                    )
                  }
                </div>

                <div className="document-column">
                  <h2>Atestados</h2>

                  {
                    atestados.length === 0 ? (
                      <p className="empty">
                        Nenhum atestado encontrado.
                      </p>
                    ) : (
                      atestados.map(atestado => (
                        <div
                          key={atestado.id}
                          className="record-item"
                        >
                          <strong>
                            Atestado #{atestado.id}
                          </strong>

                          <p>
                            {atestado.motivo} • {atestado.diasAfastamento} dia(s)
                          </p>

                          <small>
                            {
                              new Date(
                                atestado.dataEmissao
                              ).toLocaleString("pt-BR")
                            }
                          </small>

                          <button
                            className="small-btn"
                            onClick={() =>
                              abrirPdf(`/Atestado/pdf/${atestado.id}`)
                            }
                          >
                            📄 PDF
                          </button>
                        </div>
                      ))
                    )
                  }
                </div>

              </div>
            )
          },
          {
            key: "especialidade",
            label: "Especialidade",
            icon:
              especialidade === "Odontologia"
                ? "🦷"
                : especialidade === "Psicologia"
                  ? "🧠"
                  : "🥗",
            content: (
              <div className="records-list">

                {
                  especialidade === "Psicologia" && (
                    <>
                      <h2>Psicologia</h2>

                      {
                        psicologia.length === 0 ? (
                          <p className="empty">
                            Nenhum registro psicológico encontrado.
                          </p>
                        ) : (
                          psicologia.map(registro => (
                            <div
                              key={registro.id}
                              className="record-item"
                            >
                              <strong>
                                Humor: {registro.humor}
                              </strong>

                              <p>{registro.queixaPrincipal}</p>

                              <p>{registro.evolucaoSessao}</p>

                              <small>
                                {
                                  new Date(
                                    registro.dataRegistro
                                  ).toLocaleString("pt-BR")
                                }
                              </small>
                            </div>
                          ))
                        )
                      }
                    </>
                  )
                }

                {
                  especialidade === "Odontologia" && (
                    <>
                      <h2>Odontologia</h2>

                      {
                        odontologia.length === 0 ? (
                          <p className="empty">
                            Nenhum registro odontológico encontrado.
                          </p>
                        ) : (
                          odontologia.map(registro => (
                            <div
                              key={registro.id}
                              className="record-item"
                            >
                              <strong>
                                Dente {registro.dente} - {registro.status}
                              </strong>

                              <p>
                                {registro.observacoes || "Sem observações."}
                              </p>

                              <small>
                                {
                                  new Date(
                                    registro.dataRegistro
                                  ).toLocaleString("pt-BR")
                                }
                              </small>
                            </div>
                          ))
                        )
                      }
                    </>
                  )
                }

                {
                  especialidade === "Nutrição" && (
                    <>
                      <h2>Nutrição</h2>

                      {
                        nutricao.length === 0 ? (
                          <p className="empty">
                            Nenhum registro nutricional encontrado.
                          </p>
                        ) : (
                          nutricao.map(registro => (
                            <div
                              key={registro.id}
                              className="record-item"
                            >
                              <strong>
                                IMC {registro.imc}
                              </strong>

                              <p>
                                <b>Objetivo:</b> {registro.objetivo}
                              </p>

                              <p>
                                <b>Plano:</b> {registro.planoAlimentar}
                              </p>

                              <small>
                                {
                                  new Date(
                                    registro.dataRegistro
                                  ).toLocaleString("pt-BR")
                                }
                              </small>
                            </div>
                          ))
                        )
                      }
                    </>
                  )
                }

              </div>
            )
          }
        ]}
      />

    </div>
  );
}