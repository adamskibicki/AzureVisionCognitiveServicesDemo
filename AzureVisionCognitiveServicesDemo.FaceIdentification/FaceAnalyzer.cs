using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;

namespace AzureVisionCognitiveServicesDemo.Face
{
    public static class FaceAnalyzer
    {
        // Detect faces in a local image
        public static async Task<IEnumerable<DetectedFace>> DetectLocalAsync(FaceClient faceClient, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return Enumerable.Empty<DetectedFace>();
            }

            try
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    IList<DetectedFace> faceList =
                        await faceClient.Face.DetectWithStreamAsync(
                            imageStream, true, false, Variables.FaceAttributes);

                    return faceList;
                }
            }
            catch (APIErrorException e)
            {
                Console.WriteLine(imagePath + ": " + e.Message);
            }
            return Enumerable.Empty<DetectedFace>();
        }

        // Detect faces in a remote image
        public static async Task<IEnumerable<DetectedFace>> DetectRemoteAsync(
            FaceClient faceClient, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine("\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return Enumerable.Empty<DetectedFace>();
            }

            try
            {
                IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        imageUrl, true, false, Variables.FaceAttributes);

                return faceList;
            }
            catch (APIErrorException e)
            {
                Console.WriteLine(imageUrl + ": " + e.Message);
            }
            return Enumerable.Empty<DetectedFace>();
        }

        public static void DisplayResults(IEnumerable<DetectedFace> faceList, string imagePath)
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

        public static async Task<VerifyResult> CompareTwoFaces(FaceClient faceClient, Guid faceId0, Guid faceId1)
        {
            var result = await faceClient.Face
                .VerifyFaceToFaceWithHttpMessagesAsync(faceId0, faceId1);

            return result.Body;
        }

        public static void DisplayResults(VerifyResult verifyResult)
        {
            Console.WriteLine($"Confidence: {verifyResult.Confidence}");
            Console.WriteLine($"Is identical: {verifyResult.IsIdentical}");
        }
    }
}
