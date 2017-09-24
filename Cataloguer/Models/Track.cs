namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PageLink { get; set; }

        public int Rank { get; set; }

        public string Duration { get; set; }

        public string Listeners { get; set; }

        public string Scrobbles { get; set; }

        //For binding in databases
        public virtual Album Album { get; set; }

        //For binding in databases
        public virtual Artist Artist { get; set; }

        public Track() {}
    }
}