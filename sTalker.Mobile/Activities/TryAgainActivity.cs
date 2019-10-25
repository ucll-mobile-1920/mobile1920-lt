
using Android.App;
using Android.OS;

namespace sTalker.Activities
{
    [Activity(Label = "TryAgainActivity")]
    public class TryAgainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //SetContentView(Resource.Layout.tryAgain);

            //FindViewById<Button>(Resource.Id.tryAgain_btn).Click += (sender, e) => {
             //   StartActivity(typeof(GameActivity));
            //};
        }
    }
}
