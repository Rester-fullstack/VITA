using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitaApi.DTOs.Pdf;

namespace VitaApi.Services.Pdf;

public static class VitaPdfDocument
{
    public static void Header(
     IContainer container,
     PdfHeaderDto header
 )
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item()
                        .Text(header.NomeClinica)
                        .FontSize(34)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    left.Item()
                        .Text(header.Subtitulo)
                        .FontSize(12)
                        .FontColor(Colors.Grey.Darken2);

                    if (!string.IsNullOrWhiteSpace(header.EnderecoClinica))
                        left.Item()
                            .Text(header.EnderecoClinica)
                            .FontSize(10);

                    if (!string.IsNullOrWhiteSpace(header.TelefoneClinica))
                        left.Item()
                            .Text($"Telefone: {header.TelefoneClinica}")
                            .FontSize(10);

                    if (!string.IsNullOrWhiteSpace(header.EmailClinica))
                        left.Item()
                            .Text(header.EmailClinica)
                            .FontSize(10);
                });

                row.ConstantItem(240)
                    .AlignRight()
                    .Column(right =>
                    {
                        right.Item()
                            .Text(header.MedicoNome)
                            .FontSize(13)
                            .Bold()
                            .AlignRight();

                        if (!string.IsNullOrWhiteSpace(header.Especialidade))
                            right.Item()
                                .Text(header.Especialidade)
                                .FontSize(10)
                                .AlignRight();

                        right.Item()
                            .Text($"CRM: {header.MedicoCrm}")
                            .FontSize(10)
                            .AlignRight();

                        if (!string.IsNullOrWhiteSpace(header.EnderecoProfissional))
                        {
                            right.Item()
                                .Text(header.EnderecoProfissional)
                                .FontSize(10)
                                .AlignRight();
                        }

                        if (!string.IsNullOrWhiteSpace(header.Cidade))
                        {
                            var local =
                                string.IsNullOrWhiteSpace(header.Estado)
                                    ? header.Cidade
                                    : $"{header.Cidade} - {header.Estado}";

                            right.Item()
                                .Text(local)
                                .FontSize(10)
                                .AlignRight();
                        }

                        right.Item()
                         .PaddingTop(5)
                         .Text($"Documento Nº {header.DocumentoId:000000}")
                         .FontSize(9)
                         .FontColor(Colors.Grey.Darken2)
                         .AlignRight();

                        right.Item()
                            .Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken2)
                            .AlignRight();
                    });
            });

            column.Item()
                .PaddingTop(18)
                .Text("Paciente")
                .FontSize(10)
                .FontColor(Colors.Grey.Darken2);

            column.Item()
                .Text(header.PacienteNome)
                .FontSize(13)
                .Bold();

            column.Item()
                .PaddingTop(14)
                .LineHorizontal(1);

            column.Item()
                .PaddingTop(12)
                .Text(header.Titulo)
                .FontSize(22)
                .Bold()
                .FontColor(Colors.Grey.Darken4);
        });
    }


    public static void Signature(
     IContainer container,
     PdfHeaderDto header
 )
    {
        container.AlignCenter().Column(signature =>
        {
            signature.Item()
                .Width(280)
                .LineHorizontal(1);

            if (!string.IsNullOrWhiteSpace(header.Assinatura))
            {
                signature.Item()
                    .PaddingTop(8)
                    .Text(header.Assinatura)
                    .FontSize(11)
                    .Italic()
                    .AlignCenter();
            }

            signature.Item()
                .PaddingTop(6)
                .Text(header.MedicoNome)
                .FontSize(13)
                .Bold()
                .AlignCenter();

            if (!string.IsNullOrWhiteSpace(header.Especialidade))
            {
                signature.Item()
                    .Text(header.Especialidade)
                    .FontSize(11)
                    .AlignCenter();
            }

            signature.Item()
                .Text($"CRM: {header.MedicoCrm}")
                .FontSize(11)
                .AlignCenter();
        });
    }
    public static void Footer(
        IContainer container,
        PdfHeaderDto header
    )
    {
        container.Column(footer =>
        {
            footer.Item()
                .LineHorizontal(1);

            footer.Item()
                .PaddingTop(6)
                .AlignCenter()
                .Text(header.NomeClinica)
                .FontSize(9)
                .Bold();

            if (!string.IsNullOrWhiteSpace(header.Rodape))
            {
                footer.Item()
                    .AlignCenter()
                    .Text(header.Rodape)
                    .FontSize(8);
            }
            if (!string.IsNullOrWhiteSpace(header.TelefoneClinica))
            {
                footer.Item()
                    .AlignCenter()
                    .Text(header.TelefoneClinica)
                    .FontSize(8);
            }

            if (!string.IsNullOrWhiteSpace(header.EmailClinica))
            {
                footer.Item()
                    .AlignCenter()
                    .Text(header.EmailClinica)
                    .FontSize(8);
            }

            footer.Item()
                .AlignCenter()
                .Text($"Emitido em {DateTime.Now:dd/MM/yyyy HH:mm}")
                .FontSize(8)
                .FontColor(Colors.Grey.Darken2);
        });
    }
}