using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
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

    public class EmployeeExperienceTier
    {
        public int PaymentTier { get; set; }
        public decimal AvgExperience { get; set; }
        public int TotalEmployees { get; set; }
    }

}
