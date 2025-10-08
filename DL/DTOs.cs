using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class DTOs
    {
        public int IdEmployee { get; set; }
        public int? JoiningYear { get; set; }
        public byte? PaymentTier { get; set; }
        public int? Age { get; set; }
        public string? EverBenched { get; set; }
        public byte? Experience { get; set; }
        public bool LeaveOrNot { get; set; }
        public int? IdCity { get; set; }
        public string? CityName { get; set; }
        public int? IdEducation { get; set; }
        public string? EducationName { get; set; }
        public int? IdGender { get; set; }
        public string? GenderName { get; set; }

    }

    public class LoginDTO
    {

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? RolName { get; set; }


    }

    public class GenderReport
    {
        public string? Gender { get; set; }
        public int TotalEmployees { get; set; }
    }

    public class CityReport
    {
        public string? City { get; set; }
        public int TotalEmployees { get; set; }
    }

    public class EducationReport
    {
        public string? Education { get; set; }
        public int TotalEmployees { get; set; }
    }

    public class AverageAgeReport
    {
        public string? City { get; set; }
        public decimal AverageAge { get; set; }
    }

    public class ExperienceTierReport
    {
        public byte PaymentTier { get; set; }
        public decimal AvgExperience { get; set; }
        public int TotalEmployees { get; set; }
    }
    public class EmployeesBenched
    {
        public int IdEmployee { get; set; }
        public int JoiningYear { get; set; }
        public byte Experience { get; set; }
        public string? CityName { get; set; }
        public string? EducationName { get; set; }
        public string? GenderName { get; set; }
    }

    public class LeavePrediction
    {
        public decimal LeavePercentage { get; set; }
    }
}
