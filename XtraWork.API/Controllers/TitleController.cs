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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TitleResponse>> Create([FromBody] TitleRequest request)
        {
            TitleResponse response;

            try
            {
                response = await _titleService.Create(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _titleService.Delete(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TitleResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _titleService.GetAll(cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TitleResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await _titleService.Get(id, cancellationToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TitleResponse>> Update(Guid id, [FromBody] TitleRequest request, CancellationToken cancellationToken)
        {
            TitleResponse response;

            try
            {
                response = await _titleService.Update(id, request, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(response);
        }
    }
}