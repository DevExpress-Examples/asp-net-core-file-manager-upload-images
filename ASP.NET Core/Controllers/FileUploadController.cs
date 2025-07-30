using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Text.Json;
using ASP.NET_Core.Models;

namespace ASP.NET_Core.Controllers {
    public class FileUploadController : Controller {
        protected IWebHostEnvironment _hostingEnvironment { get; set; }

        public FileUploadController(IWebHostEnvironment env) {
            _hostingEnvironment = env;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile uploadedFile, string chunkMetadata) {
            var tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var resultImageUrl = string.Empty;
            try {
                if (!string.IsNullOrEmpty(chunkMetadata)) {
                    var metaDataObject = JsonSerializer.Deserialize<ChunkMetadata>(chunkMetadata);
                    CheckFileExtensionValid(metaDataObject.FileName);
                    var tempFilePath = Path.Combine(tempPath, metaDataObject.FileGuid + ".tmp");
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    AppendContentToFile(tempFilePath, uploadedFile);
                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1))
                        resultImageUrl = ProcessUploadedFile(tempFilePath, metaDataObject.FileName);
                }
            } catch {
                return BadRequest();
            }
            return new JsonResult(new { imgUrl = resultImageUrl });
        }

        void CheckFileExtensionValid(string fileName) {
            fileName = fileName.ToLower();
            string[] imageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

            var isValidExtenstion = imageExtensions.Any(ext => {
                return fileName.LastIndexOf(ext) > -1;
            });
            if (!isValidExtenstion)
                throw new Exception("Not allowed file extension");
        }

        void CheckMaxFileSize(FileStream stream) {
            if (stream.Length > 4000000)
                throw new Exception("File is too large");
        }

        string ProcessUploadedFile(string tempFilePath, string fileName) {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);
            System.IO.File.Copy(tempFilePath, path);
            return Path.GetRelativePath(_hostingEnvironment.WebRootPath, path);
        }

        void AppendContentToFile(string path, IFormFile content) {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write)) {
                content.CopyTo(stream);
                CheckMaxFileSize(stream);
            }
        }
    }
}
