using System;

namespace AzureVisionCognitiveServicesDemo.FaceGrouping
{
    //https://docs.microsoft.com/en-us/azure/cognitive-services/face/face-api-how-to-topics/howtoidentifyfacesinimage
    class Program
    {
        static void Main(string[] args)
        {
            FaceGroupingAndIdentification.Start().GetAwaiter().GetResult();

            Console.ReadKey();
        }
    }
}
