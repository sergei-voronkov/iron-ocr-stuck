namespace iron_ocr_stuck
{
    using IronOcr;
    using System.Diagnostics;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            TesseractConfiguration configuration = new()
            {
                PageSegmentationMode = TesseractPageSegmentationMode.SparseText
            };

            Installation.LicenseKey = Environment.GetEnvironmentVariable(
                "carat__local__Tesseract__LicenseKey");

            IronTesseract ironTesseract = new(
                configuration)
            {
                Language = OcrLanguage.EnglishBest
            };

            foreach (string path in Directory.EnumerateFiles("good", "*.png"))
            {
                await RecognizeImageAsync(
                    ironTesseract,
                    path);
            }


            foreach (string path in Directory.EnumerateFiles("stuck", "*.png"))
            {
                await RecognizeImageAsync(
                    ironTesseract,
                    path);
            }
        }

        private static async Task<OcrInput> RecognizeImageAsync(IronTesseract ironTesseract, string path)
        {
            Stopwatch sw = Stopwatch.StartNew();

            OcrInput ocrInput = new(
                path);

            Console.WriteLine(
                $"Recognizing: {path}...");

            OcrResult tessResult = await ironTesseract.ReadAsync(
                ocrInput);

            Console.WriteLine(
                $"Done. Path: {path}. " +
                $"Confidence: {tessResult.Confidence}. " +
                $"Time spent: {sw.Elapsed}.");

            return ocrInput;
        }
    }
}