using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.Pdf;
using VitaApi.DTOs.Prontuarios;
using VitaApi.Interfaces;
using VitaApi.Services.Pdf;

namespace VitaApi.Services;

public class ProntuarioPdfService
{
    private readonly IConfiguracaoClinicaService _configuracaoService;

    public ProntuarioPdfService(
        IConfiguracaoClinicaService configuracaoService
    )
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<byte[]> GerarPdfAsync(
        ProntuarioPacienteDto prontuario
    )
    {
        QuestPDF.Settings.License =
            LicenseType.Community;

        var primeiraConsulta =
            prontuario.Consultas.FirstOrDefault();

        var config =
            await _configuracaoService.GetAsync();

        var header = new PdfHeaderDto
        {
            Titulo = "Prontuário do Paciente",

            DocumentoId = prontuario.PacienteId,

            PacienteNome = prontuario.PacienteNome,

            MedicoNome =
                primeiraConsulta?.MedicoNome ?? "",

            MedicoCrm =
                primeiraConsulta?.MedicoCrm ?? "",

            Especialidade =
                primeiraConsulta?.MedicoEspecialidade ?? "",

            TelefoneMedico =
                primeiraConsulta?.MedicoTelefone ?? "",

            Cidade =
                primeiraConsulta?.MedicoCidade ?? "",

            Estado =
                primeiraConsulta?.MedicoEstado ?? "",

            EnderecoProfissional =
                primeiraConsulta?.MedicoEnderecoProfissional ?? "",

            Assinatura =
                primeiraConsulta?.MedicoAssinatura ?? "",

            NomeClinica =
                config.NomePlataforma,

            EmailClinica =
                config.EmailSuporte,

            TelefoneClinica =
                config.TelefoneSuporte,

            Rodape =
                config.RodapePdf,

            Subtitulo =
                "Sistema Inteligente de Gestão Clínica"
        };

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(35);

                page.DefaultTextStyle(x =>
                    x.FontSize(11));

                page.Header()
                    .Element(c =>
                        VitaPdfDocument.Header(
                            c,
                            header
                        ));

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(18);

                        InformacoesPaciente(
                            column,
                            prontuario
                        );

                        foreach (var consulta in prontuario.Consultas)
                        {
                            Consulta(
                                column,
                                consulta
                            );
                        }
                    });

                page.Footer()
                    .Element(c =>
                        VitaPdfDocument.Footer(
                            c,
                            header
                        ));
            });

        }).GeneratePdf();
    }

    private static void InformacoesPaciente(
     ColumnDescriptor column,
     ProntuarioPacienteDto paciente
 )
    {
        column.Item()
            .Text("Dados do Paciente")
            .FontSize(18)
            .Bold()
            .FontColor(
                Colors.Blue.Medium
            );

        column.Item()
            .Border(1)
            .BorderColor(
                Colors.Grey.Lighten2
            )
            .Padding(15)
            .Column(info =>
            {
                info.Spacing(6);

                info.Item().Text(
                    $"Nome: {paciente.PacienteNome}"
                );

                info.Item().Text(
                    $"CPF: {(
                        string.IsNullOrWhiteSpace(paciente.CPF)
                            ? "Não informado"
                            : paciente.CPF
                    )}"
                );

                info.Item().Text(
                    $"Telefone: {(
                        string.IsNullOrWhiteSpace(paciente.Telefone)
                            ? "Não informado"
                            : paciente.Telefone
                    )}"
                );

                info.Item().Text(
                    $"Nascimento: {(
                        paciente.DataNascimento.HasValue
                            ? paciente.DataNascimento.Value.ToString("dd/MM/yyyy")
                            : "Não informado"
                    )}"
                );
            });
    }

    private static void Consulta(
    ColumnDescriptor column,
    ProntuarioConsultaDto consulta
)
    {
        column.Item()
            .PaddingTop(20)
            .Text(
                $"Consulta - {consulta.DataConsulta:dd/MM/yyyy HH:mm}"
            )
            .FontSize(17)
            .Bold();

        column.Item()
            .Border(1)
            .BorderColor(
                Colors.Grey.Lighten2
            )
            .Padding(15)
            .Column(c =>
            {
                c.Spacing(10);

                c.Item().Text(
                    $"Médico: {(
                        string.IsNullOrWhiteSpace(consulta.MedicoNome)
                            ? "Não informado"
                            : consulta.MedicoNome
                    )}"
                );

                c.Item().Text(
                    $"CRM: {(
                        string.IsNullOrWhiteSpace(consulta.MedicoCrm)
                            ? "Não informado"
                            : consulta.MedicoCrm
                    )}"
                );

                if (!string.IsNullOrWhiteSpace(
                    consulta.MedicoEspecialidade
                ))
                {
                    c.Item().Text(
                        $"Especialidade: {consulta.MedicoEspecialidade}"
                    );
                }

                c.Item().Text(
                    $"Status: {(
                        string.IsNullOrWhiteSpace(consulta.Status)
                            ? "Não informado"
                            : consulta.Status
                    )}"
                );

                if (!string.IsNullOrWhiteSpace(
                    consulta.Observacoes
                ))
                {
                    c.Item().Text(
                        $"Observações: {consulta.Observacoes}"
                    );
                }

                Lista(
                    c,
                    "Histórico Clínico",
                    consulta.Historicos
                );

                Lista(
                    c,
                    "Receitas",
                    consulta.Receitas
                );

                Lista(
                    c,
                    "Atestados",
                    consulta.Atestados
                );

                Lista(
                    c,
                    "Declarações",
                    consulta.Declaracoes
                );

                Lista(
                    c,
                    "Solicitações de Exames",
                    consulta.SolicitacoesExames
                );

                Lista(
                    c,
                    "Exames",
                    consulta.Exames
                );
            });
    }

    private static void Lista(
        ColumnDescriptor column,
        string titulo,
        List<string> itens
    )
    {
        if (itens.Count == 0)
            return;

        column.Item()
            .PaddingTop(8)
            .Text(titulo)
            .Bold()
            .FontColor(
                Colors.Blue.Medium
            );

        foreach (var item in itens)
        {
            column.Item()
                .PaddingLeft(12)
                .Text($"• {item}");
        }
    }
}