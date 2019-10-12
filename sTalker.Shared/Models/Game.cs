using System;
using System.Collections.Generic;
using Java.Lang;
using Java.Util;

namespace sTalker.Shared.Models
{
    public class Game
    {
        public string GameTime { get; set; }
        public int Duration { get; set; }
        public int RoomCode { get; set; }
        public string Title { get; set; }

        public Game(string title, int duration)
        {
            System.Random random = new System.Random();
            RoomCode = random.Next(1000, 9999);
            GameInfo.roomCode = RoomCode.ToString();
            GameInfo.title = title;
            Title = title;
            Duration = duration;
        }
    }
}
