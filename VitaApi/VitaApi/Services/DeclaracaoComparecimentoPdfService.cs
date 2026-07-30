using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.DeclaracoesComparecimento;
using VitaApi.DTOs.Pdf;
using VitaApi.Interfaces;
using VitaApi.Services.Pdf;

namespace VitaApi.Services;

public class DeclaracaoComparecimentoPdfService
{
    private readonly IConfiguracaoClinicaService _configuracaoService;

    public DeclaracaoComparecimentoPdfService(
        IConfiguracaoClinicaService configuracaoService
    )
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<byte[]> GerarPdfAsync(
        DeclaracaoComparecimentoResponseDto declaracao
    )
    {
        QuestPDF.Settings.License =
            LicenseType.Community;

        var config =
            await _configuracaoService.GetAsync();

        var header = new PdfHeaderDto
        {
            Titulo = "Declaração de Comparecimento",

            DocumentoId = declaracao.Id,

            PacienteNome = declaracao.PacienteNome,

            MedicoNome = declaracao.MedicoNome,

            MedicoCrm = declaracao.MedicoCrm,

            Especialidade =
                declaracao.MedicoEspecialidade,

            TelefoneMedico =
                declaracao.MedicoTelefone,

            Cidade =
                declaracao.MedicoCidade,

            Estado =
                declaracao.MedicoEstado,

            EnderecoProfissional =
                declaracao.MedicoEnderecoProfissional,

            Assinatura =
                declaracao.MedicoAssinatura,

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
                        column.Spacing(24);

                        column.Item()
                            .Text(text =>
                            {
                                text.DefaultTextStyle(style =>
                                    style.FontSize(13)
                                         .LineHeight(1.6f)
                                );

                                text.Span(
                                    "Declaro para os devidos fins que o(a) paciente "
                                );

                                text.Span(
                                    declaracao.PacienteNome
                                )
                                .Bold();

                                text.Span(
                                    $" compareceu à {header.NomeClinica} na data de "
                                );

                                text.Span(
                                    declaracao.DataConsulta
                                        .ToString("dd/MM/yyyy")
                                )
                                .Bold();

                                text.Span(
                                    " para atendimento médico."
                                );
                            });

                        if (!string.IsNullOrWhiteSpace(
                            declaracao.Observacoes
                        ))
                        {
                            column.Item()
                                .Text("Observações")
                                .FontSize(16)
                                .Bold()
                                .FontColor(
                                    Colors.Blue.Medium
                                );

                            column.Item()
                                .Border(1)
                                .BorderColor(
                                    Colors.Grey.Lighten2
                                )
                                .Padding(14)
                                .MinHeight(90)
                                .Text(
                                    declaracao.Observacoes
                                )
                                .LineHeight(1.4f);
                        }

                        column.Item()
                            .AlignRight()
                            .Text(
                                $"Emitido em {declaracao.DataEmissao:dd/MM/yyyy HH:mm}"
                            )
                            .FontSize(10)
                            .FontColor(
                                Colors.Grey.Darken1
                            );

                        column.Item()
                            .PaddingTop(60)
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