using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdRol { get; set; }

    public int IdEmployee { get; set; }

    public string? Email { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
