using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Java.Util;
using sTalker.Helpers;
using sTalker.Shared.Models;

namespace sTalker
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

            FindViewById<Button>(Resource.Id.joinGame_btn).Click += (sender, e) => {
				player = new Player(false);
                StartActivity(typeof(RegistrationActivity));
            };

            FindViewById<Button>(Resource.Id.createGame_btn).Click += (sender, e) => {
                //player = new Player(true);
                //StartActivity(typeof(RegistrationActivity));
                HashMap game = new HashMap();
                game.Put("code", 1234);
                game.Put("time", 2019);
                DatabaseReference newGame = DataHelper.GetDatabase().GetReference("game").Push();
                newGame.SetValue((Java.Lang.Object)game);
                
            };
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

