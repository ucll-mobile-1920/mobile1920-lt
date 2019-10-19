using System;
using Android.App;
using Firebase;
using Firebase.Database;
using Firebase.Storage;

namespace sTalker.Helpers
{
    public static class DataHelper
    {
        private static FirebaseStorage storage = new FirebaseStorage("stalker-2019.appspot.com");

        public static FirebaseDatabase GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;

            if(app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApplicationId("stalker - 2019")
                    .SetApiKey("AIzaSyC9iN0Rkx_3E8tIxYLS6xK4tzaPwUCNxfA")
                    .SetDatabaseUrl("https://stalker-2019.firebaseio.com")
                    .SetStorageBucket("stalker-2019.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }
            return database;
        }

        public static FirebaseStorage GetStorage()
        {
            return storage;
        }
    }
}
