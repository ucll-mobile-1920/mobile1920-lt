using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using sTalker.Helpers;
using sTalker.Shared.Models;

namespace sTalker.Listeners
{
    public class PlayersListener : Java.Lang.Object, IValueEventListener
    {
        List<Player> list = new List<Player>();
        public event EventHandler<PlayerDataEventArgs> DataRetrieved;

        public class PlayerDataEventArgs : EventArgs
        {
            public List<Player> Players { get; set; }
        }

        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                list.Clear();
                foreach (DataSnapshot player in child)
                {
                    Player p = new Player();
                    p.UserId = player.Value.ToString();
                    p.Name = player.Child(p.UserId).Child("name").Value.ToString();
                    list.Add(p);
                }
                DataRetrieved.Invoke(this, new PlayerDataEventArgs { Players = list });
            }
        }


        public void Create()
        {
            DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
            reference.Child(GameInfo.roomCode).Child("players").AddValueEventListener(this);
        }
    }
}
