using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.Pdf;
using VitaApi.DTOs.Receitas;
using VitaApi.Interfaces;
using VitaApi.Services.Pdf;

namespace VitaApi.Services;

public class ReceitaPdfService
{
    private readonly IConfiguracaoClinicaService _configuracaoService;

    public ReceitaPdfService(
        IConfiguracaoClinicaService configuracaoService
    )
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<byte[]> GerarPdfAsync(
        ReceitaResponseDto receita
    )
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var config =
            await _configuracaoService.GetAsync();

        var header = new PdfHeaderDto
        {
            Titulo = "Receita Médica",

            DocumentoId = receita.Id,

            PacienteNome = receita.PacienteNome,

            MedicoNome = receita.MedicoNome,
            MedicoCrm = receita.MedicoCrm,
            Especialidade = receita.MedicoEspecialidade,
            TelefoneMedico = receita.MedicoTelefone,
            Cidade = receita.MedicoCidade,
            Estado = receita.MedicoEstado,
            EnderecoProfissional = receita.MedicoEnderecoProfissional,
            Assinatura = receita.MedicoAssinatura,

            NomeClinica = config.NomePlataforma,
            EmailClinica = config.EmailSuporte,
            TelefoneClinica = config.TelefoneSuporte,
            Rodape = config.RodapePdf,

            Subtitulo = "Sistema Inteligente de Gestão Clínica"
        };

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(38);

                page.DefaultTextStyle(x =>
                    x.FontSize(11)
                     .FontColor(Colors.Grey.Darken3));

                page.Header()
                    .Element(c =>
                        VitaPdfDocument.Header(c, header));

                page.Content()
                    .PaddingTop(28)
                    .Column(column =>
                    {
                        column.Spacing(18);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("Data da receita: ").Bold();
                                text.Span(
                                    receita.DataReceita.ToString("dd/MM/yyyy")
                                );
                            });

                            row.RelativeItem()
                                .AlignRight()
                                .Text(text =>
                                {
                                    text.Span("Documento: ").Bold();
                                    text.Span($"#{receita.Id:000000}");
                                });
                        });

                        column.Item()
                            .Text("Prescrição")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(18)
                            .MinHeight(200)
                            .Column(prescricao =>
                            {
                                prescricao.Spacing(12);

                                prescricao.Item()
                                    .Text(receita.Medicamento)
                                    .FontSize(16)
                                    .Bold();

                                prescricao.Item()
                                    .Text($"Dosagem: {receita.Dosagem}");

                                prescricao.Item()
                                    .Text($"Frequência: {receita.Frequencia}");

                                prescricao.Item()
                                    .Text($"Duração: {receita.Duracao}");
                            });

                        column.Item()
                            .Text("Observações")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(14)
                            .MinHeight(90)
                            .Text(
                                string.IsNullOrWhiteSpace(receita.Observacoes)
                                    ? "Sem observações."
                                    : receita.Observacoes
                            )
                            .LineHeight(1.4f);

                        column.Item()
                            .PaddingTop(55)
                            .Element(c =>
                                VitaPdfDocument.Signature(c, header));
                    });

                page.Footer()
                    .Element(c =>
                        VitaPdfDocument.Footer(c, header));
            });
        })
        .GeneratePdf();
    }
}