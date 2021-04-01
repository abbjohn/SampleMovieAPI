using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleMovieApi.Models;
using SampleMovieApi.Services;

namespace SampleMovieApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private readonly ILogger<MetaDataController> _logger;
        private readonly IMovieService _movieService;

        public MetaDataController(ILogger<MetaDataController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [HttpGet("{movieId}", Name = "GetMovieMetadata")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int movieId)
        {
            var metaData = await _movieService.GetMetaDataByMovieIdAsync(movieId);

            if (metaData == null || metaData.Count == 0)
            {
                return NotFound();
            }

            return Ok(metaData);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(MetaDataModel metadata)
        {
            var newMovie = await _movieService.AddMetaData(metadata);

            return CreatedAtRoute("GetMovieMetadata", new { movieId = newMovie.MovieId.Value }, newMovie);
        }
    }
}
