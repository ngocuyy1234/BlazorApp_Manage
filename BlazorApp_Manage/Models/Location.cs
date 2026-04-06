using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Building { get; set; } = null!;

    public string? Floor { get; set; }

    public string? Room { get; set; }

    public string? Rack { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
