using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Newtonsoft.Json;
using sTalker.Helpers;
using sTalker.Shared.Models;

namespace sTalker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class CreateGameActivity : AppCompatActivity
    {
        AutoCompleteTextView title;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.createGame);

            title = FindViewById<AutoCompleteTextView>(Resource.Id.title_txt);

            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {
                CreateGame(title.Text);
                StartActivity(typeof(PlayersListActivity));
            };
        }

        private void CreateGame(string gameTitle)
        {
            Game game = new Game(gameTitle);
            DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
            reference.Child(game.RoomCode.ToString()).SetValue(JsonConvert.SerializeObject(game));
        }
    }
}

