
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
    [Activity(Label = "CameraActivity")]
    public class RegisterPhotoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registerPhoto);

            FindViewById<Button>(Resource.Id.takeSelfie_btn).Click += (sender, e) =>
            {
                StartActivity(typeof(GameActivity));
            };
        }
    }
}
