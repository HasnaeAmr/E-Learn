using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
namespace Elearn.Pages.Course
{
    public class CertificateModel : PageModel
    {


        public int Score { get; set; }
        public string UserName { get; set; }

        public CertificateModel()
        {
            Score = TestModel.Score; // Assuming QcmModel.Score is a static property
        }

        public IActionResult OnGet()
        {
            // Retrieve username from session
            UserName = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(UserName))
            {
                // Redirect to login page if session is empty
                return RedirectToPage("/Login");
            }

            return Page();
        }

        // Action to download the certificate as PDF
        public IActionResult OnGetDownloadCertificate()
        {
            // Ensure username is retrieved from session
            UserName = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(UserName))
            {
                // Redirect to login page if session is empty
                return RedirectToPage("/Login");
            }

            using (var memoryStream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                var graphics = XGraphics.FromPdfPage(page);

                // Load the Canva design as an image (PNG/JPEG)
                string templateImagePath = "C:\\Users\\hp\\Downloads\\certif.png";
                XImage certificateImage = XImage.FromFile(templateImagePath);

                    // Draw the image as the background
                    graphics.DrawImage(certificateImage, 0, 0, page.Width, page.Height);

                    // Define a font for the name and score
                    var font = new XFont("Times New Roman", 24);
                    var scoreFont = new XFont("Times New Roman", 18);

                    // Overlay the user's name
                    graphics.DrawString(UserName, font, XBrushes.Black, new XPoint(400, 230)); // Adjust position as needed

                    // Overlay the user's score or other dynamic information
                    graphics.DrawString($"Score: {Score}/100", scoreFont, XBrushes.Black, new XPoint(400, 280)); // Adjust position as needed

                    // Save the PDF to memory
                    document.Save(memoryStream, false);
                    memoryStream.Position = 0;

                    // Return the generated PDF for download
                    return File(memoryStream.ToArray(), "application/pdf", "certificat_reussite.pdf");
                }
            }
        }
    }