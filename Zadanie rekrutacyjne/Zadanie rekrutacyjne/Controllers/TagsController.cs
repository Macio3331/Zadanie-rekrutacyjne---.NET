using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zadanie_rekrutacyjne.Controllers
{
    /// <summary>
    /// Controller managing the operations connected with tags.
    /// </summary>
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        /// <summary>
        /// Service object for tags' operations.
        /// </summary>
        private readonly ITagsService _service;
        /// <summary>
        /// Object storing information that the database was at least once supplied with data from StackOverflow API.
        /// </summary>
        private readonly WasLoadedModel _loadedModel;
        /// <summary>
        /// Default constructor of controller.
        /// </summary>
        /// <param name="service">Service instance with scoped range.</param>
        /// <param name="loadedModel">Model object with singleton range.</param>
        public TagsController(ITagsService service, WasLoadedModel loadedModel)
        {
            _service = service;
            _loadedModel = loadedModel;
        }
        /// <summary>
        /// Endpoint used for getting a proper page of tags sorted by name or share in all population in ascending or descending order. 
        /// </summary>
        /// <param name="page">Number of page to get.</param>
        /// <param name="sortBy">Specifies if data should be sorted by count share in population or by name. Accepted values: "name", "share".</param>
        /// <param name="order">Specifies the order of sorting. Accepts values: "asc" (ascending order), "desc" (descending order).</param>
        /// <returns>List of 50 TagModel objects enclosed in ResultObject (status code and body of response).</returns>
        /// <response code="200">Returns the list of tags.</response>
        /// <response code="400">Returned when there are some mistakes in query string.</response> 
        /// <response code="500">Returned when there is an error on server-side.</response> 
        // GET: api/tags
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TagModel>>> GetTagsAsync([FromQuery][Required]int page, [FromQuery][Required]string sortBy, [FromQuery][Required]string order)
        {
            if(page < 0) return BadRequest("Bad query parameter.");

            if (!_loadedModel.WasLoaded)
            {
                var result = await SeedData();
                if (!result.Key) return StatusCode(500, result.Value);
                _loadedModel.WasLoaded = true;
            }

            IEnumerable<TagModel> tags = new List<TagModel>();
            try
            {
                tags = await _service.GetTagsAsync(page, sortBy, order, 50);
            }
            catch(ArgumentException ex)
            {
                Log.Information(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, "Unexpected problem occured.");
            }

            return Ok(tags);
        }
        /// <summary>
        /// Endpoint that is used for resending request to StackOverflow API that makes program upload data into database.
        /// </summary>
        /// <returns>ResultObject (status code and body of response).</returns>
        /// <response code="200">Returned when the action was completed successfully.</response>
        /// <response code="500">Returned when there is an error on server-side.</response> 
        // POST: api/tags
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> FetchNewTagsAsync()
        {
            var result = await SeedData();
            if (!result.Key) return StatusCode(500, result.Value);
            return Ok();
        }
        /// <summary>
        /// Private method used whenever there is a need to file a database with new data from StackOverflow API.
        /// </summary>
        /// <returns>Boolean value specifying if the seeding was fortunate.</returns>
        private async Task<KeyValuePair<bool, string>> SeedData()
        {
            try
            {
                await _service.SeedAsync();
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex.Message);
                return new KeyValuePair<bool, string>(false, "Unable to fetch API's data.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new KeyValuePair<bool, string>(false, "Internal server error. Unexpected problem occured");
            }
            return new KeyValuePair<bool, string>(true, "");
        }
    }
}
