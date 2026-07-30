using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.DTOs.ConfiguracoesClinica;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class ConfiguracaoClinicaService
    : IConfiguracaoClinicaService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;

    public ConfiguracaoClinicaService(
        AppDbContext context,
        IAuditoriaService auditoriaService
    )
    {
        _context = context;
        _auditoriaService = auditoriaService;
    }

    public async Task<ConfiguracaoClinicaDto> GetAsync()
    {
        var config =
            await _context.ConfiguracoesClinica
                .FirstOrDefaultAsync();

        if (config == null)
        {
            config = new ConfiguracaoClinica
            {
                NomePlataforma = "VITA",
                RodapePdf =
                    "Documento emitido eletronicamente pela plataforma VITA.",
                AtualizadoEm = DateTime.UtcNow
            };

            await _context.ConfiguracoesClinica.AddAsync(config);
            await _context.SaveChangesAsync();
        }

        return Map(config);
    }

    public async Task<ConfiguracaoClinicaDto> UpdateAsync(
        UpdateConfiguracaoClinicaDto dto
    )
    {
        var config =
            await _context.ConfiguracoesClinica
                .FirstOrDefaultAsync();

        if (config == null)
        {
            config = new ConfiguracaoClinica();

            await _context.ConfiguracoesClinica.AddAsync(config);
        }

        config.NomePlataforma =
            dto.NomePlataforma.Trim();

        config.EmailSuporte =
            dto.EmailSuporte.Trim();

        config.TelefoneSuporte =
            dto.TelefoneSuporte.Trim();

        config.WhatsappSuporte =
            dto.WhatsappSuporte.Trim();

        config.RodapePdf =
            dto.RodapePdf.Trim();

        config.MensagemPadrao =
            dto.MensagemPadrao.Trim();

        config.AtualizadoEm =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(
            entidade: "Configuração da Plataforma",
            acao: "Atualizou",
            descricao:
                "As configurações gerais da plataforma foram atualizadas.",
            registroId: config.Id,
            icone: "⚙️",
            cor: "#6366F1"
        );

        return Map(config);
    }

    private static ConfiguracaoClinicaDto Map(
        ConfiguracaoClinica config
    )
    {
        return new ConfiguracaoClinicaDto
        {
            Id = config.Id,
            NomePlataforma = config.NomePlataforma,
            EmailSuporte = config.EmailSuporte,
            TelefoneSuporte = config.TelefoneSuporte,
            WhatsappSuporte = config.WhatsappSuporte,
            RodapePdf = config.RodapePdf,
            MensagemPadrao = config.MensagemPadrao,
            AtualizadoEm = config.AtualizadoEm
        };
    }
}