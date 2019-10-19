using Android.Graphics;
using Android.Media;
using Firebase.Storage;
using Java.IO;
using Java.Lang;
using Java.Nio;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using sTalker;
using sTalker.Fragments;
using sTalker.Helpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace stalker.Listeners
{
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        private readonly Camera2Fragment owner;

        public ImageAvailableListener(Camera2Fragment fragment)
        {
            owner = fragment ?? throw new ArgumentNullException("fragment");
        }

        public void OnImageAvailable(ImageReader reader)
        {
            Image img = reader.AcquireNextImage();
            ByteBuffer buffer = img.GetPlanes()[0].Buffer;
            byte[] bytes = new byte[buffer.Remaining()];
            buffer.Get(bytes);
            var stream = new MemoryStream(bytes);

            var faces = Task.Run(async () => await GameInfo.faceServiceClient.DetectAsync(stream)).Result;

            if (faces.Length < 1)
            {
                //TODO: fix the issue that camera flips taken photos
                owner.ShowToast("No face detected! Try rotating camera upside-down :D");
                return;
            }
            else if (faces.Length > 1)
            {
                owner.ShowToast("More than one face detected! Take a selfie alone!");
                return;
            }
            else
            {
                owner.Detected = true;
                AddNewFace(bytes);
            }
        }

        private async void AddNewFace(byte[] bytes)
        {
            using (var output = new FileOutputStream(owner.mFile))
            {
                try
                {
                    output.Write(bytes);
                }
                catch (Java.IO.IOException e)
                {
                    e.PrintStackTrace();
                }
            }

            using(var stream = System.IO.File.Open(owner.mFile.AbsolutePath, FileMode.Open))
            {
                var imageName = $"faces/{GameInfo.personGroup.PersonGroupId}-{GameInfo.player.UserId}.jpg";

                var storage = DataHelper.GetStorage();
                await storage.Child(imageName).PutAsync(stream);
                var url = await storage.Child(imageName).GetDownloadUrlAsync();

                try
                {
                    var result = await GameInfo.faceServiceClient.CreatePersonAsync(GameInfo.personGroup.PersonGroupId, GameInfo.player.UserId, GameInfo.player.Name);
                    var added = await GameInfo.faceServiceClient.AddPersonFaceAsync(GameInfo.personGroup.PersonGroupId, result.PersonId, url);

                }
                catch (FaceAPIException e)
                {

                }
                finally
                {
                    System.IO.File.Delete(owner.mFile.AbsolutePath);
                }

            }

        }


    }
}