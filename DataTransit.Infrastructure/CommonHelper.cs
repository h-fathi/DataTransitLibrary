using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataTransit.Infrastructure
{
    public static class CommonHelper
    {
        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the download binary array
        /// </returns>
        public static async Task<byte[]> GetDownloadBitsAsync(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    await fileStream.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    return fileBytes;
                }
            }
        }
    }
}
