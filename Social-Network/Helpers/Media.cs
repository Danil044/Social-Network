using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Helpers
{
    public class Media
    {
        public static string WebRootStoragePath = "";

        public static String CreateDirectory(String directoryPath)
        {
            DateTime date = DateTime.Now;
            if (!Directory.Exists(WebRootStoragePath + directoryPath + "\\" + date.Year))
                Directory.CreateDirectory(WebRootStoragePath + directoryPath + "\\" + date.Year);

            if (!Directory.Exists(WebRootStoragePath + directoryPath + "\\" + date.Year + "\\" + date.Month))
                Directory.CreateDirectory(WebRootStoragePath + directoryPath + "\\" + date.Year + "\\" + date.Month);

            return directoryPath + "/" + date.Year + "/" + date.Month;
        }

        public static string GetDefaultExtension(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        public async static Task<string> UploadImage(IFormFile fileToSocialNetwork, string path = "tmp")
        {
            if (fileToSocialNetwork != null)
            {
                path = Media.CreateDirectory(path); 
                path += "/" + Guid.NewGuid().ToString()
                    + GetDefaultExtension(fileToSocialNetwork.ContentType);

                using (var fileStream = new FileStream(WebRootStoragePath + path, FileMode.Create))
                {
                    await fileToSocialNetwork.CopyToAsync(fileStream);
                }

                return "/storage/" + path;

            }
            return null;
        }
    }
}

/*
        public static string WebRootStoragePath = "";

        public static String CreateDirectory(String directoryPath)
        {
            DateTime date = DateTime.Now;
            if (!Directory.Exists(WebRootStoragePath + directoryPath + "\\" + date.Year))
                Directory.CreateDirectory(WebRootStoragePath + directoryPath + "\\" + date.Year);

            if (!Directory.Exists(WebRootStoragePath + directoryPath + "\\" + date.Year + "\\" + date.Month))
                Directory.CreateDirectory(WebRootStoragePath + directoryPath + "\\" + date.Year + "\\" + date.Month);

            return directoryPath + "/" + date.Year + "/" + date.Month;
        }

        public static readonly List<String> ImageExtensions = new List<String> { ".png", ".jpe", ".bmp", ".jpg" };
        public static String WebRootPath;
        public static String StoragePath => WebRootPath + "\\storage\\";

        public static bool IsImage(string fileExtension)
        {
            String value = fileExtension.ToUpper();
            return ImageExtensions.Contains(value);
        }

        public async static Task<string> UploadImage(IFormFile file, string directoryName)
        {
            if (file == null)
                return String.Empty;

            String fileExtension = Path.GetExtension(file.FileName);

            if (!IsImage(fileExtension))
                return String.Empty;

            String fileName = Guid.NewGuid().ToString() + ".webp";
            String basePath = CreateDirectory(directoryName) + "/" + fileName;

            using (var webPFileStream = file.OpenReadStream())
            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                imageFactory.Load(webPFileStream)
                            .Format(new WebPFormat())
                            .Quality(70)
                            .Save(StoragePath + basePath);

            return "/storage/" + basePath;
        }
 */
