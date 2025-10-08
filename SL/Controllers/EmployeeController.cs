using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly BL.Employee _employee;

        public EmployeeController(BL.Employee employee)
        {
            _employee = employee;
        }
        
        [HttpPost]
        [Route("GetAll")]
        public IActionResult GetAll([FromBody] ML.Employee employee)
        {
            ML.Result result = _employee.GetAll(employee);

            if (result.Correct)
            {
                return Ok(result); // opbjects, correct
            }
            else
            {
                return BadRequest(result); // error, ex, correct
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] ML.Employee employee)
        {

            ML.Result result = _employee.Add(employee);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPut] //Crea o remplza un regsitro 
        [Route("Update")]
        public IActionResult Update([FromBody] ML.Employee employee)
        {

            ML.Result result = _employee.Update(employee);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int IdEmployee)
        {

            ML.Result result = _employee.Delete(IdEmployee);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int IdEmployee)
        {
            var result = _employee.GetById(IdEmployee);
            if (result.Correct)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet]
        [Route("benched")]
        public IActionResult Benched(string Benched)
        {
            var result = _employee.GetByBenched(Benched);   
            if (result.Correct)
                return Ok(result);
            else
                return BadRequest(result);
        }   

    }
}
