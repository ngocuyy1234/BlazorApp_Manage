using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class DeviceVlan
{
    public int DeviceId { get; set; }

    public int Vlanid { get; set; }

    public DateTime? AssignmentDate { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Vlan Vlan { get; set; } = null!;
}
