using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool IsActive { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
