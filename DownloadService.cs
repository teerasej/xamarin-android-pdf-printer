using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PDFPrinter
{
    public class DownloadService
    {
        public string DestinationPath;
        public WebClient webClient;

        public DownloadService(string destinationPath)
        {
            DestinationPath = destinationPath;
            webClient = new WebClient();
        }

        public async Task<Uri> Download(string targetFileUrl)
        {
            var targetFileName = Path.GetFileName(targetFileUrl);
            var localFilePath = Path.Combine(DestinationPath, targetFileName);

            await webClient.DownloadFileTaskAsync(targetFileUrl, localFilePath);

            if(!File.Exists(localFilePath))
            {
                return new Uri("");
            }
            
            return new Uri(localFilePath);
        }
    }
}
