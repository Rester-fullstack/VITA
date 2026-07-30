using VitaApi.DTOs.Timeline;

namespace VitaApi.Interfaces;

public interface ITimelineService
{
    Task<List<TimelineItemDto>> GetTimelinePacienteAsync(
        int pacienteId
    );
}