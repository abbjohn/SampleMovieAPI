namespace SampleMovieApi.Models
{
    public class StatsModel
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public double? AverageWatchDurationS { get; set; }

        public int? Watches { get; set; }

        public int? ReleaseYear { get; set; }
    }
}
