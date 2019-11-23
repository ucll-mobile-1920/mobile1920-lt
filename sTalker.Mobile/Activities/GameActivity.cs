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

        private DateTime endTime;
        private DateTime startTime;
        private DateTime firstHintTime;
        private DateTime secondHintTime;
        private DateTime thirdHintTime;
        private DateTime fourthHintTime;
        private DateTime fifthHintTime;

        private int nextHintNr;
        private DateTime nextHintTime;

        private int duration;

        private TextView gameTimer;

        private EditText[] hints;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);

            FindViewById<TextView>(Resource.Id.game_Title).Text = GameInfo.title;

            gameTimer = FindViewById<TextView>(Resource.Id.game_timer);

            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status")
                .AsObservable<GameStatus>()
                .Subscribe(x => {
                    if (x.Object == GameStatus.FINISHED)
                    {
                        EndGame();
                    }
                });

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

            hints[0].Text = playerToFind.hints[0];
        }

        public void SetTimer()
        {
            startTime = Task.Run(async () => await DataHelper.GetFirebase()
                .Child($"Games/{GameInfo.roomCode}/StartTime").OnceSingleAsync<DateTime>()).Result;

            duration = Task.Run(async () => await DataHelper.GetFirebase()
                    .Child($"Games/{GameInfo.roomCode}/Duration").OnceSingleAsync<int>()).Result;

            endTime = startTime.AddMinutes(duration);
            GameInfo.gameEnd = endTime;

            var nextHintShownAfter = duration / 5;

            firstHintTime = startTime.AddMinutes(nextHintShownAfter);
            secondHintTime = firstHintTime.AddMinutes(nextHintShownAfter);
            thirdHintTime = secondHintTime.AddMinutes(nextHintShownAfter);
            fourthHintTime = thirdHintTime.AddMinutes(nextHintShownAfter);
            fifthHintTime = fourthHintTime.AddMinutes(nextHintShownAfter);

            nextHintNr = 1;
            nextHintTime = secondHintTime;

            TimeSpan timeSpan = endTime.Subtract(DateTime.Now);
            gameTimer.Text = timeSpan.ToString(@"hh\:mm\:ss");

            GameInfo.timer = new Timer();
            GameInfo.timer.Interval = 1000;
            GameInfo.timer.Elapsed += Timer_Elapsed;
            GameInfo.timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan timeSpan = endTime.Subtract(DateTime.Now);
          
            RunOnUiThread(() => gameTimer.Text = timeSpan.ToString(@"hh\:mm\:ss"));

            if (timeSpan.TotalSeconds == 0)
            {
                EndGame();
                return;
            }
            else if(DateTime.Now >= secondHintTime && nextHintNr==1)
            {
                RunOnUiThread(() => hints[1].Text = playerToFind.hints[1]);
                nextHintNr = 2;
                nextHintTime = thirdHintTime;
            }
            else if (DateTime.Now >= thirdHintTime && nextHintNr==2)
            {
                RunOnUiThread(() => hints[2].Text = playerToFind.hints[2]);
                nextHintNr = 3;
                nextHintTime = fourthHintTime;

            }
            else if (DateTime.Now >= fourthHintTime && nextHintNr==3)
            {
                RunOnUiThread(() => hints[3].Text = playerToFind.hints[3]);
                nextHintNr = 4;
                nextHintTime = fifthHintTime;

            }
            else if (DateTime.Now >= fifthHintTime && nextHintNr==4)
            {
                RunOnUiThread(() => hints[4].Text = playerToFind.hints[4]);
                nextHintNr = 5;
            }

            if (nextHintNr < 5)
            {

                RunOnUiThread(() =>
                {
                    try
                    {
                        hints[nextHintNr].Text = $"{nextHintTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss")} until next hint";
                    }
                    catch { };
                });

            }
        }

        private void EndGame()
        {
            GameInfo.timer.Stop();
            GameInfo.timer.Dispose();
            StartActivity(typeof(ResultsActivity));
        }
    }
}
