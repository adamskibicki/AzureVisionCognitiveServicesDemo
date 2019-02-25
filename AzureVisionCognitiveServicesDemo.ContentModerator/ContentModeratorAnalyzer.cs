﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Microsoft.Rest;

namespace AzureVisionCognitiveServicesDemo.ContentModerator
{
    public static class ContentModeratorAnalyzer
    {
        public static async Task<Evaluate> DetectLocalAsync(ContentModeratorClient contentModeratorClient, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return null;
            }

            try
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    HttpOperationResponse<Evaluate> response =
                        await contentModeratorClient.ImageModeration.EvaluateFileInputWithHttpMessagesAsync(
                            imageStream, true);

                    return response.Body;
                }
            }
            catch (APIErrorException e)
            {
                Console.WriteLine(imagePath + ": " + e.Message);
            }
            return null;
        }
    }
}