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
    [Activity(Label = "UserWaitingActivity")]
    public class UserWaitingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.userWaiting);

            FindViewById<Button>(Resource.Id.test_btn).Click += (sender, e) =>
            {
                StartActivity(typeof(GameActivity));
            };

        }
    }
}