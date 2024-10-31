using iTextSharp.text;
using iTextSharp.text.pdf;
using WebApp.Models.Account;

namespace WebApp.Helper;

public static class FileExtension
{
    public static Document GenerateReportDocument(AccountViewModel reportBody, FileStream document)
    {
        var pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
        PdfWriter.GetInstance(pdfDoc, document);

        pdfDoc.Open();
        Paragraph paragraphId = new Paragraph(reportBody.Id.ToString());
        pdfDoc.Add(paragraphId);
        Paragraph paragraphName = new Paragraph(reportBody.FirstName + reportBody.LastName);
        pdfDoc.Add(paragraphName);
        Paragraph paragraphBalance = new Paragraph(reportBody.Balance.ToString());
        pdfDoc.Add(paragraphBalance);

        string transactions = string.Empty;
        if(reportBody.Transactions != null)
        {
            foreach (var item in reportBody.Transactions)
            {
                transactions += $"" +
                    $" | {item.OperationType}" +
                    $" | {item.Amount}" +
                    $" | {item.DestinationAccountId}" +
                    $" | {item.DestinationFirstName} {item.DestinationLastName} |\n";
            }
        }
        Paragraph paragraphTransactions = new Paragraph(transactions);
        pdfDoc.Add(paragraphTransactions);

        return pdfDoc;
    }

    public static void GeneratePDFFromView()
    {

    }
}
