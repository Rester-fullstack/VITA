using VitaApi.DTOs.Auditorias;

namespace VitaApi.Interfaces;

public interface IAuditoriaService
{
    Task<List<AuditoriaResponseDto>> GetAllAsync();

    Task RegistrarAsync(
        string entidade,
        string acao,
        string descricao,
        int? consultaId = null,
        int? pacienteId = null,
        int? registroId = null,
        string? icone = "📄",
        string? cor = "#2563EB"
    );
}