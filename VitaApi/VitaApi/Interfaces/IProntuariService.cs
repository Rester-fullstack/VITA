using VitaApi.DTOs.Prontuarios;

namespace VitaApi.Interfaces;

public interface IProntuarioService
{
    Task<ProntuarioPacienteDto?> GetPacienteAsync(
        int pacienteId
    );
}