using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;

namespace AzureVisionCognitiveServicesDemo.ComputerVision
{
    public static class ComputerVisionAnalyzer
    {
        // Analyze a local image
        public static Task<ImageAnalysis> AnalyzeLocalAsync(ComputerVisionClient computerVision,
            string imagePath, IList<VisualFeatureTypes> features)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                throw new FileNotFoundException(imagePath);
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                return computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
            }
        }

        // Analyze a remote image
        public static Task<ImageAnalysis> AnalyzeRemoteAsync(
            ComputerVisionClient computerVision, string imageUrl, IList<VisualFeatureTypes> features)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                throw new FileNotFoundException(imageUrl);
            }

            return computerVision.AnalyzeImageAsync(imageUrl, features);
        }

        // Recognize text from a local image
        public static async Task<TextOperationResult> ExtractLocalTextAsync(
            ComputerVisionClient computerVision, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                throw new FileNotFoundException(imagePath);
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                // Start the async process to recognize the text
                RecognizeTextInStreamHeaders textHeaders =
                    await computerVision.RecognizeTextInStreamAsync(
                        imageStream, Variables.TextRecognitionMode);

                return await GetTextAsync(computerVision, textHeaders.OperationLocation);
            }
        }

        // Recognize text from a remote image
        public static async Task<TextOperationResult> ExtractRemoteTextAsync(
            ComputerVisionClient computerVision, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                throw new FileNotFoundException(imageUrl);
            }

            // Start the async process to recognize the text
            RecognizeTextHeaders textHeaders =
                await computerVision.RecognizeTextAsync(
                    imageUrl, Variables.TextRecognitionMode);

            return await GetTextAsync(computerVision, textHeaders.OperationLocation);
        }

        // Retrieve the recognized text
        private static async Task<TextOperationResult> GetTextAsync(
            ComputerVisionClient computerVision, string operationLocation)
        {
            // Retrieve the URI where the recognized text will be
            // stored from the Operation-Location header
            string operationId = operationLocation.Substring(
                operationLocation.Length - Constants.NUMBER_OF_CHARS_IN_OPERATION_ID);

            Console.WriteLine("\nCalling GetHandwritingRecognitionOperationResultAsync()");
            TextOperationResult result =
                await computerVision.GetTextOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                Console.WriteLine(
                    "Server status: {0}, waiting {1} seconds...", result.Status, i);
                await Task.Delay(1000);

                result = await computerVision.GetTextOperationResultAsync(operationId);
            }

            return result;
        }

        public static void DisplayResults(TextOperationResult result)
        {
            // Display the results
            Console.WriteLine();
            var lines = result.RecognitionResult.Lines;
            foreach (Line line in lines)
            {
                Console.WriteLine(line.Text);
            }
            Console.WriteLine();
        }

        // Display the most relevant caption for the image
        public static void DisplayResults(ImageAnalysis analysis, string imageUri)
        {
            Console.WriteLine(imageUri);
            string json = JsonConvert.SerializeObject(analysis, Formatting.Indented);
            Console.WriteLine(json + "\n");
        }
    }
}
