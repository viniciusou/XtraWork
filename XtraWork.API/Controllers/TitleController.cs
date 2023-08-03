using Microsoft.AspNetCore.Mvc;
using XtraWork.API.Requests;
using XtraWork.API.Responses;
using XtraWork.API.Services;

namespace XtraWork.API.Controllers
{
    [ApiController]
    [Route("titles")]
    public class TitleController : ControllerBase
    {
        private readonly ITitleService _titleService;

        public TitleController(ITitleService titleService)
        {
            _titleService = titleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TitleResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _titleService.GetAll(cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TitleResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await _titleService.Get(id, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TitleResponse>> Create([FromBody] TitleRequest request)
        {
            var response = await _titleService.Create(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TitleResponse>> Update(Guid id, [FromBody] TitleRequest request, CancellationToken cancellationToken)
        {
            var response = await _titleService.Update(id, request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _titleService.Delete(id);
            return NoContent();
        }
    }
}