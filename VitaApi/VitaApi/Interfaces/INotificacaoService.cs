using VitaApi.DTOs.Notificacoes;

namespace VitaApi.Interfaces;

public interface INotificacaoService
{
    Task<List<NotificacaoDto>> GetUltimasAsync();
}