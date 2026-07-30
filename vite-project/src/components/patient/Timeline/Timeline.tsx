import {
  useEffect,
  useState
} from "react";

import api from "../../../api/axios";

import "./Timeline.css";

type Props = {
  pacienteId: number;
};

export default function Timeline({
  pacienteId
}: Props) {

  const [timeline, setTimeline] =
    useState<any[]>([]);

  const [loading, setLoading] =
    useState(true);

  useEffect(() => {
    loadTimeline();
  }, [pacienteId]);

  async function loadTimeline() {
    try {
      const response =
        await api.get(
          `/Timeline/paciente/${pacienteId}`
        );

      setTimeline(
        response.data.data ?? []
      );

    } catch (error) {
      console.error(error);

    } finally {
      setLoading(false);
    }
  }

  function getIcon(tipo: string) {
    switch (tipo) {
      case "Consulta":
        return "🩺";
      case "Histórico":
        return "📋";
      case "Receita":
        return "💊";
      case "Atestado":
        return "📄";
      case "Declaração":
        return "🧾";
      case "Solicitação":
        return "🧪";
      case "Exame":
        return "🧬";
      default:
        return "📌";
    }
  }

  function getPdfUrl(item: any) {
    if (!item.documentoId) return null;

    switch (item.tipo) {
      case "Receita":
        return `/Receita/pdf/${item.documentoId}`;

      case "Atestado":
        return `/Atestado/pdf/${item.documentoId}`;

      case "Declaração":
        return `/DeclaracaoComparecimento/pdf/${item.documentoId}`;

      case "Solicitação":
        return `/SolicitacaoExame/pdf/${item.documentoId}`;

      default:
        return null;
    }
  }

  async function abrirPdf(item: any) {
    const url = getPdfUrl(item);

    if (!url) return;

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
      alert("Erro ao abrir PDF");
    }
  }

  if (loading) {
    return (
      <div className="timeline-loading">
        Carregando timeline...
      </div>
    );
  }

  return (
    <div className="timeline-container">

      <div className="timeline-header">
        <div>
          <h2>
            Timeline do Paciente
          </h2>

          <p>
            Histórico completo dos eventos clínicos.
          </p>
        </div>
      </div>

      {
        timeline.length === 0 ? (

          <div className="timeline-empty">
            Nenhum registro encontrado.
          </div>

        ) : (

          <div className="timeline-list">
            {
              timeline.map(item => {

                const pdfUrl =
                  getPdfUrl(item);

                return (
                  <div
                    key={`${item.tipo}-${item.documentoId}-${item.data}`}
                    className={`timeline-item timeline-${item.tipo
                      .toLowerCase()
                      .normalize("NFD")
                      .replace(/[\u0300-\u036f]/g, "")
                    }`}
                  >

                    <div className="timeline-marker">
                      <span>
                        {getIcon(item.tipo)}
                      </span>
                    </div>

                    <div className="timeline-content">

                      <div className="timeline-top">
                        <span className="timeline-type">
                          {item.tipo}
                        </span>

                        <span className="timeline-date">
                          {
                            new Date(
                              item.data
                            ).toLocaleString("pt-BR")
                          }
                        </span>
                      </div>

                      <h3>
                        {item.titulo}
                      </h3>

                      <p>
                        {item.descricao}
                      </p>

                      {
                        pdfUrl && (
                          <button
                            className="timeline-pdf-btn"
                            onClick={() =>
                              abrirPdf(item)
                            }
                          >
                            📄 Abrir PDF
                          </button>
                        )
                      }

                    </div>

                  </div>
                );
              })
            }
          </div>
        )
      }

    </div>
  );
}