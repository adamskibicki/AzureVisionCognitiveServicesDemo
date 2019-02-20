using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace AzureVisionCognitiveServicesDemo.Face
{
    class Program
    { 
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));
            faceClient.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Faces being detected ...");
            var r1 = await FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_0);
            FaceAnalyzer.DisplayResults(r1, Paths.LOCAL_IMAGE_PATH_0);

            Console.ReadLine();

            var r2 = await FaceAnalyzer
                .DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH_1);
            FaceAnalyzer.DisplayResults(r2, Paths.LOCAL_IMAGE_PATH_1);
            Console.ReadLine();

            var r3 = await FaceAnalyzer
                .CompareTwoFaces(faceClient, r1.First().FaceId.Value, r2.First().FaceId.Value);
            FaceAnalyzer.DisplayResults(r3);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
