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
        int duration;
        SeekBar seekBar;
        TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            duration = 10;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.createGame);

            title = FindViewById<EditText>(Resource.Id.nameOfTheGame_txt);

            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {
                CreateGame(title.Text);
                StartActivity(typeof(PlayersListActivity));
            };

            seekBar = FindViewById<SeekBar>(Resource.Id.time_seekBar);
            textView = FindViewById<TextView>(Resource.Id.time);
            seekBar.Max = 170;
            seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    textView.Text = string.Format("{0} min", e.Progress+10);
                    duration = e.Progress + 10;
                }
            };

        }

        private void CreateGame(string gameTitle)
        {
            game = new Game(gameTitle, duration);
            DatabaseReference reference = DataHelper.GetDatabase().GetReference("games");
            reference.Child(game.RoomCode.ToString()).Child("info").Child("title").SetValue(game.Title);
            reference.Child(game.RoomCode.ToString()).Child("info").Child("duration").SetValue(game.Duration);
        }
    }
}

