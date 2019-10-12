using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;

namespace sTalker.Activities
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);


            FindViewById<Button>(Resource.Id.foundSomeone_btn).Click += (sender, e) => {
                StartActivity(typeof(ProveItActivity));
            };
        }
    }
}
