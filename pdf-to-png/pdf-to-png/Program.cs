using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace pdf_to_png
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the pdf file path");
            var pdfFilePath = Console.ReadLine();
            Console.WriteLine("Please enter output folder path");
            var outputDirectory = Console.ReadLine();
            SplitPdfAndConvertToImages(pdfFilePath, outputDirectory);
            Console.WriteLine("Completed, please check output directory.");
        }
        private static void SplitPdfAndConvertToImages(string pdfFilePath, string outputDirectory)
        {
            // Load the PDF document
            PdfDocument document = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Import);

            // Iterate through all the pages in the PDF
            for (int i = 0; i < document.PageCount; i++)
            {
                Console.WriteLine($"Processing page {i + 1}/{document.PageCount}");

                // Create a memory stream for the individual page
                using (var singlePageStream = new MemoryStream())
                {
                    // Create a temporary PDF file for each page
                    using (PdfDocument singlePageDocument = new PdfDocument())
                    {
                        singlePageDocument.AddPage(document.Pages[i]);

                        // Save the individual page to the memory stream
                        singlePageDocument.Save(singlePageStream);

                        // Convert the PDF page to PNG
                        var pdfBytes = singlePageStream.ToArray();
                        byte[] pngBytes = Freeware.Pdf2Png.Convert(pdfBytes, 1, 200);
                        var imagePath = Path.Combine(outputDirectory, $"page_{i + 1}.png");

                        // Save png image to ouput directory
                        System.IO.File.WriteAllBytes(imagePath + ".png", pngBytes);
                    }
                }
            }
        }
    }
}
