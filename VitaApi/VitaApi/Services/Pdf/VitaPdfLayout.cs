using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VitaApi.Services.Pdf;

public static class VitaPdfLayout
{
    public static void Header(
        IContainer container,
        string titulo,
        string pacienteNome,
        string medicoNome,
        string medicoCrm,
        int documentoId
    )
    {
        container.Column(header =>
        {
            header.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item()
                        .Text("VITA")
                        .FontSize(34)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    col.Item()
                        .Text("Clínica Médica")
                        .FontSize(13)
                        .FontColor(Colors.Grey.Darken1);
                });

                row.ConstantItem(230).AlignRight().Column(col =>
                {
                    col.Item()
                        .Text(medicoNome)
                        .FontSize(13)
                        .Bold()
                        .AlignRight();

                    col.Item()
                        .Text($"CRM: {medicoCrm}")
                        .FontSize(10)
                        .AlignRight();

                    col.Item()
                        .PaddingTop(4)
                        .Text($"Documento Nº {documentoId:000000}")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1)
                        .AlignRight();
                });
            });

            header.Item()
                .PaddingTop(18)
                .Text("Paciente")
                .FontSize(10)
                .FontColor(Colors.Grey.Darken1);

            header.Item()
                .Text(pacienteNome)
                .FontSize(13)
                .Bold();

            header.Item()
                .PaddingTop(14)
                .LineHorizontal(1)
                .LineColor(Colors.Grey.Darken2);

            header.Item()
                .PaddingTop(12)
                .Text(titulo)
                .FontSize(22)
                .Bold()
                .FontColor(Colors.Grey.Darken4);
        });
    }

    public static void Signature(
        IContainer container,
        string medicoNome,
        string medicoCrm
    )
    {
        container.AlignCenter().Column(signature =>
        {
            signature.Item()
                .Width(270)
                .LineHorizontal(1)
                .LineColor(Colors.Black);

            signature.Item()
                .PaddingTop(6)
                .Text(medicoNome)
                .FontSize(13)
                .Bold()
                .AlignCenter();

            signature.Item()
                .Text($"CRM: {medicoCrm}")
                .FontSize(11)
                .AlignCenter();
        });
    }

    public static void Footer(IContainer container)
    {
        container.Column(footer =>
        {
            footer.Item()
                .LineHorizontal(1)
                .LineColor(Colors.Grey.Lighten2);

            footer.Item()
                .PaddingTop(6)
                .AlignCenter()
                .Text("VITA • Sistema Inteligente para Gestão Clínica")
                .FontSize(9)
                .Bold()
                .FontColor(Colors.Grey.Darken2);

            footer.Item()
                .PaddingTop(2)
                .AlignCenter()
                .Text($"Documento emitido eletronicamente em {DateTime.Now:dd/MM/yyyy HH:mm}")
                .FontSize(8)
                .FontColor(Colors.Grey.Darken1);

            footer.Item()
                .PaddingTop(2)
                .AlignCenter()
                .Text("Autenticidade garantida pelo sistema VITA.")
                .FontSize(8)
                .FontColor(Colors.Grey.Darken1);
        });
    }
}