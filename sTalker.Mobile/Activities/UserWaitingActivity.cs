using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System;
using System.Reactive.Linq;

namespace sTalker.Activities
{
    [Activity(Label = "UserWaitingActivity")]
    public class UserWaitingActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.userWaiting);

            CheckIfStarted();
            DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status").AsObservable<GameStatus>().Subscribe(x => StartGame(x.Object));

            //For testing
            FindViewById<Button>(Resource.Id.test_btn).Click += async (sender, e) =>
            {
                StartActivity(typeof(GameActivity));
            };

        }

        public void StartGame(GameStatus gameStatus)
        {
            if(gameStatus == GameStatus.STARTED)
            {
                StartActivity(typeof(GameActivity));
            }
        }

        public async void CheckIfStarted()
        {
            //TODO: Solve the case when user tries to join after game has started
        }


    }
}