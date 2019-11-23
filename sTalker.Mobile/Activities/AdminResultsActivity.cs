using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Database.Query;
using sTalker.Adapters;
using sTalker.Helpers;
using sTalker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sTalker.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class AdminResultsActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.adminResults);

        }
    }
        
}

