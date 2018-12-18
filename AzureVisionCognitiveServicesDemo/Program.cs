using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace AzureVisionCognitiveServicesDemo
{
    class Program
    {
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-hand-text-sdk
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-analyze-sdk
        //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-detecting-domain-content
        static void Main(string[] args)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));

            computerVision.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Images being analyzed ...");
            var t1 = ComputerVisionAnalyzer.AnalyzeLocalAsync(computerVision, Paths.LOCAL_IMAGE_PATH, Variables.Features);
            var t2 = ComputerVisionAnalyzer.AnalyzeLocalAsync(computerVision, Paths.LOCAL_CELEBRITY_PATH, Variables.Features);

            var r1 = t1.GetAwaiter().GetResult();
            ComputerVisionAnalyzer.DisplayResults(r1, Paths.LOCAL_IMAGE_PATH);

            var r2 = t2.GetAwaiter().GetResult();
            ComputerVisionAnalyzer.DisplayResults(r2, Paths.LOCAL_CELEBRITY_PATH);

            ComputerVisionAnalyzer.ExtractLocalTextAsync(computerVision, Paths.LOCAL_HANDWRITTEN_IMAGE_PATH).GetAwaiter().GetResult();

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}