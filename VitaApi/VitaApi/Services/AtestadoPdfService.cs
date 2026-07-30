using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.Atestados;
using VitaApi.DTOs.Pdf;
using VitaApi.Interfaces;
using VitaApi.Services.Pdf;

namespace VitaApi.Services;

public class AtestadoPdfService
{
    private readonly IConfiguracaoClinicaService _configuracaoService;

    public AtestadoPdfService(
        IConfiguracaoClinicaService configuracaoService
    )
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<byte[]> GerarPdfAsync(
        AtestadoResponseDto atestado
    )
    {
        QuestPDF.Settings.License =
            LicenseType.Community;

        var config =
            await _configuracaoService.GetAsync();

        var header = new PdfHeaderDto
        {
            Titulo = "Atestado Médico",

            DocumentoId = atestado.Id,

            PacienteNome = atestado.PacienteNome,

            MedicoNome = atestado.MedicoNome,

            MedicoCrm = atestado.MedicoCrm,

            Especialidade = atestado.MedicoEspecialidade,

            TelefoneMedico = atestado.MedicoTelefone,

            Cidade = atestado.MedicoCidade,

            Estado = atestado.MedicoEstado,

            EnderecoProfissional =
                atestado.MedicoEnderecoProfissional,

            Assinatura = atestado.MedicoAssinatura,

            NomeClinica = config.NomePlataforma,

            EmailClinica = config.EmailSuporte,

            TelefoneClinica = config.TelefoneSuporte,

            Rodape = config.RodapePdf,

            Subtitulo =
                "Sistema Inteligente de Gestão Clínica"
        };

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(38);

                page.DefaultTextStyle(x =>
                    x.FontSize(11)
                     .FontColor(Colors.Grey.Darken3)
                );

                page.Header()
                    .Element(c =>
                        VitaPdfDocument.Header(
                            c,
                            header
                        )
                    );

                page.Content()
                    .PaddingTop(35)
                    .Column(column =>
                    {
                        column.Spacing(22);

                        column.Item()
                            .Text(text =>
                            {
                                text.DefaultTextStyle(style =>
                                    style.FontSize(13)
                                         .LineHeight(1.5f)
                                );

                                text.Span(
                                    "Atesto para os devidos fins que o(a) paciente "
                                );

                                text.Span(
                                    atestado.PacienteNome
                                )
                                .Bold();

                                text.Span(
                                    " necessita de afastamento de suas atividades por "
                                );

                                text.Span(
                                    $"{atestado.DiasAfastamento} dia(s)"
                                )
                                .Bold();

                                text.Span(
                                    ", a partir de "
                                );

                                text.Span(
                                    atestado.DataInicio
                                        .ToString("dd/MM/yyyy")
                                )
                                .Bold();

                                text.Span(".");
                            });

                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .Text("Motivo")
                                            .FontSize(10)
                                            .FontColor(
                                                Colors.Grey.Darken1
                                            );

                                        col.Item()
                                            .Text(
                                                string.IsNullOrWhiteSpace(
                                                    atestado.Motivo
                                                )
                                                    ? "Não informado"
                                                    : atestado.Motivo
                                            )
                                            .FontSize(13)
                                            .Bold();
                                    });

                                row.ConstantItem(150)
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .Text("CID")
                                            .FontSize(10)
                                            .FontColor(
                                                Colors.Grey.Darken1
                                            );

                                        col.Item()
                                            .Text(
                                                string.IsNullOrWhiteSpace(
                                                    atestado.Cid
                                                )
                                                    ? "Não informado"
                                                    : atestado.Cid
                                            )
                                            .FontSize(13)
                                            .Bold();
                                    });
                            });

                        column.Item()
                            .Text("Observações")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .Border(1)
                            .BorderColor(
                                Colors.Grey.Lighten2
                            )
                            .Padding(14)
                            .MinHeight(110)
                            .Text(
                                string.IsNullOrWhiteSpace(
                                    atestado.Observacoes
                                )
                                    ? "Sem observações."
                                    : atestado.Observacoes
                            )
                            .LineHeight(1.4f);

                        column.Item()
                            .AlignRight()
                            .Text(
                                $"Emitido em {atestado.DataEmissao:dd/MM/yyyy}"
                            )
                            .FontSize(10)
                            .FontColor(
                                Colors.Grey.Darken1
                            );

                        column.Item()
                            .PaddingTop(55)
                            .Element(c =>
                                VitaPdfDocument.Signature(
                                    c,
                                    header
                                )
                            );
                    });

                page.Footer()
                    .Element(c =>
                        VitaPdfDocument.Footer(
                            c,
                            header
                        )
                    );
            });
        })
        .GeneratePdf();
    }
}