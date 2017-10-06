using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class ViewObject
    {
        public string Heading { get; set; }

        public string NoElementsMessage { get; set; }

        public string ContainerId { get; set; }

        public string PanelCssClass { get; set; }

        public List<MusicObject> MusicObjects { get; set; }   

        public string ButtonId { get; set; }

        public ViewObject() { }
    }

    public class MusicObject
    {
        public string Name { get; set; }

        public string PageLink { get; set; }

        public string PictureLink { get; set; }

        public string ArtistName { get; set; }

        public string Listeners { get; set; }

        public MusicObject() { }
    }
}