using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;

namespace DataGridRazorPages.Pages {
    public class ChunkMetadata {
        public int Index { get; set; }
        public int TotalCount { get; set; }
        public int FileSize { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileGuid { get; set; }
    }

    public class IndexModel : PageModel {
        protected IWebHostEnvironment _hostingEnvironment { get; set; }
        public IndexModel(IWebHostEnvironment env) {
            _hostingEnvironment = env;
        }
        public void OnGet() {
        }
        public IActionResult OnPostFileUpload(IFormFile uploadedFile, string chunkMetadata) {
            var tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var resultImageUrl = string.Empty;
            try {
                if (!string.IsNullOrEmpty(chunkMetadata)) {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);
                    CheckFileExtensionValid(metaDataObject.FileName);
                    var tempFilePath = Path.Combine(tempPath, metaDataObject.FileGuid + ".tmp");
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    AppendContentToFile(tempFilePath, uploadedFile);
                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1))
                        resultImageUrl = ProcessUploadedFile(tempFilePath, metaDataObject.FileName);
                }
            }
            catch {
                return BadRequest();
            }
            return new JsonResult(new { imgUrl = resultImageUrl });
        }
        void CheckFileExtensionValid(string fileName) {
            fileName = fileName.ToLower();
            string[] imageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

            var isValidExtenstion = imageExtensions.Any(ext =>
            {
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
