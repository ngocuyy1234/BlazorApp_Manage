using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class DeviceType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual FirmwareStandard? FirmwareStandard { get; set; }
}
