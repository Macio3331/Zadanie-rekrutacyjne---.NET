using Microsoft.AspNetCore.Mvc;
using Serilog;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zadanie_rekrutacyjne.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _service;
        private readonly WasLoadedModel _loadedModel;

        public TagsController(ITagsService service, WasLoadedModel loadedModel)
        {
            _service = service;
            _loadedModel = loadedModel;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagModel>>> Get([FromQuery(Name = "page")]int page = 1, [FromQuery(Name = "sortBy")]string sortBy = "name", [FromQuery(Name = "order")]string order = "asc")
        {
            if (!_loadedModel.WasLoaded)
            {
                if (!await SeedData()) return StatusCode(500, "Unexpected problem occured.");
                _loadedModel.WasLoaded = true;
            }
            IEnumerable<TagModel> tags = new List<TagModel>();
            try
            {
                tags = await _service.GetTags(page, sortBy, order);
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

        // POST: api/tags
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            if (!await SeedData()) return StatusCode(500, "Unexpected problem occured.");
            return Ok();
        }

        private async Task<bool> SeedData()
        {
            try
            {
                await _service.Seed();
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
            return true;
        }
    }
}
