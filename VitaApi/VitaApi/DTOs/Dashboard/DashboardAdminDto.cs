namespace VitaApi.DTOs.Dashboard;

public class DashboardAdminDto
{
    
    public int TotalPacientes { get; set; }
    public int TotalMedicos { get; set; }
    public int TotalConsultas { get; set; }
    public int ConsultasHoje { get; set; }
    public int ConsultasSemana { get; set; }
    public int ConsultasCanceladas { get; set; }

   
    public int TotalReceitas { get; set; }
    public int TotalAtestados { get; set; }
    public int TotalSolicitacoesExames { get; set; }
    public int TotalDeclaracoes { get; set; }

    public int TotalExames { get; set; }

    
    public object? UltimosPacientes { get; set; }
    public object? UltimosExames { get; set; }

    
    public List<DashboardTimelineDto> Timeline { get; set; } = [];

    public List<DashboardChartDto> ConsultasPorMes { get; set; } = [];

    public List<DashboardChartDto> DocumentosEmitidos { get; set; } = [];

}