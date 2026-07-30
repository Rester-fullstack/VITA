using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.DTOs.Notificacoes;
using VitaApi.Interfaces;

namespace VitaApi.Services;

public class NotificacaoService
    : INotificacaoService
{
    private readonly AppDbContext _context;

    public NotificacaoService(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<NotificacaoDto>>
        GetUltimasAsync()
    {
        return await _context.Auditorias

            .OrderByDescending(x => x.DataHora)

            .Take(15)

            .Select(x => new NotificacaoDto
            {
                Id = x.Id,

                Titulo =
                    $"{x.Acao} - {x.Entidade}",

                Descricao =
                    x.Descricao,

                Icone =
                    x.Icone,

                Cor =
                    x.Cor,

                DataHora =
                    x.DataHora
            })

            .ToListAsync();
    }
}