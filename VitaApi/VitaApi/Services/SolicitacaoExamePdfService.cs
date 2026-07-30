using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.Pdf;
using VitaApi.DTOs.SolicitacoesExames;
using VitaApi.Interfaces;
using VitaApi.Services.Pdf;

namespace VitaApi.Services;

public class SolicitacaoExamePdfService
{
    private readonly IConfiguracaoClinicaService _configuracaoService;

    public SolicitacaoExamePdfService(
        IConfiguracaoClinicaService configuracaoService
    )
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<byte[]> GerarPdfAsync(
        SolicitacaoExameResponseDto solicitacao
    )
    {
        QuestPDF.Settings.License =
            LicenseType.Community;

        var config =
            await _configuracaoService.GetAsync();

        var header = new PdfHeaderDto
        {
            Titulo = "Solicitação de Exames",

            DocumentoId = solicitacao.Id,

            PacienteNome = solicitacao.PacienteNome,

            MedicoNome = solicitacao.MedicoNome,

            MedicoCrm = solicitacao.MedicoCrm,

            Especialidade =
                solicitacao.MedicoEspecialidade,

            TelefoneMedico =
                solicitacao.MedicoTelefone,

            Cidade =
                solicitacao.MedicoCidade,

            Estado =
                solicitacao.MedicoEstado,

            EnderecoProfissional =
                solicitacao.MedicoEnderecoProfissional,

            Assinatura =
                solicitacao.MedicoAssinatura,

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

        var exames =
            solicitacao.ExamesSolicitados
                .Split(
                    new[] { "\r\n", "\n", "," },
                    StringSplitOptions.RemoveEmptyEntries
                )
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(38);

                page.DefaultTextStyle(x =>
                    x.FontSize(10)
                     .FontColor(Colors.Grey.Darken3));

                page.Header()
                    .Element(c =>
                        VitaPdfDocument.Header(
                            c,
                            header
                        ));

                page.Content()
                    .PaddingTop(24)
                    .Column(column =>
                    {
                        column.Spacing(18);

                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Text(text =>
                                    {
                                        text.Span("Data: ").Bold();
                                        text.Span(
                                            solicitacao.DataSolicitacao
                                                .ToString("dd/MM/yyyy")
                                        );
                                    });

                                row.RelativeItem()
                                    .AlignRight()
                                    .Text(text =>
                                    {
                                        text.Span("Consulta: ").Bold();
                                        text.Span(
                                            $"#{solicitacao.ConsultaId}"
                                        );
                                    });
                            });

                        column.Item()
                            .PaddingTop(10)
                            .Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item()
                                        .Text("EXAMES SOLICITADOS")
                                        .FontSize(12)
                                        .Bold();

                                    col.Item()
                                        .PaddingTop(8)
                                        .Column(lista =>
                                        {
                                            lista.Spacing(6);

                                            foreach (var exame in exames.Take(18))
                                            {
                                                lista.Item()
                                                    .Text($"○ {exame}")
                                                    .FontSize(10);
                                            }
                                        });
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item()
                                        .Text("CONTINUAÇÃO")
                                        .FontSize(12)
                                        .Bold();

                                    col.Item()
                                        .PaddingTop(8)
                                        .Column(lista =>
                                        {
                                            lista.Spacing(6);

                                            foreach (var exame in exames.Skip(18).Take(18))
                                            {
                                                lista.Item()
                                                    .Text($"○ {exame}")
                                                    .FontSize(10);
                                            }
                                        });
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item()
                                        .Text("OUTROS")
                                        .FontSize(12)
                                        .Bold();

                                    col.Item()
                                        .PaddingTop(8)
                                        .Column(lista =>
                                        {
                                            lista.Spacing(6);

                                            foreach (var exame in exames.Skip(36))
                                            {
                                                lista.Item()
                                                    .Text($"○ {exame}")
                                                    .FontSize(10);
                                            }

                                            if (!exames.Skip(36).Any())
                                            {
                                                lista.Item().Text("○ __________________________").FontSize(10);
                                                lista.Item().Text("○ __________________________").FontSize(10);
                                                lista.Item().Text("○ __________________________").FontSize(10);
                                            }
                                        });
                                });
                            });

                        column.Item()
                            .PaddingTop(18)
                            .Text("Justificativa clínica")
                            .FontSize(12)
                            .Bold();

                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(12)
                            .MinHeight(80)
                            .Text(
                                string.IsNullOrWhiteSpace(
                                    solicitacao.Justificativa
                                )
                                    ? "Sem justificativa informada."
                                    : solicitacao.Justificativa
                            )
                            .LineHeight(1.4f);

                        column.Item()
                            .PaddingTop(55)
                            .Element(c =>
                                VitaPdfDocument.Signature(
                                    c,
                                    header
                                ));
                    });

                page.Footer()
                    .Element(c =>
                        VitaPdfDocument.Footer(
                            c,
                            header
                        ));
            });
        })
        .GeneratePdf();
    }
}