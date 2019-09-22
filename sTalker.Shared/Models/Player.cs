using System;
using System.Collections.Generic;

namespace sTalker.Shared.Models
{
    public class Player
    {
        public string Name
        {
            get;
            set;
        }
        public List<string> hints;
        public int roomCode;
        public bool isAdmin;
        public int points;
        public Player playerToFind;

        public Player(bool isAdmin)
        {
            this.isAdmin = isAdmin;
            hints = new List<string>();
        }
    }
}
