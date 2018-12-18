using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;

namespace AzureVisionCognitiveServicesDemo.FaceIdentification
{
    public static class FaceAnalyzer
    {
        // Detect faces in a remote image
        public static async Task DetectRemoteAsync(
            FaceClient faceClient, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine("\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return;
            }

            try
            {
                IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        imageUrl, true, false, Variables.FaceAttributes);

                DisplayResults(faceList, imageUrl);
            }
            catch (APIErrorException e)
            {
                Console.WriteLine(imageUrl + ": " + e.Message);
            }
        }

        private static void DisplayResults(IList<DetectedFace> faceList, string imagePath)
        {
            Console.WriteLine(imagePath);
            foreach (var detectedFace in faceList)
            {
                Console.WriteLine("FACE DETECTED:");
                var stringResultRepresentation = JsonConvert.SerializeObject(detectedFace, Formatting.Indented);

                Console.WriteLine(stringResultRepresentation);
                Console.WriteLine();
            }
        }

        // Detect faces in a local image
        public static async Task DetectLocalAsync(FaceClient faceClient, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return;
            }

            try
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    IList<DetectedFace> faceList =
                        await faceClient.Face.DetectWithStreamAsync(
                            imageStream, true, false, Variables.FaceAttributes);

                    DisplayResults(faceList, imagePath);
                }
            }
            catch (APIErrorException e)
            {
                Console.WriteLine(imagePath + ": " + e.Message);
            }
        }
    }
}
