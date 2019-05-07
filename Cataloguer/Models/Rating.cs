namespace Cataloguer.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Track Track { get; set; }

        public int Rank { get; set; }
    }
}
