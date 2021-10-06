using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;
namespace CarShop.FileManager
{
    public class PhotoManager : IFileManager
    {
        private IWebHostEnvironment webHostEnvironment;
        public PhotoManager(IWebHostEnvironment webHost)
        {
            webHostEnvironment = webHost;
        }
        public string UploadedFile(IFormFile photo, string pathInWebroot)
        {
            string uniqueFileName = null;

            if (photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, pathInWebroot);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                return "/" + pathInWebroot + "/" + uniqueFileName;
            }
            else
            {
                return uniqueFileName;
            }
        }
        public bool HasExtenssion(IFormFile photo, string[] extenssions)
        {
            if (photo == null) return false;
            return extenssions.Contains(Path.GetExtension(photo.FileName));
        }
        public bool deleteFile(string photoUrl)
        {
            StringBuilder deletePath = new StringBuilder(photoUrl);
            if (deletePath[0] == '/') deletePath.Remove(0, 1);
            deletePath.Replace("/", "\\");

            try
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, deletePath.ToString());
                File.Delete(uploadsFolder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
