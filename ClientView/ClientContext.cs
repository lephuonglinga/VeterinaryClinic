using System;
using VeterinaryClinic.Models;

namespace VeterinaryClinic
{
    public class ClientContext
    {
        public static Client? CurrentClient { get; set; }
    }
}
