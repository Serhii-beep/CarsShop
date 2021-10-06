using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CarShop.FileManager
{
    public interface IFileManager
    {
        public bool HasExtenssion(IFormFile file, string[] extenssions);
        public string UploadedFile(IFormFile file, string pathInWebroot);

        public bool deleteFile(string path);
    }
}
