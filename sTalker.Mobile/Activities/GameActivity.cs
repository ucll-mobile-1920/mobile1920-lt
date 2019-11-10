using Android.App;
using Android.OS;
using Android.Widget;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace sTalker.Activities
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        private int duration;
        private int timeLeft;

        private TextView gameTimer;

        private EditText hint1;
        private EditText hint2;
        private EditText hint3;
        private EditText hint4;
        private EditText hint5;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);

            gameTimer = FindViewById<TextView>(Resource.Id.game_timer);

            hint1 = FindViewById<EditText>(Resource.Id.hint1);
            hint2 = FindViewById<EditText>(Resource.Id.hint2);
            hint3 = FindViewById<EditText>(Resource.Id.hint3);
            hint4 = FindViewById<EditText>(Resource.Id.hint4);
            hint5 = FindViewById<EditText>(Resource.Id.hint5);




            FindViewById<Button>(Resource.Id.foundSomeone_btn).Click += (sender, e) => {
                StartActivity(typeof(ProveItActivity));
            };

            FillPlayerData();
            SetTimer();
            
        }

        public void FillPlayerData()
        {
            var player = Task.Run(async()=>await DataHelper.GetFirebase()
                .Child($"Games/{GameInfo.roomCode}/Players/{GameInfo.player.UserId}/playerToFind")
                .OnceSingleAsync<Player>()).Result;
            GameInfo.player.playerToFind = player;
            try
            {
                hint1.Text = player.hints[0];
                hint2.Text = player.hints[1];
                hint3.Text = player.hints[2];
                hint4.Text = player.hints[3];
                hint5.Text = player.hints[4];
            } catch 
            {
            }
        }

        public void SetTimer()
        {
            duration = Task.Run(async () => await DataHelper.GetFirebase()
                .Child($"Games/{GameInfo.roomCode}/Duration").OnceSingleAsync<int>()).Result * 60;

            timeLeft = duration;
            gameTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"hh\:mm\:ss");

            GameInfo.timer = new Timer();
            GameInfo.timer.Interval = 1000;
            GameInfo.timer.Elapsed += Timer_Elapsed;
            GameInfo.timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timeLeft-- == 00)
            {
                GameInfo.timer.Stop();
                GameInfo.timer.Dispose();
                StartActivity(typeof(ResultsActivity));
            }
            string timeString = TimeSpan.FromSeconds(timeLeft).ToString(@"hh\:mm\:ss");
            RunOnUiThread(()=>gameTimer.Text = timeString);
        }
    }
}
