using AutoMapper;
using VitaApi.DTOs.Consultas;
using VitaApi.Interfaces;
using VitaApi.Models;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _repository;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IMapper _mapper;

    public ConsultaService(
        IConsultaRepository repository,
        IMapper mapper,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _auditoriaService = auditoriaService;
    }

    private static void ValidarExclusao(Consulta consulta)
    {
        if (consulta.Status != "Agendada")
            throw new Exception(
                "Somente consultas agendadas podem ser excluídas."
            );

        if (consulta.HistoricosClinicos.Any())
            throw new Exception(
                "Não é possível excluir porque existe histórico clínico."
            );

        if (consulta.Receitas.Any())
            throw new Exception(
                "Não é possível excluir porque existem receitas."
            );

        if (consulta.Atestados.Any())
            throw new Exception(
                "Não é possível excluir porque existem atestados."
            );

        if (consulta.Exames.Any())
            throw new Exception(
                "Não é possível excluir porque existem exames."
            );

        if (consulta.SolicitacoesExames.Any())
            throw new Exception(
                "Não é possível excluir porque existem solicitações de exame."
            );

        if (consulta.DeclaracoesComparecimento.Any())
            throw new Exception(
                "Não é possível excluir porque existem declarações."
            );
    }

    private bool PodeExcluir(Consulta consulta)
    {
        return consulta.Status == "Agendada"
            && !consulta.HistoricosClinicos.Any()
            && !consulta.Receitas.Any()
            && !consulta.Atestados.Any()
            && !consulta.Exames.Any()
            && !consulta.SolicitacoesExames.Any()
            && !consulta.DeclaracoesComparecimento.Any();
    }

    public async Task<List<ConsultaResponseDto>> GetAllAsync()
    {
        var consultas = await _repository.GetAllAsync();

        var dtos = _mapper.Map<List<ConsultaResponseDto>>(consultas);

        for (int i = 0; i < consultas.Count; i++)
        {
            dtos[i].PodeExcluir =
                consultas[i].Status == "Agendada";
        }

        return dtos;
    }

    public async Task<ConsultaResponseDto?> GetByIdAsync(int id)
    {
        var consulta = await _repository.GetByIdForDeleteAsync(id);

        if (consulta == null)
            return null;

        var dto = _mapper.Map<ConsultaResponseDto>(consulta);

        dto.PodeExcluir = PodeExcluir(consulta);

        return dto;
    }

    public async Task<List<ConsultaResponseDto>>
    GetMyConsultasAsync(int userId)
    {
        var consultas =
            await _repository.GetByUserIdAsync(
                userId
            );

        return _mapper.Map<
            List<ConsultaResponseDto>
        >(consultas);
    }

    public async Task<ConsultaResponseDto> CreateAsync(
         CreateConsultaDto dto,
         int? usuarioId,
         string? usuarioNome,
         string? usuarioRole
     )
    {
        var conflito = await _repository.ExistsConflictAsync(
            dto.MedicoId,
            dto.DataConsulta
        );

        if (conflito)
            throw new Exception("Já existe uma consulta nesse horário para esse médico.");

        var consulta = _mapper.Map<Consulta>(dto);

        consulta.Status = "Agendada";

        await _repository.AddAsync(consulta);
        await _repository.SaveChangesAsync();


        var consultaCompleta =
        await _repository.GetByIdAsync(consulta.Id);

            await _auditoriaService.RegistrarAsync(

                entidade: "Consulta",

                acao: "Criou",

                descricao:
                    $"Consulta agendada para {consultaCompleta!.Paciente?.Nome}",

                consultaId: consultaCompleta.Id,

                pacienteId: consultaCompleta.PacienteId,

                registroId: consultaCompleta.Id,

                icone: "📅",

                cor: "#2563EB"

            );

        return _mapper.Map<ConsultaResponseDto>(
            consultaCompleta
        );

   
    }

    public async Task<ConsultaResponseDto>
    CreateMinhaConsultaAsync(

        int userId,

        string? usuarioNome,

        string? usuarioRole,

        CreateMinhaConsultaDto dto

    )
    {
        var medicoId =
            await _repository.GetMedicoIdByUserIdAsync(userId);

        if (medicoId == null)
            throw new Exception("Médico não encontrado.");

        var createDto =
     new CreateConsultaDto
     {
         PacienteId = dto.PacienteId,
         MedicoId = medicoId.Value,
         DataConsulta = dto.DataConsulta,
         Observacoes =
             dto.Observacoes ?? "Primeira consulta"
     };

        return await CreateAsync(

            createDto,

            userId,

            usuarioNome,

            usuarioRole

        );
    }

    public async Task<bool> UpdateAsync(int id, UpdateConsultaDto dto)
    {
        var consulta = await _repository.GetByIdAsync(id);

        if (consulta == null)
            return false;

        consulta.DataConsulta = dto.DataConsulta;
        consulta.Status = dto.Status;
        consulta.Observacoes = dto.Observacoes;
        consulta.PacienteId = dto.PacienteId;
        consulta.MedicoId = dto.MedicoId;

        await _repository.UpdateAsync(consulta);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var consulta =
            await _repository.GetByIdForDeleteAsync(id);

        if (consulta == null)
            return false;

        ValidarExclusao(consulta);

        await _repository.DeleteAsync(consulta);
        await _repository.SaveChangesAsync();

        return true;
    }
}