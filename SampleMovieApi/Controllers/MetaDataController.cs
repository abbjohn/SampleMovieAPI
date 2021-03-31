using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok("value");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] string metadata)
        {
            return Ok();
        }
    }
}
