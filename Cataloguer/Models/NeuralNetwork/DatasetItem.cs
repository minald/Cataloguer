using System.Collections.Generic;
using System.Linq;

namespace Cataloguer.Models.NeuralNetwork
{
    public class DatasetItem
    {
        public float[] InputData { get; set; } = new float[7];

        public float[] OutputData { get; set; }

        public DatasetItem(ApplicationUser user, List<Rating> ratings, List<Track> tracks, int outputLayerLength)
        {
            InputData = user.GetInputData();
            OutputData = new float[outputLayerLength];
            InitializeOutputData(ratings, tracks);
        }

        public void InitializeOutputData(List<Rating> ratings, List<Track> tracks)
        {
            int maxRank = ratings.Select(r => r.Rank).Max();
            List<Track> userTracks = ratings.Select(r => r.Track).ToList();
            int i = 0;
            foreach (Track track in tracks.OrderBy(t => t.Id))
            {
                if (userTracks.Contains(track))
                {
                    Rating rating = ratings.First(r => r.Track.Id == track.Id);
                    OutputData[i] = (float)(2 * maxRank - rating.Rank) / (2 * maxRank);
                }
                else
                {
                    OutputData[i] = 0;
                }
                
                i++;
            }
        }
    }
}
