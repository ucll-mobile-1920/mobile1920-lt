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
        public List<Player> Players { get; set; }

        /*Using array because firebase subscription doesn't work elseway*/
        public List<GameStatus> Status { get; set; }

        public Game(string title, int duration)
        {
            Players = new List<Player>();
            Status = new List<GameStatus>();
            System.Random random = new System.Random();
            RoomCode = random.Next(1000, 9999);
            GameInfo.roomCode = RoomCode.ToString();
            GameInfo.title = title;
            Title = title;
            Duration = duration;
            Status.Add(GameStatus.NEW);
        }

        public Game()
        {
        }
    }

    public enum GameStatus { NEW, STARTED, FINISHED}
}
