using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using sTalker.Helpers;
using sTalker.Shared.Models;

namespace sTalker.Listeners
{
    public class GameListener : Java.Lang.Object, IValueEventListener
    {
        List<Player> list = new List<Player>();
        public event EventHandler<GameDataEventArgs> DataRetrieved;

        public GameListener()
        {

        }

        public class GameDataEventArgs : EventArgs
        {
            public List<Player> Players { get; set; }
        }

        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if(snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                foreach (DataSnapshot game in child)
                {
                    Player player = new Player();
                    game.Child("ffff").Value.ToString();
                    list.Add(player);
                }
                DataRetrieved.Invoke(this, new GameDataEventArgs { Players = list });
            }
        }

        public void Create()
        {
            DatabaseReference reference = DataHelper.GetDatabase().GetReference("game");
            reference.AddValueEventListener(this);
        }
    }
}
