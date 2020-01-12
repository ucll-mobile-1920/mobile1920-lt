using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using sTalker.Helpers;
using sTalker.Notifications;
using sTalker.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class RegistrationActivity : AppCompatActivity
    {
        Player player;
        string name;
        string[] hints;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.registration);
            hints = new string[5];


            FindViewById<Button>(Resource.Id.next_btn).Click += (sender, e) => {

                name = FindViewById<EditText>(Resource.Id.name).Text;
                hints[0]=FindViewById<EditText>(Resource.Id.hint1).Text;
                hints[1]=FindViewById<EditText>(Resource.Id.hint2).Text;
                hints[2]=FindViewById<EditText>(Resource.Id.hint3).Text;
                hints[3]=FindViewById<EditText>(Resource.Id.hint4).Text;
                hints[4]=FindViewById<EditText>(Resource.Id.hint5).Text;

                if (string.IsNullOrEmpty(name) || hints.Where(x=>string.IsNullOrEmpty(x)).Count() != 0)
                {
                    new ToastCreator(this, "Please fill in all fields!").Run();
                    return;
                }

                var status = Task.Run(async () =>
                    await DataHelper.GetFirebase().Child($"Games/{GameInfo.roomCode}/Status/0").OnceSingleAsync<GameStatus>()).Result;
                if (status != GameStatus.NEW)
                {
                    new ToastCreator(this, "Game has already started. You cannot join.").Run();
                    StartActivity(typeof(MainActivity));
                    Finish();
                    return;
                }

                player = new Player(name, hints.ToList());
                GameInfo.player = player;

                StartActivity(typeof(SelfieCameraActivity));
                Finish();
            };

            FindViewById<TextView>(Resource.Id.registrationTitle).Text = GameInfo.title;
        }
    }
}

