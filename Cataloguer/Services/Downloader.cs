using System.IO;

namespace Cataloguer.Services
{
    public class Downloader : IDownloader
    {
        public void Download(string trackName, string artistName)
        {
            string mainFolderPath = @"C:\ProgramData\Cataloguer";
            string artistFolderPath = Path.Combine(mainFolderPath, artistName);
            Directory.CreateDirectory(artistFolderPath);
            string trackFullPath = Path.Combine(artistFolderPath, trackName) + ".mp3";
            if (!File.Exists(trackFullPath))
            {
                File.Create(trackFullPath);
            }
        }
    }
}
