using CsvHelper.Configuration;

namespace SampleMovieApi.Models
{
    public class StatsDataModelMap : ClassMap<StatsDataModel>
    {
        public StatsDataModelMap()
        {
            Map(m => m.MovieId).Name("movieId");
            Map(m => m.WatchDurationMs).Name("watchDurationMs");
        }
    }
}
