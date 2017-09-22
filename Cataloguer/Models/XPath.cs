namespace Cataloguer.Models
{
    public class XPath
    {
        public class MusicPage
        {
            public const string Artists = 
                "//*[@class='music-charts']/div/div[2]/table/tbody/tr[contains(@class, 'js-link-block')]";
        }

        public class ArtistProfilePage
        {
            public const string Scrobbles = 
                "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[1]/p/abbr";

            public const string Listeners = 
                "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[2]/p/abbr";

            public const string ShortBiography = 
                "//*[@id='mantle_skin']/div[4]/div/div[1]/section[2]/p";

            public const string Albums = 
                "//*[@id='mantle_skin']/div[4]/div/div[1]/section[4]/div/ol/li";

            public const string Tracks = 
                "//*[@id='top-tracks-section']/div/table/tbody/tr";

            public const string Tags = 
                "//*[@id='mantle_skin']/div[4]/div/div[1]/section[1]/ul/li";
        }

        public class BiographyPage
        {
            public const string FullBiography =
                "//*[@id='mantle_skin']/div[4]/div/div[1]/div[1]/div";
        }

        public class TracksPage
        {
            public const string Tracks =
                "//*[@id='mantle_skin']/div[4]/div/div[1]/section/table/tbody/tr[contains(@class, 'js-link-block')]";
        }

        public class AlbumsPage
        {
            public const string LastPageLink =
                "//*[@id='artist-albums-section']/nav/ul/li[contains(@class, 'pagination-page')][last()]/a";

            public const string Albums = 
                "//*[@id='artist-albums-section']/ol/li[@itemscope]";
        }
    }
}