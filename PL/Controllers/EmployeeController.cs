using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public EmployeeController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public ActionResult GuardarCargaMasiva()
        {
            // Extraer la ruta del archivo correcto de la sesion
            string rutaExcel = HttpContext.Session.GetString("rutaCorrectosExcel");

            if (rutaExcel != null)
            {
                var baseConnection = _configuration.GetSection("ConnectionOleDb")["OleDbBase"];
                var connectionString = string.Format(baseConnection, rutaExcel);
                ML.Result resultExcel = BL.Employee.Excel(connectionString);

                foreach (ML.Employee itemExcel in resultExcel.Objects)
                {
                    BL.Employee.AddExcel(itemExcel);

                }
                HttpContext.Session.Remove("rutaCorrectosExcel");
            }



            return RedirectToAction("GetAll");
        }



        [HttpGet]
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
            Employee.Errores = new List<object>();
            Employee.Correctos = new List<object>();

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

        private async Task inicializarEmployee(ML.Employee employee)
        {
            employee.City ??= new ML.City();
            employee.Education ??= new ML.Education();
            employee.Gender ??= new ML.Gender();

            employee.JoiningYear = employee.JoiningYear;
            employee.PaymentTier = employee.PaymentTier;
            employee.Age = employee.Age;
            employee.EverBenched = employee.EverBenched;
            employee.Experience = employee.Experience;
            employee.LeaveOrNot = employee.LeaveOrNot;

            employee.City.IdCity = employee.City.IdCity ?? 0;
            employee.Education.IdEducation = employee.Education.IdEducation ?? 0;
            employee.Gender.IdGender = employee.Gender.IdGender ?? 0;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7180/api/Employee/GetAll", employee);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ML.Result>();
                if (result.Correct)
                {
                    employee.Employees = new List<object>();

                    foreach (var usuarioResult in result.Objects)
                    {
                        ML.Employee employeeObj = JsonConvert.DeserializeObject<ML.Employee>(usuarioResult.ToString());

                        employee.Employees.Add(employeeObj);

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

            employee.City.Cities = BL.City.GetAll().Objects ?? new List<object>();
            employee.Education.Educations = BL.Education.GetAll().Objects ?? new List<object>();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(ML.Employee employee, string tipoArchivo, IFormFile archivo)
        {
            employee.Errores = new List<object>();
            employee.Correctos = new List<object>();


            if (tipoArchivo == null)
            {
                await inicializarEmployee(employee);

            }
            else
            {
                string nombreArchivo = archivo.FileName;
                string extensionArchivo = Path.GetExtension(archivo.FileName);
                string extension = archivo.FileName.Split(".")[1];
                int numeroLinea = 2;
                if (archivo.FileName.Split(".")[1] == "xlsx")
                {
                    if (archivo != null)
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string nombreCompleto = Path.GetFileNameWithoutExtension(archivo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        var baseConnection = _configuration.GetSection("ConnectionOleDb")["OleDbBase"];
                        string rutaCompleta = Path.Combine(webRootPath, "Excel", nombreCompleto);
                        var connectionString = string.Format(baseConnection, rutaCompleta);

                        // Valido si existe mi archivo y lo guardo
                        if (!System.IO.File.Exists(rutaCompleta))
                        {
                            using (FileStream source = new FileStream(rutaCompleta, FileMode.Create))
                            {
                                archivo.CopyTo(source);
                            }
                        }
                        // Leer mi archivo de Excel.
                        ML.Result resultExcel = BL.Employee.Excel(connectionString);

                        //Validacion de los campos y mostrar errores y correctos
                        if (resultExcel.Correct)
                        {
                            foreach (ML.Employee excelObj in resultExcel.Objects)
                            {
                                string[] lineaLeida =
                                {
                                        excelObj.Education.IdEducation.ToString() ?? "",
                                        excelObj.JoiningYear.ToString() ?? "",
                                        excelObj.City.IdCity.ToString() ?? "",
                                        excelObj.PaymentTier.ToString() ?? "",
                                        excelObj.Age.ToString() ?? "",
                                        excelObj.Gender.IdGender.ToString() ?? "" ,
                                        excelObj.EverBenched ?? "",
                                        excelObj.Experience.ToString() ?? "",
                                        excelObj.LeaveOrNot.ToString() ?? "",

                                };
                                string resultValidacion = ValidarFila(lineaLeida);

                                if (resultValidacion.Contains("es correcto"))
                                {
                                    // agregar a la lista de correctos
                                    employee.Correctos.Add($"Linea{numeroLinea}|{resultValidacion}");

                                }
                                else
                                {
                                    // agregar a la lista de errores
                                    employee.Errores.Add($"Linea {numeroLinea}|{resultValidacion}");
                                }
                                numeroLinea++;
                            }
                        }
                        if (employee.Errores.Count == 0)
                        {
                            HttpContext.Session.SetString("rutaCorrectosExcel", rutaCompleta);
                        }
                    }
                }
            }
            await inicializarEmployee(employee);

            return View(employee);
        }



        public static string ValidarFila(string[] lineaLeida)
        {
            string error = "";

            if (!Regex.IsMatch(lineaLeida[0], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[0] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[1], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[1] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[2], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[2] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[3], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[3] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[4], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[4] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[5], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[5] + " |";
            }

            if (!Regex.IsMatch(lineaLeida[6], @"^[a-zA-Záéíóúñ]+$"))
            {
                error = error + "Solo se aceptan letras Yes o No en: " + lineaLeida[0] + " |";
            }
            
            if (!Regex.IsMatch(lineaLeida[7], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[7] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[8], @"^[a-zA-Záéíóúñ]+$"))
            {
                error = error + "Solo se aceptan True o False en: " + lineaLeida[8] + " |";
            }
            
            if (error != "")
            {
                return error;
            }
            else
            {
                return error += "El registro " + lineaLeida[0] + " es correcto";
            }
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
        
    }
}
