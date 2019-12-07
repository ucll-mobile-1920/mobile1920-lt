using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using sTalker.Shared.Models;

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
