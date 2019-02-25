using System;
using Microsoft.Azure.CognitiveServices.ContentModerator;

namespace AzureVisionCognitiveServicesDemo.ContentModerator
{
    class Program
    {
        static void Main(string[] args)
        {
            ContentModeratorClient contentModeratorClient = new ContentModeratorClient(new ApiKeyServiceClientCredentials(Constants.SUBSCRIPTION_KEY));

            contentModeratorClient.Endpoint = Constants.ENDPOINT_URL;
        }
    }
}
