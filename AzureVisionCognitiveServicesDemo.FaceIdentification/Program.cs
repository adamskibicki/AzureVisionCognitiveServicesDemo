using System;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace AzureVisionCognitiveServicesDemo.Face
{
    class Program
    { 
        static void Main()
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));
            faceClient.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Faces being detected ...");
            var r1 = FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_0)
                .GetAwaiter()
                .GetResult();
            FaceAnalyzer.DisplayResults(r1, Paths.LOCAL_IMAGE_PATH_0);

            Console.ReadLine();

            var r2 = FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_1)
                .GetAwaiter()
                .GetResult();
            FaceAnalyzer.DisplayResults(r2, Paths.LOCAL_IMAGE_PATH_1);
            Console.ReadLine();

            FaceAnalyzer.CompareTwoFaces(faceClient, r1.First().FaceId.Value, r2.First().FaceId.Value);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
