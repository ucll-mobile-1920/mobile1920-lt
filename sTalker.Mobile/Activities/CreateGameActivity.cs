using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System.Collections.Generic;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class CreateGameActivity : AppCompatActivity
    {
        EditText title;
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

        private async void CreateGame(string gameTitle)
        {
            var game = new Game(gameTitle, duration);

            await GameInfo.faceServiceClient.CreatePersonGroupAsync(game.RoomCode.ToString(), game.Title);

            await DataHelper.GetFirebase().Child($"Games/{game.RoomCode}").PutAsync(game);
            await DataHelper.GetFirebase().Child($"Games/{game.RoomCode}/Players/admin").PutAsync(new Player("Admin", new List<string>(), true));

        }


    }
}

