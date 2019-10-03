using System;
using System.Collections.Generic;

namespace sTalker.Shared.Models
{
    public class Game
    {
        public List<Player> players;
        public int gameTime;
        public int roomCode;

        public Game(int roomCode, int gameTime)
        {
            this.gameTime = gameTime;
            this.roomCode = roomCode;
        }
    }
}
