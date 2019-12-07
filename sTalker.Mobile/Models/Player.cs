using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sTalker.Shared.Models
{
    public class Player
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public List<string> hints;
        public int roomCode;
        public int points;
        public Player playerToFind;

        //used when identifying faces
        public Guid RecognitionServiceId { get; set; }

        public Player()
        {

        }

        public Player(string Name, List<string> hints)
        {
            Random random = new Random();
            UserId = random.Next(100000000, 999999999).ToString();
            this.Name = Name;
            this.hints = hints;
        }

    }
}
