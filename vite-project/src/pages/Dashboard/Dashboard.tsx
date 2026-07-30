import {
  motion
} from "framer-motion";

import {
  useEffect,
  useState
} from "react";

import {
  Users,
  UserRound,
  CalendarDays,
  XCircle,
  FlaskConical,
  ClipboardList,
  FileCheck,
  ScrollText,
  TestTube2,
  FileText
} from "lucide-react";

import toast from "react-hot-toast";

import api from "../../api/axios";

import StatCard from "../../components/dashboard/StatCard/StatCard";

import AuditTimeline from "../../components/dashboard/AuditTimeline/AuditTimeline";

import DashboardCharts from "../../components/dashboard/DashboardCharts/DashboardCharts";

import "./Dashboard.css";

type DashboardTimeline = {
  entidade: string;
  acao: string;
  descricao: string;
  usuario: string;
  dataHora: string;
  icone: string;
  cor: string;
};

type ChartItem = {
  nome: string;
  valor: number;
};


type DashboardData = {
  totalPacientes: number;
  totalMedicos: number;
  totalConsultas: number;
  consultasHoje: number;
  consultasSemana: number;
  consultasCanceladas: number;
  totalExames: number;
  totalReceitas: number;
  totalAtestados: number;
  totalSolicitacoesExames: number;
  totalDeclaracoes: number;
  timeline: DashboardTimeline[];
  ultimosPacientes?: any[];
  ultimosExames?: any[];
  consultasPorMes: ChartItem[];
  documentosEmitidos: ChartItem[];

};

export default function Dashboard() {
  const [data, setData] =
    useState<DashboardData>({
      totalPacientes: 0,
      totalMedicos: 0,
      totalConsultas: 0,
      consultasHoje: 0,
      consultasSemana: 0,
      consultasCanceladas: 0,
      totalExames: 0,
      totalReceitas: 0,
      totalAtestados: 0,
      totalSolicitacoesExames: 0,
      totalDeclaracoes: 0,
      timeline: [],
      ultimosPacientes: [],
      ultimosExames: [],
      consultasPorMes: [],
      documentosEmitidos: []
    });

  const [loading, setLoading] =
    useState(true);

  async function loadDashboard() {
    try {
      const response =
        await api.get("/dashboard/admin");

     const dashboard =
        response.data.data;

      setData({
        totalPacientes: dashboard.totalPacientes ?? 0,
        totalMedicos: dashboard.totalMedicos ?? 0,
        totalConsultas: dashboard.totalConsultas ?? 0,
        consultasHoje: dashboard.consultasHoje ?? 0,
        consultasSemana: dashboard.consultasSemana ?? 0,
        consultasCanceladas: dashboard.consultasCanceladas ?? 0,
        totalExames: dashboard.totalExames ?? 0,
        totalReceitas: dashboard.totalReceitas ?? 0,
        totalAtestados: dashboard.totalAtestados ?? 0,
        totalSolicitacoesExames: dashboard.totalSolicitacoesExames ?? 0,
        totalDeclaracoes: dashboard.totalDeclaracoes ?? 0,
        timeline: dashboard.timeline ?? [],
        ultimosPacientes: dashboard.ultimosPacientes ?? [],
        ultimosExames: dashboard.ultimosExames ?? [],
        consultasPorMes: dashboard.consultasPorMes ?? [],
        documentosEmitidos: dashboard.documentosEmitidos ?? []
      });

    } catch (error) {
      console.error(error);

      toast.error(
        "Erro ao carregar dashboard"
      );

    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadDashboard();
  }, []);

  if (loading) {
    return (
      <div className="loading">
        Carregando dashboard...
      </div>
    );
  }

  return (
    <motion.div
      className="admin-dashboard"
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
    >
      <div className="dashboard-header">
        <div>
          <h1 className="dashboard-title">
            Dashboard Admin
          </h1>

          <p className="dashboard-subtitle">
            Visão geral da clínica e atividades recentes
          </p>
        </div>
      </div>

      <div className="stats-grid">
        <StatCard
          title="Pacientes"
          value={data.totalPacientes}
          icon={<Users size={24} />}
          color="#2563EB"
        />

        <StatCard
          title="Médicos"
          value={data.totalMedicos}
          icon={<UserRound size={24} />}
          color="#8B5CF6"
        />

        <StatCard
          title="Consultas Hoje"
          value={data.consultasHoje}
          icon={<CalendarDays size={24} />}
          color="#22C55E"
        />

        <StatCard
          title="Canceladas"
          value={data.consultasCanceladas}
          icon={<XCircle size={24} />}
          color="#EF4444"
        />

        <StatCard
          title="Total Consultas"
          value={data.totalConsultas}
          icon={<ClipboardList size={24} />}
          color="#0EA5E9"
        />

        <StatCard
          title="Receitas"
          value={data.totalReceitas}
          icon={<ScrollText size={24} />}
          color="#22C55E"
        />

        <StatCard
          title="Atestados"
          value={data.totalAtestados}
          icon={<FileCheck size={24} />}
          color="#F97316"
        />

        <StatCard
          title="Exames"
          value={data.totalExames}
          icon={<FlaskConical size={24} />}
          color="#A855F7"
        />

        <StatCard
          title="Solicitações"
          value={data.totalSolicitacoesExames}
          icon={<TestTube2 size={24} />}
          color="#F59E0B"
        />

        <StatCard
          title="Declarações"
          value={data.totalDeclaracoes}
          icon={<FileText size={24} />}
          color="#06B6D4"
        />
      </div>

      <DashboardCharts

          consultasPorMes={
              data.consultasPorMes
          }

          documentosEmitidos={
              data.documentosEmitidos
          }

      />

      <div className="dashboard-main-grid">
        <div className="dashboard-panel">
          <div className="section-header">
            <h2>
              Atividades Recentes
            </h2>
          </div>

          <AuditTimeline
            items={data.timeline}
          />
        </div>

        <div className="dashboard-panel">
          <div className="section-header">
            <h2>
              Últimos Pacientes
            </h2>
          </div>

          <div className="simple-list">
            {
              data.ultimosPacientes?.length ? (
                data.ultimosPacientes.map((paciente: any) => (
                  <div
                    key={paciente.id}
                    className="simple-item"
                  >
                    <span>👤</span>

                    <div>
                      <strong>
                        {paciente.nome}
                      </strong>

                      <small>
                        Paciente #{paciente.id}
                      </small>
                    </div>
                  </div>
                ))
              ) : (
                <p className="empty">
                  Nenhum paciente encontrado.
                </p>
              )
            }
          </div>
        </div>

        <div className="dashboard-panel">
          <div className="section-header">
            <h2>
              Últimos Exames
            </h2>
          </div>

          <div className="simple-list">
            {
              data.ultimosExames?.length ? (
                data.ultimosExames.map((exame: any) => (
                  <div
                    key={exame.id}
                    className="simple-item"
                  >
                    <span>🧪</span>

                    <div>
                      <strong>
                        {exame.nome}
                      </strong>

                      <small>
                        {exame.resultado || "Sem resultado"}
                      </small>
                    </div>
                  </div>
                ))
              ) : (
                <p className="empty">
                  Nenhum exame encontrado.
                </p>
              )
            }
          </div>
        </div>
      </div>
    </motion.div>
  );
}