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
        public bool isAdmin;
        public int points;
        public Player playerToFind;

        public Player()
        {

        }

        public Player(string Name, List<string> hints)
        {
            Random random = new Random();
            UserId = random.Next(10, 99).ToString();
            this.Name = Name;
            this.hints = hints;
        }
    }
}
