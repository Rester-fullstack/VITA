import "./PatientsSummary.css";

type Props = {
  consultas:number;
  receitas:number;
  exames:number;
  atestados:number;
  historicos:number;
};

export default function PatientSummary({
  consultas,
  receitas,
  exames,
  atestados,
  historicos
}:Props){

  const cards = [
    {
      label:"Consultas",
      value:consultas,
      icon:"📅"
    },
    {
      label:"Receitas",
      value:receitas,
      icon:"💊"
    },
    {
      label:"Exames",
      value:exames,
      icon:"🧪"
    },
    {
      label:"Atestados",
      value:atestados,
      icon:"📋"
    },
    {
      label:"Evoluções",
      value:historicos,
      icon:"🩺"
    }
  ];

  return(
    <div className="patient-summary-grid">
      {
        cards.map(card => (
          <div
            key={card.label}
            className="patient-summary-card"
          >
            <div className="summary-icon">
              {card.icon}
            </div>

            <span>
              {card.label}
            </span>

            <strong>
              {card.value}
            </strong>
          </div>
        ))
      }
    </div>
  );
}