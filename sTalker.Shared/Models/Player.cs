using System;
namespace sTalker.Shared.Models
{
    public class Player
    {
        public string name;
        public string hint1;
        public string hint2;
        public int roomCode;
        public bool isAdmin;
        public int points;
        public Player playerToFind;

        public Player(bool isAdmin)
        {
            this.isAdmin = isAdmin;
        }
    }
}
