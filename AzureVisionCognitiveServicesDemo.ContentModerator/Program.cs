using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Newtonsoft.Json;

namespace AzureVisionCognitiveServicesDemo.ContentModerator
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            ContentModeratorClient contentModeratorClient = new ContentModeratorClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));

            contentModeratorClient.Endpoint = Constants.ENDPOINT_URL;

            Console.WriteLine("Moderating photos: ");

            var r1 = await ContentModeratorAnalyzer.DetectLocalAsync(contentModeratorClient, Paths.IMAGE_PATH_0);
            Console.WriteLine(JsonConvert.SerializeObject(r1, Formatting.Indented));
            Console.ReadLine();

            var r2 = await ContentModeratorAnalyzer.DetectLocalAsync(contentModeratorClient, Paths.IMAGE_PATH_1);
            Console.WriteLine(JsonConvert.SerializeObject(r2, Formatting.Indented));
            Console.ReadLine();

            var r3 = await ContentModeratorAnalyzer.DetectLocalAsync(contentModeratorClient, Paths.IMAGE_PATH_2);
            Console.WriteLine(JsonConvert.SerializeObject(r3, Formatting.Indented));
            Console.ReadLine();

            var r4 = await ContentModeratorAnalyzer.DetectLocalAsync(contentModeratorClient, Paths.IMAGE_PATH_3);
            Console.WriteLine(JsonConvert.SerializeObject(r4, Formatting.Indented));
            Console.ReadLine();

            var r5 = await ContentModeratorAnalyzer.DetectLocalAsync(contentModeratorClient, Paths.TEXT_PATH);
            Console.WriteLine(JsonConvert.SerializeObject(r5, Formatting.Indented));
            Console.ReadLine();
        }
    }
}
