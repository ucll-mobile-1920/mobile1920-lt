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
using Java.Lang;

namespace sTalker.Notifications
{
    public class ToastCreator : Java.Lang.Object, IRunnable
    {
        private readonly string text;
        private readonly Context context;

        public ToastCreator(Context context, string text)
        {
            this.context = context;
            this.text = text;
        }

        public void Run()
        {
            Toast.MakeText(context, text, ToastLength.Short).Show();
        }
    }
}