using AutoMapper;
using VitaApi.DTOs.Medicos;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class MedicoService
{
    private readonly IMedicoRepository
        _repository;

    private readonly IAuditoriaService _auditoriaService;

    private readonly IMapper _mapper;

    public MedicoService(
        IMedicoRepository repository,
        IMapper mapper,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _auditoriaService = auditoriaService;
    }

    private static MedicoResponseDto MapToResponse(
    Medico medico
)
    {
        return new MedicoResponseDto
        {
            Id = medico.Id,
            CRM = medico.CRM,
            Nome = medico.User.Nome,
            Email = medico.User.Email,
            EspecialidadeId =
                medico.EspecialidadeId,
            Especialidade =
                medico.Especialidade.Nome,
            Telefone =
                medico.Telefone,
            Cidade =
                medico.Cidade,
            Estado =
                medico.Estado,
            EnderecoProfissional =
                medico.EnderecoProfissional,
            Assinatura =
                medico.Assinatura
        };
    }

   

    public async Task<
        List<MedicoResponseDto>
    > GetAllAsync()
    {
        var medicos =
            await _repository.GetAllAsync();

        return medicos.Select(m =>
            new MedicoResponseDto
            {
                Id = m.Id,

                CRM = m.CRM,

                Nome = m.User.Nome,

                Email = m.User.Email,

                EspecialidadeId =
                    m.EspecialidadeId,

                Especialidade =
                    m.Especialidade.Nome,

                Telefone =
                    m.Telefone,

                Cidade =
                    m.Cidade,

                Estado =
                    m.Estado,

                EnderecoProfissional =
                    m.EnderecoProfissional,

                Assinatura =
                    m.Assinatura
            }
        ).ToList();
    }


    public async Task<
        MedicoResponseDto?
    > GetByIdAsync(int id)
    {
        var medico =
            await _repository.GetByIdAsync(id);

        if (medico == null)
            return null;

        return new MedicoResponseDto
        {
            Id = medico.Id,

            CRM = medico.CRM,

            Nome = medico.User.Nome,

            Email = medico.User.Email,

                EspecialidadeId =
            medico.EspecialidadeId,

                Especialidade =
            medico.Especialidade.Nome,

                Telefone =
            medico.Telefone,

                Cidade =
            medico.Cidade,

                Estado =
            medico.Estado,

                EnderecoProfissional =
            medico.EnderecoProfissional,

                Assinatura =
            medico.Assinatura
        };
    }

  

    public async Task<MedicoResponseDto> CreateAsync(
    CreateMedicoDto dto,
    int? usuarioId,
    string? usuarioNome,
    string? usuarioRole
)
    {
        var medico = new Medico
        {
            CRM = dto.CRM,

            UserId = dto.UserId,

                EspecialidadeId =
            dto.EspecialidadeId,

                Telefone =
            dto.Telefone,

                Cidade =
            dto.Cidade,

                Estado =
            dto.Estado,

                EnderecoProfissional =
            dto.EnderecoProfissional,

                Assinatura =
            dto.Assinatura
        };

        await _repository.AddAsync(medico);

        await _repository.SaveChangesAsync();

        var created =
            await _repository.GetByIdAsync(medico.Id);

        await _auditoriaService.RegistrarAsync(
            entidade: "Médico",
            acao: "Criou",
            descricao: $"Médico {created!.User.Nome} cadastrado",
            registroId: created.Id,
            icone: "👨‍⚕️",
            cor: "#3B82F6"
        );

        return new MedicoResponseDto
        {
            Id = created.Id,

            CRM = created.CRM,

            Nome = created.User.Nome,

            Email = created.User.Email,

            EspecialidadeId =
         created.EspecialidadeId,

            Especialidade =
         created.Especialidade.Nome,

            Telefone =
         created.Telefone,

            Cidade =
         created.Cidade,

            Estado =
         created.Estado,

            EnderecoProfissional =
         created.EnderecoProfissional,

            Assinatura =
         created.Assinatura
        };
    }

   

    public async Task<bool>
        UpdateAsync(
            int id,
            UpdateMedicoDto dto
        )
    {
        var medico =
            await _repository.GetByIdAsync(id);

        if (medico == null)
            return false;

        medico.CRM = dto.CRM;

        medico.EspecialidadeId = dto.EspecialidadeId;

        medico.Telefone =
            dto.Telefone;

        medico.Cidade =
            dto.Cidade;

        medico.Estado =
            dto.Estado;

        medico.EnderecoProfissional =
            dto.EnderecoProfissional;

        medico.Assinatura =
            dto.Assinatura;

        medico.User.Nome = dto.Nome;
        medico.User.Email = dto.Email;

        await _repository.UpdateAsync(
            medico
        );

        await _repository.SaveChangesAsync();

        return true;
    }


    public async Task<MedicoResponseDto?>
    GetMeuPerfilAsync(
        int userId
    )
    {
        var medico =
            await _repository.GetByUserIdAsync(
                userId
            );

        if (medico == null)
            return null;

        return MapToResponse(medico);
    }

    public async Task<MedicoResponseDto?>
        UpdateMeuPerfilAsync(
            int userId,
            UpdateMedicoDto dto
        )
    {
        var medico =
            await _repository.GetByUserIdAsync(
                userId
            );

        if (medico == null)
            return null;

        medico.CRM =
            dto.CRM.Trim();

        medico.EspecialidadeId =
            dto.EspecialidadeId;

        medico.Telefone =
            dto.Telefone?.Trim();

        medico.Cidade =
            dto.Cidade?.Trim();

        medico.Estado =
            dto.Estado?.Trim();

        medico.EnderecoProfissional =
            dto.EnderecoProfissional?.Trim();

        medico.Assinatura =
            dto.Assinatura?.Trim();

        medico.User.Nome =
            dto.Nome.Trim();

        medico.User.Email =
            dto.Email.Trim();

        await _repository.UpdateAsync(medico);

        await _repository.SaveChangesAsync();

        var atualizado =
            await _repository.GetByUserIdAsync(
                userId
            );

        await _auditoriaService.RegistrarAsync(
            entidade: "Perfil Médico",
            acao: "Atualizou",
            descricao:
                $"O médico {medico.User.Nome} atualizou o próprio perfil.",
            registroId: medico.Id,
            icone: "👤",
            cor: "#2563EB"
        );

        return atualizado == null
            ? null
            : MapToResponse(atualizado);
    }

   

    public async Task<bool>
        DeleteAsync(int id)
    {
        var medico =
            await _repository.GetByIdAsync(id);

        if (medico == null)
            return false;

        await _repository.DeleteAsync(
            medico
        );

        await _repository.SaveChangesAsync();

        return true;
    }
}

