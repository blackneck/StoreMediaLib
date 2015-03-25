using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Storage;

namespace StoreMediaLib
{
    public static class Images
    {
        public static void SaveImage(BitmapImage imageToBeSaved, string folderName, string fileName)
        {
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();

            if (!local.DirectoryExists(folderName))
                local.CreateDirectory(folderName);            

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var wb = new WriteableBitmap(imageToBeSaved);

                using (var isoFileStream = isoStore.CreateFile(folderName + "\\" + fileName + ".jpg"))
                    wb.SaveJpeg(isoFileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
            }
        }

        public static List<BitmapImage> LoadImages()
        {
            List<BitmapImage> retreivedImages = new List<BitmapImage>();

            foreach (var dir in IsolatedStorageFile.GetUserStoreForApplication().GetDirectoryNames())
                retreivedImages.AddRange(LoadParticularImages(dir));

            return retreivedImages;
        }

        public static List<BitmapImage> LoadParticularImages(string folderName)
        {

            List<BitmapImage> retreivedImages = new List<BitmapImage>();

            try
            {
                foreach (var file in Directory.GetFiles(ApplicationData.Current.LocalFolder.Path + "\\" + folderName))
                {
                    using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var isoFileStream = isoStore.OpenFile(file, FileMode.Open))
                        {
                            var retreivedImage = new BitmapImage();
                            retreivedImage.SetSource(isoFileStream);
                            retreivedImages.Add(retreivedImage);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Folder not exist");
                return new List<BitmapImage>();
            }

            return retreivedImages;
        }
    }
}
