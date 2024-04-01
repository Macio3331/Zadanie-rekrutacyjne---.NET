using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<TagsController> _logger;
        private bool _firstSeeded = false;

        public TagsController(ITagsService service, ILogger<TagsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagModel>>> Get([FromQuery]int page = 1, [FromQuery]string sortBy = "name", [FromQuery]string order = "asc")
        {
            if(!_firstSeeded)
            {
                _firstSeeded = true;
                try
                {
                    await _service.Seed();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    //TODO: changing ActionResult
                    return BadRequest();
                }
            }
            IEnumerable<TagModel> tags = new List<TagModel>();
            try
            {
                tags = await _service.GetTags(page, sortBy, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //TODO: changing ActionResult
                return BadRequest();
            }
            return Ok(tags);
        }

        // POST: api/tags
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            try
            {
                _firstSeeded = true;
                await _service.Seed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //TODO: changing ActionResult
                return BadRequest();
            }
            
            return Ok();
        }
    }
}
