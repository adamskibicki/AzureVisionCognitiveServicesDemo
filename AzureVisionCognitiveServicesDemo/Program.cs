using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace AzureVisionCognitiveServicesDemo.ComputerVision
{
    class Program
    {
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-hand-text-sdk
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-analyze-sdk
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-detecting-domain-content
        static void Main()
        {
            ComputerVisionClient computerVisionClient = InitializeComputerVisionClient();

            Console.WriteLine("Images being analyzed ...");

            var r1 = ComputerVisionAnalyzer
                .AnalyzeLocalAsync(computerVisionClient, Paths.LOCAL_IMAGE_PATH, Variables.Features)
                .GetAwaiter()
                .GetResult();

            ComputerVisionAnalyzer.DisplayResults(r1, Paths.LOCAL_IMAGE_PATH);
            Console.ReadLine();

            var r2 = ComputerVisionAnalyzer
                .AnalyzeLocalAsync(computerVisionClient, Paths.LOCAL_CELEBRITY_PATH, Variables.Features)
                .GetAwaiter()
                .GetResult();

            ComputerVisionAnalyzer.DisplayResults(r2, Paths.LOCAL_CELEBRITY_PATH);
            Console.ReadLine();

            var r3 = ComputerVisionAnalyzer
                .ExtractLocalTextAsync(computerVisionClient, Paths.LOCAL_HANDWRITTEN_IMAGE_PATH)
                .GetAwaiter()
                .GetResult();

            ComputerVisionAnalyzer.DisplayResults(r3);
            Console.ReadLine();

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static ComputerVisionClient InitializeComputerVisionClient()
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));

            computerVision.Endpoint = Constants.ENDPOINT_URL;
            return computerVision;
        }
    }
}