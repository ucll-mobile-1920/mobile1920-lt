using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using sTalker.Activities;
using sTalker.Adapters;
using sTalker.Shared.Models;

namespace sTalker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class PlayersListActivity : AppCompatActivity
    {
        private ListView playersListView;
        private List<Player> list;
        PlayersAdapter adapter;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.playersList);

            FindViewById<Button>(Resource.Id.startGame_btn).Click += (sender, e) => {
                StartActivity(typeof(GameActivity));
            };

            List<Player> players = new List<Player>();
            Player p1 = new Player(false);
            p1.Name = "John";
            players.Add(p1);
            Player p2 = new Player(false);
            p2.Name = "Bob";
            players.Add(p2);
            playersListView = FindViewById<ListView>(Resource.Id.list);
            list = new List<Player>();
            list = players;
            adapter = new PlayersAdapter(this, list);
            playersListView.Adapter = adapter;
            playersListView.ItemClick += PlayersListView_ItemClick;
        }

        private void PlayersListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = list[e.Position].Name;
            Android.Widget.Toast.MakeText(this, select, Android.Widget.ToastLength.Long).Show();
        }
    }
}

