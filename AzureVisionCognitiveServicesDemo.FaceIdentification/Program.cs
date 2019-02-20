using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;

namespace AzureVisionCognitiveServicesDemo.FaceIdentification
{
    class Program
    { 
        static void Main()
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));
            faceClient.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Faces being detected ...");
            FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_0)
                .GetAwaiter()
                .GetResult();
            Console.ReadLine();

            FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_1)
                .GetAwaiter()
                .GetResult();
            Console.ReadLine();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
