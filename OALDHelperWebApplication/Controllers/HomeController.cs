using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace OALDHelperWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult GetFile(string url, string extension = "jpg",
            bool extensionIsImportant = false, string mimeType = "application/octet-stream")
        {
            var fileName = Path.GetFileName(url);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(url);
            var fileDownloadName = String.Empty;

            if (extension == String.Empty ||
                (fileName == fileNameWithoutExt && !extensionIsImportant))
            {
                fileDownloadName = fileName;
            }
            else
            {
                fileDownloadName = fileNameWithoutExt + "." + extension;
            }


            var fileBytes = new WebClient().DownloadData(url);

            var result = File(fileContents: fileBytes,
                            contentType: mimeType,
                            fileDownloadName: fileDownloadName);
            result.LastModified = new DateTimeOffset(new DateTime(2020, 1, 1));

            return result;
        }
    }
}
