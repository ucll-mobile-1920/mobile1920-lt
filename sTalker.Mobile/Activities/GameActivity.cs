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
        Player playerToFind;

        private int duration;
        private int timeLeft;

        private int nextHintShownAfter;
        private int nextHintTimeLeft;

        private TextView gameTimer;

        private EditText[] hints;

        private int currentHintNr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);

            FindViewById<TextView>(Resource.Id.game_Title).Text = GameInfo.title;

            gameTimer = FindViewById<TextView>(Resource.Id.game_timer);

            hints = new EditText[]
            {
                FindViewById<EditText>(Resource.Id.hint1),
                FindViewById<EditText>(Resource.Id.hint2),
                FindViewById<EditText>(Resource.Id.hint3),
                FindViewById<EditText>(Resource.Id.hint4),
                FindViewById<EditText>(Resource.Id.hint5)
            };

            FindViewById<Button>(Resource.Id.foundSomeone_btn).Click += (sender, e) => {
                StartActivity(typeof(ProveItActivity));
            };

            FillPlayerData();
            SetTimer();
            
        }

        public void FillPlayerData()
        {
            playerToFind = Task.Run(async()=>await DataHelper.GetFirebase()
                .Child($"Games/{GameInfo.roomCode}/Players/{GameInfo.player.UserId}/playerToFind")
                .OnceSingleAsync<Player>()).Result;

            GameInfo.player.playerToFind = playerToFind;

            currentHintNr = 0;
            hints[0].Text = playerToFind.hints[0];
        }

        public void SetTimer()
        {
            duration = Task.Run(async () => await DataHelper.GetFirebase()
                .Child($"Games/{GameInfo.roomCode}/Duration").OnceSingleAsync<int>()).Result * 60;

            timeLeft = duration;
            nextHintShownAfter = duration / 5;
            nextHintTimeLeft = nextHintShownAfter;
            gameTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"hh\:mm\:ss");

            GameInfo.timer = new Timer();
            GameInfo.timer.Interval = 1000;
            GameInfo.timer.Elapsed += Timer_Elapsed;
            GameInfo.timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timeLeft-- == 0)
            {
                GameInfo.timer.Stop();
                GameInfo.timer.Dispose();
                StartActivity(typeof(ResultsActivity));
                return;
            }
            if(nextHintTimeLeft-- == 0)
            {
                nextHintTimeLeft = nextHintShownAfter;
                RunOnUiThread(() => hints[++currentHintNr].Text = playerToFind.hints[currentHintNr]);
            }
            if (currentHintNr < 4)
            {

                RunOnUiThread(() =>
                {
                    try
                    {
                        hints[currentHintNr + 1].Text = $"{TimeSpan.FromSeconds(nextHintTimeLeft).ToString(@"hh\:mm\:ss")} until next hint";
                    }
                    catch { };
                });

            }
            RunOnUiThread(()=>gameTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"hh\:mm\:ss"));
        }
    }
}
