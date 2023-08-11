using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Security;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //To load an existing file. 
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreatePDFDocument()
        {
            var finalStream = new MemoryStream();
            string base64 = "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";
            try
            {
                PdfDocument document = new PdfDocument();
                PdfPage pdfPage = document.Pages.Add();
                X509Certificate2 certificate2 = new X509Certificate2(Convert.FromBase64String(base64), "moorthy", X509KeyStorageFlags.DefaultKeySet);
                PdfCertificate pdfCertificate = new PdfCertificate(certificate2);
                PdfSignature signature = new PdfSignature(document, pdfPage, pdfCertificate, "Signature");
                signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
                signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;
                signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamp.entrust.net/TSS/RFC3161sha2TS"));
                document.Save(finalStream);
                document.Close(true);
                finalStream.Position = 0;
            }
            catch (Exception e)
            {
                finalStream = new MemoryStream();
                PdfDocument document = new PdfDocument();
                PdfPage pdfPage = document.Pages.Add();
                PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
                pdfPage.Graphics.DrawString(e.Message, font, PdfBrushes.Black, new Syncfusion.Drawing.RectangleF(0, 0, pdfPage.Graphics.ClientSize.Width, pdfPage.Graphics.ClientSize.Height));
                pdfPage.Graphics.DrawString(e.StackTrace, font, PdfBrushes.Black, new Syncfusion.Drawing.RectangleF(0, 0, pdfPage.Graphics.ClientSize.Width, pdfPage.Graphics.ClientSize.Height));
                document.Save(finalStream);
                document.Close(true);
                finalStream.Position = 0;
            }

            FileStreamResult fileStreamResult = new FileStreamResult(finalStream, "application/pdf");
            fileStreamResult.FileDownloadName = "SignatureSignAzureFunction.pdf";
            return fileStreamResult;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}