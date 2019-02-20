using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace AzureVisionCognitiveServicesDemo.ComputerVision
{
    public static class Variables
    {
        // Specify the features to return
        public static readonly List<VisualFeatureTypes> Features =
            new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags
            };

        // For printed text, change to TextRecognitionMode.Printed
        public static readonly TextRecognitionMode TextRecognitionMode =
            TextRecognitionMode.Handwritten;
    }
}
