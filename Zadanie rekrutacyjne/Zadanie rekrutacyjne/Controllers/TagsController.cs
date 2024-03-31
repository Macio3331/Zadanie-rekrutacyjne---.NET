using Microsoft.AspNetCore.Mvc;
using Zadanie_rekrutacyjne.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zadanie_rekrutacyjne.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private ITagsService _service;
        private ILogger<TagsController> _logger;
        private TagsSeeder _seeder;

        TagsController(ITagsService service, ILogger<TagsController> logger, TagsSeeder seeder)
        {
            _service = service;
            _logger = logger;
            _seeder = seeder;
        }

        // GET: api/tags
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        // POST api/tags
        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                _seeder.Seed();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
            
            return Ok();
        }
    }
}
