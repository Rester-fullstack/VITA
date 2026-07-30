using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IProntuarioRepository
{
    Task<Paciente?> GetPacienteCompletoAsync(
        int pacienteId
    );
}