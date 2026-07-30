import {
  useEffect,
  useState
} from "react";

import {
  useNavigate
} from "react-router-dom";

import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell
} from "recharts";

import api from "../../api/axios";

import "./MedicoDashboard.css";

export default function MedicoDashboard(){

  const navigate = useNavigate();

  const [consultas, setConsultas] =
    useState<any[]>([]);

  const [exames, setExames] =
   useState<any[]>([]);

  const [receitas, setReceitas] =
    useState<any[]>([]);

  const [atestados, setAtestados] =
    useState<any[]>([]);

  const [psicologia, setPsicologia] =
   useState<any[]>([]);

  const [odontologia, setOdontologia] =
    useState<any[]>([]);

  const [nutricao, setNutricao] =
    useState<any[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [busca, setBusca] =
    useState("");
  
  const user =
    JSON.parse(
      localStorage.getItem("user") || "{}"
    );

  const especialidade =
    user.especialidade;

  async function loadDashboardData(){

    try{

      const [
        consultasRes,
        examesRes,
        receitasRes,
        atestadosRes,
        psicologiaRes,
        odontologiaRes,
        nutricaoRes
      ] =
        await Promise.all([
          api.get("/Consulta/minhas"),
          api.get("/Exame"),
          api.get("/Receita"),
          api.get("/Atestado"),
          api.get("/Psicologia"),
          api.get("/Odontograma"),
          api.get("/Nutricao")
        ]);

      const consultasMedico =
        consultasRes.data.data ?? [];

      const idsConsultasMedico =
        consultasMedico.map(
          (c:any) => c.id
        );

      setConsultas(consultasMedico);

      setExames(
        (examesRes.data.data ?? [])
          .filter((e:any) =>
            idsConsultasMedico.includes(e.consultaId)
          )
      );

      setReceitas(
        (receitasRes.data.data ?? [])
          .filter((r:any) =>
            idsConsultasMedico.includes(r.consultaId)
          )
      );

      setAtestados(
        (atestadosRes.data.data ?? [])
          .filter((a:any) =>
            idsConsultasMedico.includes(a.consultaId)
          )
      );

      setPsicologia(
        (psicologiaRes.data.data ?? [])
          .filter((p:any) =>
            idsConsultasMedico.includes(p.consultaId)
          )
      );

      setOdontologia(
        (odontologiaRes.data.data ?? [])
          .filter((o:any) =>
            idsConsultasMedico.includes(o.consultaId)
          )
      );

      setNutricao(
        (nutricaoRes.data.data ?? [])
          .filter((n:any) =>
            idsConsultasMedico.includes(n.consultaId)
          )
      );

    }catch(error){

      console.error(error);

    }finally{

      setLoading(false);

    }
  }
  

  useEffect(() => {
    loadDashboardData();
  }, []);

  if(loading){
    return (
      <div className="loading">
        Carregando...
      </div>
    );
  }

  const hoje =
    new Date().toDateString();

  const consultasHoje =
    consultas.filter(c =>
      new Date(c.dataConsulta)
        .toDateString() === hoje
    ).length;

  const finalizadas =
    consultas.filter(c =>
      c.status === "Finalizada"
    ).length;

  const canceladas =
    consultas.filter(c =>
      c.status === "Cancelada"
    ).length;

  const confirmadas =
    consultas.filter(c =>
      c.status === "Confirmada"
    ).length;

  const agendadas =
    consultas.filter(c =>
      c.status === "Agendada"
    ).length;

  const pacientesUnicos =
    new Set(
      consultas.map(c => c.pacienteNome)
    ).size;

  const proximasConsultas =
    consultas
      .filter(c =>
        new Date(c.dataConsulta) > new Date()
      )
      .sort(
        (a,b) =>
          new Date(a.dataConsulta).getTime() -
          new Date(b.dataConsulta).getTime()
      );

  const proximaConsulta =
    proximasConsultas[0] || null;

  const consultasFiltradas =
    consultas.filter(c =>
      (c.pacienteNome || "")
        .toLowerCase()
        .includes(
          busca.toLowerCase()
        )
    );

  const statusData = [
    {
      name: "Agendadas",
      value: agendadas
    },
    {
      name: "Confirmadas",
      value: confirmadas
    },
    {
      name: "Finalizadas",
      value: finalizadas
    },
    {
      name: "Canceladas",
      value: canceladas
    }
  ];

  const meses = [
    "Jan", "Fev", "Mar", "Abr",
    "Mai", "Jun", "Jul", "Ago",
    "Set", "Out", "Nov", "Dez"
  ];

  const consultasPorMes =
    meses.map((mes, index) => ({
      mes,
      consultas:
        consultas.filter(c =>
          new Date(c.dataConsulta).getMonth() === index
        ).length
    }));

  const agendaSemana =
    proximasConsultas.slice(0, 5);

  const especialidadeTotal =
    especialidade === "Psicologia"
      ? psicologia.length
      : especialidade === "Odontologia"
        ? odontologia.length
        : especialidade === "Nutrição"
          ? nutricao.length
          : 0;

  const especialidadeTitulo =
    especialidade === "Psicologia"
      ? "Sessões"
      : especialidade === "Odontologia"
        ? "Odontogramas"
        : especialidade === "Nutrição"
          ? "Avaliações"
          : "Especialidade";

  const especialidadeIcone =
    especialidade === "Psicologia"
      ? "🧠"
      : especialidade === "Odontologia"
        ? "🦷"
        : especialidade === "Nutrição"
          ? "🥗"
          : "⭐";

  const total =
    finalizadas + canceladas;

  const taxaComparecimento =
    total > 0
      ? Math.round(
          (finalizadas / total) * 100
        )
      : 0;

  const mesAtual =
    new Date().getMonth();

  const consultasMes =
    consultas.filter(c =>
      new Date(c.dataConsulta)
        .getMonth() === mesAtual
    ).length;

  return(

    <div className="medico-dashboard">

      <div className="dashboard-header">

        <div>
          <h1 className="dashboard-title">
            Dashboard Médico
          </h1>

          <p className="dashboard-subtitle">
            Visão geral das consultas, pacientes e agenda
          </p>
        </div>

      </div>

      <div className="stats-grid">

        <div className="next-consultation-card">

          <h3>
            Próxima Consulta
          </h3>

          {
            proximaConsulta ? (
              <>
                <strong>
                  {proximaConsulta.pacienteNome}
                </strong>

                <p>
                  {
                    new Date(
                      proximaConsulta.dataConsulta
                    ).toLocaleString("pt-BR")
                  }
                </p>

                <span>
                  {proximaConsulta.status}
                </span>
              </>
            ) : (
              <p>
                Nenhuma consulta agendada
              </p>
            )
          }

        </div>

        <div className="stat-card">
          <div className="stat-icon blue">
            📅
          </div>

          <div>
            <span>Hoje</span>
            <h2>{consultasHoje}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon blue">
            📆
          </div>

          <div>
            <span>Este mês</span>
            <h2>{consultasMes}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon green">
            📈
          </div>

          <div>
            <span>Comparecimento</span>
            <h2>{taxaComparecimento}%</h2>
          </div>
        </div>
        
        <div className="stat-card">
          <div className="stat-icon green">
            ✅
          </div>

          <div>
            <span>Finalizadas</span>
            <h2>{finalizadas}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon orange">
            👥
          </div>

          <div>
            <span>Pacientes</span>
            <h2>{pacientesUnicos}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon purple">
            🕒
          </div>

          <div>
            <span>Próximas</span>
            <h2>{proximasConsultas.length}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon blue">
            🧪
          </div>

          <div>
            <span>Exames</span>
            <h2>{exames.length}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon green">
            💊
          </div>

          <div>
            <span>Receitas</span>
            <h2>{receitas.length}</h2>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon purple">
            📋
          </div>

          <div>
            <span>Atestados</span>
            <h2>{atestados.length}</h2>
          </div>
        </div>

        <div className="stat-card special-stat">
          <div className="stat-icon special">
            {especialidadeIcone}
          </div>

          <div>
            <span>{especialidadeTitulo}</span>
            <h2>{especialidadeTotal}</h2>
          </div>
        </div>

      </div>

      <div className="charts-grid">

        <div className="chart-card">

          <h3>
            Consultas por Mês
          </h3>

          <ResponsiveContainer
            width="100%"
            height={260}
          >
            <BarChart data={consultasPorMes}>
              <XAxis dataKey="mes" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Bar
                dataKey="consultas"
                fill="#2563EB"
                radius={[8,8,0,0]}
              />
            </BarChart>
          </ResponsiveContainer>

        </div>

        <div className="chart-card">

          <h3>
            Status das Consultas
          </h3>

          <ResponsiveContainer
            width="100%"
            height={260}
          >
            <PieChart>
              <Pie
                data={statusData}
                dataKey="value"
                nameKey="name"
                outerRadius={90}
                label
              >
                {
                  statusData.map((_, index) => (
                    <Cell
                      key={index}
                      fill={[
                        "#60A5FA",
                        "#22C55E",
                        "#A855F7",
                        "#EF4444"
                      ][index]}
                    />
                  ))
                }
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>

        </div>

      </div>

      <div className="agenda-card">

        <h3>
          Agenda da Semana
        </h3>

        {
          agendaSemana.length === 0 ? (
            <p className="empty-agenda">
              Nenhuma consulta futura encontrada
            </p>
          ) : (
            agendaSemana.map(consulta => (

              <div
                key={consulta.id}
                className="agenda-item"
              >

                <div>
                  <strong>
                    {consulta.pacienteNome}
                  </strong>

                  <span>
                    {
                      new Date(
                        consulta.dataConsulta
                      ).toLocaleString("pt-BR")
                    }
                  </span>
                </div>

                <button
                  onClick={() =>
                    navigate(
                      `/consulta/${consulta.id}`
                    )
                  }
                >
                  Abrir
                </button>

              </div>

            ))
          )
        }

      </div>

      <div className="recent-section">

        <div className="section-header">

          <h2>
            Todas as Consultas
          </h2>

          <input
            type="text"
            placeholder="Buscar paciente..."
            value={busca}
            onChange={(e) =>
              setBusca(e.target.value)
            }
            className="search-input"
          />

        </div>

        <div className="table-container">

          <table>

            <thead>
              <tr>
                <th>Paciente</th>
                <th>Data</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>

            <tbody>

              {
                consultasFiltradas.map(consulta => (

                  <tr key={consulta.id}>

                    <td>
                      {consulta.pacienteNome}
                    </td>

                    <td>
                      {
                        new Date(
                          consulta.dataConsulta
                        ).toLocaleString("pt-BR")
                      }
                    </td>

                    <td>
                      <span
                        className={`status ${
                          consulta.status
                            .toLowerCase()
                        }`}
                      >
                        {consulta.status}
                      </span>
                    </td>

                    <td>
                      <button
                        className="btn-open"
                        onClick={() =>
                          navigate(
                            `/consulta/${consulta.id}`
                          )
                        }
                      >
                        Abrir
                      </button>
                    </td>

                  </tr>

                ))
              }

            </tbody>

          </table>

        </div>

      </div>

      <div className="activity-card">

        <h3>
          Atividade Clínica Recente
        </h3>

        <div className="activity-list">

          {
            receitas.slice(0,3).map(receita => (
              <div
                key={`receita-${receita.id}`}
                className="activity-item"
              >
                <span>💊</span>

                <div>
                  <strong>
                    Receita emitida
                  </strong>

                  <p>
                    {receita.pacienteNome || "Paciente"}
                  </p>
                </div>
              </div>
            ))
          }

          {
            atestados.slice(0,3).map(atestado => (
              <div
                key={`atestado-${atestado.id}`}
                className="activity-item"
              >
                <span>📋</span>

                <div>
                  <strong>
                    Atestado emitido
                  </strong>

                  <p>
                    {atestado.pacienteNome || "Paciente"}
                  </p>
                </div>
              </div>
            ))
          }

          {
            exames.slice(0,3).map(exame => (
              <div
                key={`exame-${exame.id}`}
                className="activity-item"
              >
                <span>🧪</span>

                <div>
                  <strong>
                    Exame registrado
                  </strong>

                  <p>
                    {exame.pacienteNome || "Paciente"}
                  </p>
                </div>
              </div>
            ))
          }

        </div>

      </div>

    </div>
  );
}