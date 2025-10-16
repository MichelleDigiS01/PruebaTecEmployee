using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Employee
    {
        public int IdEmployee { get; set; }

        public int? JoiningYear { get; set; }

        public byte? PaymentTier { get; set; }

        public int? Age { get; set; }

        public string? EverBenched { get; set; }

        public byte? Experience { get; set; }

        public bool? LeaveOrNot { get; set; }
        public List<object>? Employees { get; set; }
        public List<object>? Errores { get; set; }
        public List<object>? Correctos { get; set; }

        public ML.City? City { get; set; }
        public ML.Education? Education { get; set; }
        public ML.Gender? Gender { get; set; }
    }
}
