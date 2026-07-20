using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.AttachmentServiec
{
    public class AttachmentServiec : IAttachmentServiec
    {

        public AttachmentServiec(ILogger<AttachmentServiec> logger , IWebHostEnvironment env) 
        {
            _logger = logger;
            _env = env;
        }

        string[] allowedExtensions = [".jpg", ".jpeg", ".png" , ".webp"];
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2 MB
        private readonly ILogger<AttachmentServiec> _logger;
        private readonly IWebHostEnvironment _env;

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName)
        {
            // check if fileStream is null or empty
            if (fileStream is null || !fileStream.CanRead)
                return null;

            // check on length of filestream
            if(fileStream.Length == 0)
                return null;
            // check on lenght of fileName
            if (fileStream.Length > _maxFileSize) { 
            
                _logger.LogError($"File size is too large. Maximum allowed size is {_maxFileSize} bytes.");
                return null;
            }

            var extension = Path.GetExtension(fileName).ToLower();
            if (string.IsNullOrWhiteSpace(fileName) || !allowedExtensions.Contains(extension)) 
            { 
            
                _logger.LogError($"File Rejected : Extension {extension} not allowed.");
                return null;
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath , "uploads" ,folderName);
            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}{fileName}";
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed To Upload File {fileName}");
                return null;
            }



        }

        public bool Delete(string filename, string foldername)
        {
            if(string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(foldername))
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, foldername, filename);
            try
            {
                if (!File.Exists(fullPath))
                    return false;

                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed To Delete Image");
                return false;
            }


        }
    }
}
