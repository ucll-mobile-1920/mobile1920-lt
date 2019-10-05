
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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
