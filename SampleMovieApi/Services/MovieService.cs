using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using SampleMovieApi.Models;

namespace SampleMovieApi.Services
{
    public class MovieService : IMovieService
    {
        private const string METADATA_FILEPATH = "./Data/metadata.csv";
        private const string STATS_FILEPATH = "./Data/stats.csv";
        private readonly Database _database;

        public MovieService(Database database)
        {
            _database = database;
        }

        public async Task<List<MetaDataModel>> GetMetaDataByMovieIdAsync(int movieId)
        {
            List<MetaDataModel> filteredMetaData = new List<MetaDataModel>();

            // Get the data we want from the "database"
            var workingMetaData = _database.MetaData.Where(x => x.MovieId == movieId);

            // Perform filtering, grouping, and ordering
            // Filter out incomplete data
            workingMetaData = workingMetaData.Where(x =>
                    !string.IsNullOrEmpty(x.Title)
                    && !string.IsNullOrEmpty(x.Language)
                    && !string.IsNullOrEmpty(x.Duration)
                    && x.ReleaseYear != null);

            // Only get latest piece of metadata for a given langauge
            workingMetaData = workingMetaData.GroupBy(x => x.Language).Select(y => y.OrderByDescending(z => z.Id).First());

            // Order the metadata by langauge
            workingMetaData = workingMetaData.OrderBy(x => x.Language);

            filteredMetaData = workingMetaData.ToList();

            return filteredMetaData;
        }

        public async Task<MetaDataModel> AddMetaData(MetaDataModel metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            var newId = _database.MetaData.Select(x => x.Id).OrderBy(x => x).LastOrDefault() + 1;
            metadata.Id = newId;

            _database.MetaData.Add(metadata);

            return metadata;
        }

        public async Task<List<StatsModel>> GetMovieStats()
        {
            var metadata = new List<MetaDataModel>();
            var stats = new List<StatsDataModel>();

            using (var reader = new StreamReader(METADATA_FILEPATH))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecordsAsync<MetaDataModel>();
                metadata = await records.ToListAsync();
            }

            using (var reader = new StreamReader(STATS_FILEPATH))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<StatsDataModelMap>();
                var records = csv.GetRecordsAsync<StatsDataModel>();
                stats = await records.ToListAsync();
            }

            // Filter out incomplete data
            var workingMetaData = metadata.Where(x =>
                    !string.IsNullOrEmpty(x.Title)
                    && !string.IsNullOrEmpty(x.Language)
                    && !string.IsNullOrEmpty(x.Duration)
                    && x.ReleaseYear != null);

            // Only get latest piece of metadata for a given langauge
            workingMetaData = workingMetaData.GroupBy(x => new { x.MovieId, x.Language }).Select(y => y.OrderByDescending(z => z.Id).First());


            var workingStats = (from m in workingMetaData
                    join s in stats on m.MovieId equals s.MovieId
                    into t
                    select new StatsModel
                    {
                        MovieId = m.MovieId.Value,
                        Title = m.Title,
                        ReleaseYear = m.ReleaseYear,
                        Watches = t.Count(),
                        AverageWatchDurationS = TimeSpan.FromMilliseconds(t.Average(x => x.WatchDurationMs)).TotalSeconds
                    });

            var completeStats = workingStats.OrderByDescending(x => x.Watches).ThenByDescending(y => y.ReleaseYear).ToList();

            return completeStats;
        }
    }
}
