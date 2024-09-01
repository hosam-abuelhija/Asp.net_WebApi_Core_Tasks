using System;
using System.Collections.Generic;

namespace apitask2.Models;

public partial class UserRole
{
    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
