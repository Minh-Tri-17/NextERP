using Microsoft.Extensions.Hosting;
using NextERP.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class StorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "Images";

        public StorageService(IHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.ContentRootPath, USER_CONTENT_FOLDER_NAME);
        }

        public async Task DeleteFileAsync(string fileName, string subFolder = "")
        {
            var folderPath = Path.Combine(_userContentFolder, subFolder);
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
                await Task.Run(() => File.Delete(filePath));
        }

        public string GetFileUrl(string fileName, string subFolder = "")
        {
            var folderPath = Path.Combine(_userContentFolder, subFolder);
            return Path.Combine(folderPath, fileName);
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string subFolder = "")
        {
            var folderPath = Path.Combine(_userContentFolder, subFolder);

            // Tạo folder nếu chưa có
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
    }
}
