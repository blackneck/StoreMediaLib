using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Windows.Storage;

namespace StoreMediaLib
{
    /// <summary>
    /// Provides methods to save and load images in isolated storage.
    /// </summary>
    public static class Images
    {
        /// <summary>
        /// Saves image stream in the specified folder.
        /// </summary>
        /// <param name="imageToBeSaved">Represents stream with image.</param>
        /// <param name="folderName">Desireble folder name.</param>
        /// <param name="fileName">Name of the file to create.</param>
        public static void SaveImage(Stream imageToBeSaved, string folderName, string fileName)
        {
            using (IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if(!local.DirectoryExists(folderName))
                    local.CreateDirectory(folderName);
                SaveImage(imageToBeSaved, folderName + "\\" + fileName);    
            }                        
        }

        /// <summary>        
        /// Saves stream with image to the specified path.
        /// </summary>
        /// <param name="imageToBeSaved">Represents stream with image.</param>
        /// <param name="fileName">The relative path of the file to create.</param>
        private static void SaveImage(Stream imageToBeSaved, string fileName)
        {
            using (IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication())
            {                
                using (IsolatedStorageFileStream isoFileStream = local.CreateFile(fileName + ".jpg"))
                {
                    byte[] readBuffer = new byte[4096];
                    int bytesRead = -1;

                    while ((bytesRead = imageToBeSaved.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        isoFileStream.Write(readBuffer, 0, bytesRead);
                    }

                }
            }            
        }

        /// <summary>
        /// Loads image by relative path.
        /// </summary>
        /// <param name="fileName">Relative path to the file in isolated storage.</param>
        /// <returns>Image as a Bitmap object.</returns>
        public static BitmapImage LoadImage(string fileName)
        {
            BitmapImage bmpImage = new BitmapImage();

            using (IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if(local.FileExists(fileName))
                    using (IsolatedStorageFileStream file = local.OpenFile(fileName, FileMode.Open))
                    {                        
                        bmpImage.SetSource(file);                        
                    }
            }            

            return bmpImage;
        }

        /// <summary>
        /// Loads all images in directory by relative path.
        /// </summary>
        /// <param name="folderName">Relative path to the folder</param>
        /// <returns>Collection of Bitmap object.</returns>
        public static List<BitmapImage> LoadImagesFromFolder(string folderName)
        {
            List<BitmapImage> retreivedImages = new List<BitmapImage>();

            foreach (var file in Directory.GetFiles(ApplicationData.Current.LocalFolder.Path + "\\" + folderName))            
                retreivedImages.Add(LoadImage(file));

            return retreivedImages;
        }
    }
}
