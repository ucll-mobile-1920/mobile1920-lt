using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using sTalker.Activities;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Listeners;
using sTalker.Shared.Models;

namespace sTalker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class PlayersListActivity : AppCompatActivity
    {
        private ListView playersListView;
        private List<Player> list;
        PlayersAdapter adapter;
        DateTime now;
        PlayersListener playersListener;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.playersList);

            FindViewById<Button>(Resource.Id.letsTalk_btn).Click += (sender, e) => {
                now = DateTime.Now;
                DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
                reference.Child(GameInfo.roomCode).Child("info").Child("time").SetValue(now.ToString());
                StartActivity(typeof(LivePointsActivity));
            };

            list = new List<Player>();
            

            FindViewById<TextView>(Resource.Id.title).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.roomCode).Text = GameInfo.roomCode;
            //RetrieveData();
        }

        private void PlayersListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = list[e.Position].Name;
            Android.Widget.Toast.MakeText(this, select, Android.Widget.ToastLength.Long).Show();
        }

        public void RetrieveData()
        {
            playersListener = new PlayersListener();
            playersListener.Create();
            playersListener.DataRetrieved += PlayersListener_DataRetrieved;
        }

        private void PlayersListener_DataRetrieved(object sender, PlayersListener.PlayerDataEventArgs e)
        {
            list = e.Players;
            SetupRecyClerView();
        }

        private void SetupRecyClerView()
        {
            playersListView = FindViewById<ListView>(Resource.Id.list);
            adapter = new PlayersAdapter(this, list);
            playersListView.Adapter = adapter;
            playersListView.ItemClick += PlayersListView_ItemClick;
        }
    }
}

