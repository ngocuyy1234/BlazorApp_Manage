using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class Port
{
    public int PortId { get; set; }

    public int DeviceId { get; set; }

    public int PortNumber { get; set; }

    public string? PortName { get; set; }

    public string? Status { get; set; }

    public int? Vlanid { get; set; }

    public int? ConnectedToDeviceId { get; set; }

    public string? Description { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Device? ConnectedToDevice { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Vlan? Vlan { get; set; }
}
