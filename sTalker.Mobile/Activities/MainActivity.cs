using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using sTalker.Shared.Models;
using System.Threading.Tasks;

namespace sTalker.Activities
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static Player player;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.main);

            FindViewById<Button>(Resource.Id.newGame_btn).Click += (sender, e) =>
            {
                StartActivity(typeof(CreateGameActivity));
            };

            //FindViewById<Button>(Resource.Id.newGame_btn).Click += (sender, e) =>
            //{
            //    GameInfo.roomCode = "7574";
            //    GameInfo.duration = 10;
            //    StartActivity(typeof(PlayersListActivity));
            //};

            FindViewById<Button>(Resource.Id.joinRoom_btn).Click += (sender, e) =>
            {
                StartActivity(typeof(EnterRoomCodeActivity));
            };

            //FindViewById<Button>(Resource.Id.joinRoom_btn).Click += (sender, e) =>
            //{
            //    GameInfo.player = new Player();
            //    GameInfo.player.UserId = "48";
            //    GameInfo.roomCode = "7293";
            //    GameInfo.personGroup = Task.Run(async () => await GameInfo.faceServiceClient.GetPersonGroupAsync("7293")).Result;
            //    StartActivity(typeof(GameActivity));
            //};
        }
    

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

