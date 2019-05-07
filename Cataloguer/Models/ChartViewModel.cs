using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class ChartViewModel
    {
        public List<Track> Chart { get; set; }

        public List<Track> UserRating { get; set; }

        public ChartViewModel(List<Track> chart, List<Track> userRating)
        {
            Chart = chart;
            UserRating = userRating;
        }
    }
}
