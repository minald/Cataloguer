namespace Cataloguer.Services
{
    public interface IDownloader
    {
        void Download(string trackName, string artistName);
    }
}
