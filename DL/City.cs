using System;
using System.Collections.Generic;

namespace DL;

public partial class City
{
    public int IdCity { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
