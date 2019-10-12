
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
    [Activity(Label = "LivePointsActivity")]
    public class LivePointsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.livePoints);

            FindViewById<TextView>(Resource.Id.title).Text = GameInfo.title;
            FindViewById<TextView>(Resource.Id.roomCode).Text = GameInfo.roomCode;
        }
    }
}
