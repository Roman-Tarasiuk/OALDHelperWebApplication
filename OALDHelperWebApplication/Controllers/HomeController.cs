using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

namespace OALDHelperWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult GetFile(string url, string extension = "jpg",
            bool extensionIsImportant = false, string mimeType = "application/octet-stream",
            string key = "")
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

            if (key != String.Empty)
            {
                fileBytes = Crypt(fileBytes, key);
            }

            var result = File(fileContents: fileBytes,
                            contentType: mimeType,
                            fileDownloadName: fileDownloadName);
            result.LastModified = new DateTimeOffset(new DateTime(2020, 1, 1));

            return result;
        }

        byte[] Crypt(byte[] data, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key == String.Empty)
            {
                return data;
            }

            var result = new byte[data.Length];

            var passBytes = Encoding.UTF8.GetBytes(key);
            var count = 0;

            for (var i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ passBytes[count++]);

                if (count == passBytes.Length)
                {
                    count = 0;
                }
            }

            return result;
        }
    }
}
