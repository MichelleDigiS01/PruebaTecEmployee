using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult GetAll()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportChartsToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var chartBL = new BL.Chart(new DL.PruebaTecnicaContext());

                // Género
                var genderResult = chartBL.GetGender();
                var wsGender = workbook.Worksheets.Add("Género");
                wsGender.Cell(1, 1).Value = "Género";
                wsGender.Cell(1, 2).Value = "Total Empleados";
                int row = 2;
                foreach (ML.GenderReport item in genderResult.Objects)
                {
                    wsGender.Cell(row, 1).Value = item.Gender;
                    wsGender.Cell(row, 2).Value = item.TotalEmployees;
                    row++;
                }

                // Ciudad
                var cityResult = chartBL.GetCity();
                var wsCity = workbook.Worksheets.Add("Ciudad");
                wsCity.Cell(1, 1).Value = "Ciudad";
                wsCity.Cell(1, 2).Value = "Total Empleados";
                row = 2;
                foreach (ML.CityReport item in cityResult.Objects)
                {
                    wsCity.Cell(row, 1).Value = item.City;
                    wsCity.Cell(row, 2).Value = item.TotalEmployees;
                    row++;
                }

                // Educación
                var eduResult = chartBL.GetEducation();
                var wsEdu = workbook.Worksheets.Add("Educación");
                wsEdu.Cell(1, 1).Value = "Educación";
                wsEdu.Cell(1, 2).Value = "Total Empleados";
                row = 2;
                foreach (ML.EducationReport item in eduResult.Objects)
                {
                    wsEdu.Cell(row, 1).Value = item.Education;
                    wsEdu.Cell(row, 2).Value = item.TotalEmployees;
                    row++;
                }

                // Edad Promedio por Ciudad
                var ageResult = chartBL.GetAverageAge();
                var wsAge = workbook.Worksheets.Add("Edad Promedio");
                wsAge.Cell(1, 1).Value = "Ciudad";
                wsAge.Cell(1, 2).Value = "Edad Promedio";
                row = 2;
                foreach (ML.AverageAgeReport item in ageResult.Objects)
                {
                    wsAge.Cell(row, 1).Value = item.City;
                    wsAge.Cell(row, 2).Value = item.AverageAge;
                    row++;
                }

                // Correlación Experiencia/Pago
                var expResult = chartBL.GetCorrelationExperiencePayment();
                var wsExp = workbook.Worksheets.Add("Experiencia/Pago");
                wsExp.Cell(1, 1).Value = "Tier Pago";
                wsExp.Cell(1, 2).Value = "Experiencia Promedio";
                wsExp.Cell(1, 3).Value = "Total Empleados";
                row = 2;
                foreach (ML.EmployeeExperienceTier item in expResult.Objects)
                {
                    wsExp.Cell(row, 1).Value = item.PaymentTier;
                    wsExp.Cell(row, 2).Value = item.AvgExperience;
                    wsExp.Cell(row, 3).Value = item.TotalEmployees;
                    row++;
                }

                // Predicción de abandono
                var leaveResult = chartBL.GetLeavePrediction();
                var wsLeave = workbook.Worksheets.Add("Predicción Abandono");
                wsLeave.Cell(1, 1).Value = "Porcentaje de Abandono";
                wsLeave.Cell(2, 1).Value = leaveResult.Object?.ToString() ?? "N/A";

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "GraficasEmpleados.xlsx");
                }
            }
        }
    }
}
