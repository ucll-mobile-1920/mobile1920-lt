using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using sTalker.Shared.Models;
using System.Timers;

namespace sTalker
{
    public static class GameInfo
    {
        public static string roomCode;
        public static string title;
        public static FaceServiceClient faceServiceClient = new FaceServiceClient(
            "17fbcadfb58d4322bae1dc90045f4bf6", "https://juste.cognitiveservices.azure.com/face/v1.0");
        public static PersonGroup personGroup;
        public static Player player;
        public static Timer timer;
    }
}
