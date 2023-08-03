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

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetAll(cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await _employeeService.Get(id, cancellationToken);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EmployeeResponse>>> Search(string keyword)
        {
            var response = await _employeeService.Search(keyword);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] EmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = await _employeeService.Create(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Update(Guid id, [FromBody] EmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = await _employeeService.Update(id, request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _employeeService.Delete(id);
            return NoContent();
        }
    }
}