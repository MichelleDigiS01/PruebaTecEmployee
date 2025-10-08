using System;
using System.Collections.Generic;

namespace DL;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public int? JoiningYear { get; set; }

    public byte? PaymentTier { get; set; }

    public int? Age { get; set; }

    public string EverBenched { get; set; } = null!;

    public byte? Experience { get; set; }

    public bool? LeaveOrNot { get; set; }

    public int? IdCity { get; set; }

    public int? IdEducation { get; set; }

    public int? IdGender { get; set; }

    public virtual City IdCityNavigation { get; set; } = null!;

    public virtual Education IdEducationNavigation { get; set; } = null!;

    public virtual Gender IdGenderNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
