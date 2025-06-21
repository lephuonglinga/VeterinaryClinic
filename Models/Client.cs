using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Client
{
    public string Username { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual User UsernameNavigation { get; set; } = null!;
}
