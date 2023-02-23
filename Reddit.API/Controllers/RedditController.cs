using Microsoft.AspNetCore.Mvc;
using Reddit.Core.Interfaces;

namespace Reddit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;
        private readonly INewsService _threadService;

        public RedditController(IAuthorizeService authorizeService, INewsService threadService)
        {
            _authorizeService = authorizeService;
            _threadService = threadService;
        }

        [HttpGet]
        [Route("news")]
        public async Task<IActionResult> GetThreads()
        {

            var token = await _authorizeService.Authorize();

            if (token != null)
            {
                var result = await _threadService.Get(token);

                return Ok($"{result}");
            }

            return Ok();

        }
    }
}
