using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Newtonsoft.Json;

namespace AzureVisionCognitiveServicesDemo.FaceGrouping
{
    public static class FaceGroupingAndIdentification
    {
        public static async Task Start()
        {
            var faceServiceClient = new FaceServiceClient(Constants.SUBSCRIPTION_KEY, Constants.API_URL);

            #region comment it if you have already created and trained group
            await DeleteGroup(faceServiceClient);

            // Create an empty PersonGroup
            await faceServiceClient.CreatePersonGroupAsync(Constants.PERSON_GROUP_ID, Constants.PERSON_GROUP_NAME);
            // Define known people
            await DefinePeople(faceServiceClient);
            // Train
            await faceServiceClient.TrainPersonGroupAsync(Constants.PERSON_GROUP_ID);
            // Wait
            await WaitUntilTrainingEnds(faceServiceClient);
            #endregion

            await IdentifySuspects(faceServiceClient, Paths.TEST_IMAGE_PATH_0);
            Console.ReadLine();

            await IdentifySuspects(faceServiceClient, Paths.TEST_IMAGE_PATH_1);
            Console.ReadLine();

            await IdentifySuspects(faceServiceClient, Paths.TEST_IMAGE_PATH_2);
            Console.ReadLine();

            await DeleteGroup(faceServiceClient);
        }

        private static async Task DefinePeople(FaceServiceClient faceServiceClient)
        {
            // Define person
            CreatePersonResult suspect1 = await faceServiceClient.CreatePersonAsync(
                // Id of the PersonGroup that the person belonged to
                Constants.PERSON_GROUP_ID,
                // Name of the person
                "Rafał"
            );

            CreatePersonResult suspect2 = await faceServiceClient.CreatePersonAsync(
                Constants.PERSON_GROUP_ID,
                "Marcin"
            );

            CreatePersonResult suspect3 = await faceServiceClient.CreatePersonAsync(
                Constants.PERSON_GROUP_ID,
                "Kacper"
            );

            CreatePersonResult suspect4 = await faceServiceClient.CreatePersonAsync(
                Constants.PERSON_GROUP_ID,
                "Adam"
            );

            await AddPersonFaceAsync(faceServiceClient, suspect1, Paths.suspect1ImageDir);
            await AddPersonFaceAsync(faceServiceClient, suspect2, Paths.suspect2ImageDir);
            await AddPersonFaceAsync(faceServiceClient, suspect3, Paths.suspect3ImageDir);
            await AddPersonFaceAsync(faceServiceClient, suspect4, Paths.suspect4ImageDir);
        }

        private static async Task AddPersonFaceAsync(FaceServiceClient faceServiceClient, CreatePersonResult suspectCreatePersonResult, string imageDir)
        {
            foreach (string imagePath in Directory.GetFiles(imageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add them to correct person
                    await faceServiceClient.AddPersonFaceAsync(
                        Constants.PERSON_GROUP_ID, suspectCreatePersonResult.PersonId, s);
                }
            }
        }

        private static async Task IdentifySuspects(FaceServiceClient faceServiceClient, string testImageFile)
        {
            using (Stream s = File.OpenRead(testImageFile))
            {
                var faces = await faceServiceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray();
                var faceCoordinates = faces.Select(face => JsonConvert.SerializeObject(face.FaceRectangle, Formatting.Indented));

                Console.WriteLine("Face coordinates");
                foreach (var faceCoordinate in faceCoordinates)
                {
                    Console.WriteLine(faceCoordinate);
                }

                Console.WriteLine();

                var results = await faceServiceClient.IdentifyAsync(Constants.PERSON_GROUP_ID, faceIds);
                foreach (var identifyResult in results)
                {
                    Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Length == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceServiceClient.GetPersonAsync(Constants.PERSON_GROUP_ID, candidateId);
                        Console.WriteLine("Identified as {0}", person.Name);
                    }
                }
            }
        }

        private static async Task WaitUntilTrainingEnds(FaceServiceClient faceServiceClient)
        {
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(Constants.PERSON_GROUP_ID);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }

        private static async Task DeleteGroup(FaceServiceClient faceServiceClient)
        {
            try
            {
                await faceServiceClient.DeletePersonGroupAsync(Constants.PERSON_GROUP_ID);
                Console.WriteLine("Group deleted succesfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
