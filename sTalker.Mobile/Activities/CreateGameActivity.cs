using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
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
        EditText title;
        public static Game game;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.createGame);

            title = FindViewById<EditText>(Resource.Id.nameOfTheGame_txt);

            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {
                CreateGame(title.Text);
                StartActivity(typeof(PlayersListActivity));
            };
        }

        private void CreateGame(string gameTitle)
        {
            game = new Game(gameTitle);
            DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
            reference.Child(game.RoomCode.ToString()).SetValue(JsonConvert.SerializeObject(game));
        }
    }
}

