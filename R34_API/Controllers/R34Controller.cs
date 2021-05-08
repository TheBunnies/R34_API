using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using R34_API.Models;
using System;

namespace R34_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class R34Controller : Controller
    {
        private readonly R34Service _r34Service;
        public R34Controller(R34Service r34Service)
        {
            _r34Service = r34Service;
        }
        [HttpGet]
        public IActionResult Get([FromQuery]int page, [FromQuery]string[] tags)
        {
            try
            {
                var sources = _r34Service.GetSources(page, tags);
                var media = _r34Service.GetMedia(sources);
                var response = new Response
                {
                    Items = media,
                    MaxPages = _r34Service.GetMaxPageId(tags)
                };
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
