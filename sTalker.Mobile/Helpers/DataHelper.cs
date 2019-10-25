using System;
using Android.App;
using Firebase;
using Firebase.Database;
using Firebase.Storage;

namespace sTalker.Helpers
{
    public static class DataHelper
    {
        private static FirebaseClient firebase = new FirebaseClient("https://stalker-2019.firebaseio.com/");

        public static FirebaseClient GetFirebase()
        {
            return firebase;
        }
    }
}
