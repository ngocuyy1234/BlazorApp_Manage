using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class FirmwareStandard
{
    public int TypeId { get; set; }

    public string? LatestVersion { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public virtual DeviceType Type { get; set; } = null!;
}
