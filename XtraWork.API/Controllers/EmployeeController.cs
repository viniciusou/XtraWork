using Microsoft.AspNetCore.Mvc;
using XtraWork.API.Requests;
using XtraWork.API.Responses;
using XtraWork.API.Services;

namespace XtraWork.API.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] EmployeeRequest request, CancellationToken cancellationToken)
        {
            EmployeeResponse response;

            try
            {
                response = await _employeeService.Create(request, cancellationToken);
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
            await _employeeService.Delete(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetAll(cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await _employeeService.Get(id, cancellationToken);
            return response == null ? NotFound() : Ok(response);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmployeeResponse>>> Search(string keyword)
        {
            var response = await _employeeService.Search(keyword);
            return response.Count == 0 ? NotFound() : Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeResponse>> Update(Guid id, [FromBody] EmployeeRequest request, CancellationToken cancellationToken)
        {
            EmployeeResponse response;

            try 
            {
                response = await _employeeService.Update(id, request, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(response);
        }
    }
}