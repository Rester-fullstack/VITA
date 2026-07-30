import "./ViewConsulta.css";

type Consulta = {
  id: number;
  pacienteNome: string;
  medicoNome: string;
  dataConsulta: string;
  status: string;
  observacoes: string;
};

type Props = {
  open: boolean;
  onClose: () => void;
  consulta: Consulta | null;
};

export default function ViewConsultaModal({
  open,
  onClose,
  consulta
}: Props) {

  if (!open || !consulta) return null;

  return (

    <div
      className="modal-overlay"
      onClick={onClose}
    >

      <div
        className="modal"
        onClick={(e) => e.stopPropagation()}
      >

        <div className="modal-header">

          <h2>
            Visualizar Consulta
          </h2>

          <button
            className="close-btn"
            onClick={onClose}
          >
            ✕
          </button>

        </div>

        <div className="modal-body">

          <div className="info-group">

            <label>ID</label>

            <input
              value={consulta.id}
              disabled
            />

          </div>

          <div className="info-group">

            <label>Paciente</label>

            <input
              value={consulta.pacienteNome}
              disabled
            />

          </div>

          <div className="info-group">

            <label>Médico</label>

            <input
              value={consulta.medicoNome}
              disabled
            />

          </div>

          <div className="info-group">

            <label>Data</label>

            <input
              value={
                new Date(
                  consulta.dataConsulta
                ).toLocaleString()
              }
              disabled
            />

          </div>

          <div className="info-group">

            <label>Status</label>

            <input
              value={consulta.status}
              disabled
            />

          </div>

          <div className="info-group">

            <label>Observações</label>

            <textarea
              value={consulta.observacoes}
              disabled
              rows={5}
            />

          </div>

        </div>

        <div className="modal-footer">

          <button
            className="cancel-btn"
            onClick={onClose}
          >
            Fechar
          </button>

        </div>

      </div>

    </div>

  );
}