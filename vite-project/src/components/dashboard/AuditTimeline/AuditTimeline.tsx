import "./AuditTimeline.css";

type TimelineItem = {
  entidade: string;
  acao: string;
  descricao: string;
  usuario: string;
  dataHora: string;
  icone: string;
  cor: string;
};

type Props = {
  items: TimelineItem[];
};

export default function AuditTimeline({
  items
}: Props) {
  return (
    <div className="audit-timeline">
      {
        items.length === 0 ? (
          <p className="empty">
            Nenhuma atividade registrada.
          </p>
        ) : (
          items.map((item, index) => (
            <div
              key={index}
              className="audit-item"
            >
              <div
                className="audit-icon"
                style={{
                  background: `${item.cor || "#2563EB"}22`,
                  color: item.cor || "#2563EB"
                }}
              >
                {item.icone || "📄"}
              </div>

              <div className="audit-content">
                <strong>
                  {item.descricao}
                </strong>

                <span>
                  {item.usuario || "Sistema"}
                </span>

                <small>
                  {
                    new Date(
                      item.dataHora
                    ).toLocaleString("pt-BR")
                  }
                </small>
              </div>
            </div>
          ))
        )
      }
    </div>
  );
}