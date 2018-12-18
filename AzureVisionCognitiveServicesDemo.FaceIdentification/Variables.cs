using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AzureVisionCognitiveServicesDemo.FaceIdentification
{
    public static class Variables
    {
        // Specify the features to return
        public static readonly FaceAttributeType[] FaceAttributes =
            {
                FaceAttributeType.Age,
                FaceAttributeType.Gender,
                FaceAttributeType.Emotion,
                FaceAttributeType.Smile,
                FaceAttributeType.Glasses,
                FaceAttributeType.Hair,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Accessories,
                FaceAttributeType.Makeup,
            };
    }
}
