using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using SampleMovieApi.Models;

namespace SampleMovieApi.Services
{
    public interface IMovieService
    {
        Task<List<MetaDataModel>> GetMetaDataByMovieIdAsync(int movieId);

        Task<MetaDataModel> AddMetaData(MetaDataModel metadata);

        Task<List<StatsModel>> GetMovieStats();
    }
}
