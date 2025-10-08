using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PL.Controllers
{
    public class BenchedController : Controller
    {
        private readonly HttpClient _httpClient;

        public BenchedController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Employee employee = new ML.Employee();
            employee.Employees = new List<object>();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(string Benched)
        {
            ML.Employee Employee = new ML.Employee();

            var response = await _httpClient.GetAsync("https://localhost:7180/api/Employee/benched?Benched="+ Benched);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ML.Result>();

                if (result != null && result.Correct)
                {
                    Employee.Employees = new List<object>();

                    foreach (var item in result.Objects)
                    {
                        ML.Employee emp = JsonConvert.DeserializeObject<ML.Employee>(item.ToString());
                        Employee.Employees.Add(emp);
                    }
                }
                else
                {
                    ViewBag.Error = result?.ErrorMessage ?? "No se encontraron empleados benched.";
                }
            }
            else
            {
                ViewBag.Error = "Error al conectar con la API.";
            }

            return View(Employee);
        }
    }
}
