using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Chart
    {
        private readonly DL.PruebaTecnicaContext _context;

        public Chart(DL.PruebaTecnicaContext context)
        {
            _context = context;
        }
        public ML.Result GetGender()
        {
            ML.Result result = new ML.Result();

            try
            {

                var employeGender = _context.GenderDTO.FromSqlInterpolated($"EXEC EmployeeGender").ToList();

                if (employeGender != null)
                {
                    result.Objects = new List<object>();

                    foreach (var item in employeGender)
                    {
                        ML.GenderReport report = new ML.GenderReport();
                        report.Gender = item.Gender;
                        report.TotalEmployees = item.TotalEmployees;

                        result.Objects.Add(report);
                    }

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        public ML.Result GetCity()
        {
            ML.Result result = new ML.Result();

            try
            {
                var employeCity = _context.CityDTO.FromSqlInterpolated($"EXEC EmployeeCity").ToList();


                if (employeCity != null)
                {
                    result.Objects = new List<object>();

                    foreach (var item in employeCity)
                    {
                        ML.CityReport report = new ML.CityReport();
                        report.City = item.City;
                        report.TotalEmployees = item.TotalEmployees;

                        result.Objects.Add(report);
                    }

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        public ML.Result GetEducation()
        {
            ML.Result result = new ML.Result();

            try
            {
                var query = _context.EducationDTO.FromSqlRaw("EXEC EmployeeEducation").ToList();

                if (query != null)
                {
                    result.Objects = new List<object>();

                    foreach (var item in query)
                    {
                        ML.EducationReport report = new ML.EducationReport();
                        report.Education = item.Education;
                        report.TotalEmployees = item.TotalEmployees;

                        result.Objects.Add(report);
                    }

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
        public ML.Result GetAverageAge()
        {
            ML.Result result = new ML.Result();

            try
            {

                var query = _context.AverageDTO.FromSqlRaw("EXEC EmployeeAverageAgeByCity").ToList();


                if (query != null)
                {
                    result.Objects = new List<object>();

                    foreach (var item in query)
                    {
                        ML.AverageAgeReport report = new ML.AverageAgeReport();
                        report.City = item.City;
                        report.AverageAge = item.AverageAge;

                        result.Objects.Add(report);
                    }

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result GetCorrelationExperiencePayment()
        {
            ML.Result result = new ML.Result();
            try
            {
                var experienciaPago = _context.ExperienceTierDTO.FromSqlInterpolated($"EXEC EmployeeExperienceTier").ToList();

                if (experienciaPago.Count > 0)
                {

                    result.Objects = new List<object>();
                    foreach (var obj in experienciaPago)
                    {
                        ML.EmployeeExperienceTier employeeExperienceTier = new ML.EmployeeExperienceTier();

                        employeeExperienceTier.PaymentTier = obj.PaymentTier;
                        employeeExperienceTier.AvgExperience = obj.AvgExperience;
                        employeeExperienceTier.TotalEmployees = obj.TotalEmployees;

                        result.Objects.Add(employeeExperienceTier);
                        result.Correct = true;
                    }
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result GetLeavePrediction()
        {
            ML.Result result = new ML.Result();

            try
            {
                var query = _context.LeavePredictionDTO
                    .FromSqlRaw("EXEC LeavePrediction")
                    .AsEnumerable()
                    .FirstOrDefault();

                if (query != null)
                {
                    result.Object = query.LeavePercentage;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró información de predicción de abandono.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
