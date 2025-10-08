using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployeeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            ML.Employee Employee = new ML.Employee
            {
                JoiningYear = null,
                PaymentTier = null,
                Age = null,
                EverBenched = null,
                Experience = null,
                LeaveOrNot = null,
                City = new ML.City { IdCity = 0 },
                Education = new ML.Education { IdEducation = 0 },
                Gender = new ML.Gender { IdGender = 0 }
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7180/api/Employee/GetAll", Employee);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ML.Result>();

                if (result.Correct)
                {
                    Employee.Employees = new List<object>();

                    foreach (var employeObj in result.Objects)
                    {
                        ML.Employee employee = JsonConvert.DeserializeObject<ML.Employee>(employeObj.ToString());

                        Employee.Employees.Add(employee);

                    }
                    
                }
                else
                {
                    ViewBag.Error = result?.ErrorMessage ?? "No se pudieron obtener los empleados.";
                }
            }
            else
            {
                ViewBag.Error = "No se pudieron obtener los empleados.";
            }

            // Cargar combos auxiliares
            Employee.City.Cities = BL.City.GetAll().Objects ?? new List<object>();
            Employee.Education.Educations = BL.Education.GetAll().Objects ?? new List<object>();

            return View(Employee);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(ML.Employee employee)
        {
            employee.City ??= new ML.City();
            employee.Education ??= new ML.Education();
            employee.Gender ??= new ML.Gender();

            employee.JoiningYear = employee.JoiningYear ;
            employee.PaymentTier = employee.PaymentTier ;
            employee.Age = employee.Age ;
            employee.EverBenched = employee.EverBenched;
            employee.Experience = employee.Experience;
            employee.LeaveOrNot = employee.LeaveOrNot;

            employee.City.IdCity = employee.City.IdCity ?? 0;
            employee.Education.IdEducation = employee.Education.IdEducation ?? 0;
            employee.Gender.IdGender = employee.Gender.IdGender ?? 0;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7180/api/Employee/GetAll", employee);

            ML.Employee model = new ML.Employee();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ML.Result>();
                if (result.Correct)
                {
                    model.Employees = new List<object>();

                    foreach (var usuarioResult in result.Objects)
                    {
                        ML.Employee employeeObj = JsonConvert.DeserializeObject<ML.Employee>(usuarioResult.ToString());

                        model.Employees.Add(employeeObj);

                    }

                }
                else
                {
                    ViewBag.Error = result?.ErrorMessage ?? "No se pudieron obtener los empleados.";
                }
            }
            else
            {
                ViewBag.Error = "No se pudieron obtener los empleados.";
            }

            ML.Result resultCities = BL.City.GetAll();
            model.City = new ML.City();
            model.City.Cities = resultCities.Correct && resultCities.Objects != null
                ? resultCities.Objects
                : new List<object>();

            ML.Result resulEdu = BL.Education.GetAll();
            model.Education = new ML.Education();
            model.Education.Educations = resulEdu.Correct && resulEdu.Objects != null
                ? resulEdu.Objects
                : new List<object>();

            

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Form(int? IdEmployee)
        {
            ML.Employee employee = new ML.Employee();

            ML.Result resultCities = BL.City.GetAll();
            if (resultCities.Correct)
            {
                employee.City = new ML.City();
                employee.City.Cities = resultCities.Objects;
            }

            ML.Result resulEdu = BL.Education.GetAll();
            if (resulEdu.Correct)
            {
                employee.Education = new ML.Education();
                employee.Education.Educations = resulEdu.Objects;
            }

            if (IdEmployee > 0)
            {
                var response = await _httpClient.GetAsync($"https://localhost:7180/api/Employee/GetById?IdEmployee={IdEmployee}");

                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<ML.Result>();

                    if (result.Correct)
                    {
                        ML.Employee employeeObj = JsonConvert.DeserializeObject<ML.Employee>(result.Object.ToString());

                        employee = employeeObj;
                        employee.City.Cities = resultCities.Objects;
                        employee.Education.Educations = resulEdu.Objects;



                    }





                    //employee = await response.Content.ReadFromJsonAsync<ML.Employee>();

                    //// Solución: inicializa si es null
                    //if (employee.City == null)
                    //    employee.City = new ML.City();
                    //employee.City.Cities = resultCities.Objects;

                    //if (employee.Education == null)
                    //    employee.Education = new ML.Education();
                    //employee.Education.Educations = resulEdu.Objects;
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener el empleado.";
                }
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Form(ML.Employee employee)
        {
            if (employee.IdEmployee == 0)
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7180/api/Employee/Add", employee);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Error = "Error al guardar el empleado";
                    return View(employee);
                }
            }
            else
            {
                var response = await _httpClient.PutAsJsonAsync("https://localhost:7180/api/Employee/Update", employee);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Error = "Error al guardar el empleado";
                    return View(employee);
                }
            }
        }

        public async Task<IActionResult> Delete(int IdEmployee)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7180/api/Employee/Delete?IdEmployee={IdEmployee}");

            ML.Employee employee = null;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAll");
            }
            else
            {
                ViewBag.Error = "No se pudo obtener el empleado.";
            }
            return View(employee);
        }
        //[HttpGet]

        //public async Task<IActionResult> GetAll2()
        //{
        //    ML.Employee employee = new ML.Employee();
        //    employee.Employees = new List<ML.Employee>();

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:7180/api/Employee/");
        //        var responseTask = client.GetAsync("GetAll");
        //        responseTask.Wait();

        //        var result = responseTask.Result;

        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadFromJsonAsync<ML.Employee>();
        //            readTask.Wait();

        //            foreach (var resulItem in readTask.Result)
        //            {
        //                employee.Employees.Add(resulItem);

        //            }

        //        }
        //        else
        //        {
        //            ViewBag.Mensaje = "Erro en la obtencion de Registros";
        //        }
        //    }

        //    return View(employee);
        //}
    }
}
