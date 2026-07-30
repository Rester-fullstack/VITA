using VitaApi.DTOs.Auditorias;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repository;
    private readonly IUserContextService _userContext;

    public AuditoriaService(
        IAuditoriaRepository repository,
        IUserContextService userContext
    )
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<List<AuditoriaResponseDto>> GetAllAsync()
    {
        var lista =
            await _repository.GetAllAsync();

        return lista.Select(x => new AuditoriaResponseDto
        {
            Id = x.Id,
            Entidade = x.Entidade,
            Acao = x.Acao,
            Descricao = x.Descricao,
            DataHora = x.DataHora,
            UsuarioId = x.UsuarioId,
            UsuarioNome = x.UsuarioNome,
            UsuarioRole = x.UsuarioRole,
            ConsultaId = x.ConsultaId,
            PacienteId = x.PacienteId,
            RegistroId = x.RegistroId
        }).ToList();
    }

    public async Task RegistrarAsync(
        string entidade,
        string acao,
        string descricao,
        int? consultaId = null,
        int? pacienteId = null,
        int? registroId = null,
        string? icone = "📄",
        string? cor = "#2563EB"
    )
    {
        var auditoria = new Auditoria
        {
            Entidade = entidade,
            Acao = acao,
            Descricao = descricao,
            DataHora = DateTime.Now,

            UsuarioId = _userContext.UserId,
            UsuarioNome = _userContext.UserName,
            UsuarioRole = _userContext.Role,

            ConsultaId = consultaId,
            PacienteId = pacienteId,
            RegistroId = registroId,
            Icone = icone,
            Cor = cor
        };

        await _repository.AddAsync(auditoria);

        await _repository.SaveChangesAsync();
    }
}