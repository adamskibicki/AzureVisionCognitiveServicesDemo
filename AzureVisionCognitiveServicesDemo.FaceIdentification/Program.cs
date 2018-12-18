using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

using System;

namespace AzureVisionCognitiveServicesDemo.FaceIdentification
{
    class Program
    { 
        static void Main(string[] args)
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));

            faceClient.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Faces being detected ...");
            var t2 = FaceAnalyzer.DetectLocalAsync(faceClient, Paths.LOCAL_IMAGE_PATH);

            t2.GetAwaiter().GetResult();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
