using BL;
using DL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ML;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly BL.Chart _context;

        public ChartsController(BL.Chart chart)
        {
            _context = chart;
        }

        [HttpGet("gender")]
        public IActionResult GetGenderDistribution()
        {

            ML.Result result = _context.GetGender();

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [HttpGet("city")]
        public IActionResult GetCityDistribution()
        {
            ML.Result result = _context.GetCity();

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("education")]
        public IActionResult GetEducationDistribution()
        {
            ML.Result result= _context.GetEducation();

            if (result.Correct)
            {
                return Ok(result);

            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("averageAge")]
        public IActionResult GetAverageAgeByCity()
        {
            ML.Result result = _context.GetAverageAge();
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
        [Route("CorrelationExperiencePayment")]
        public IActionResult CorrelationExperiencePayment()
        {
            ML.Result result = _context.GetCorrelationExperiencePayment();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("leavePediction")]
        public IActionResult GetLeavePrediction()
        {
            ML.Result result = _context.GetLeavePrediction();

            if (result.Correct)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}
