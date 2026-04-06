using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class NetworkConfig
{
    public int ConfigId { get; set; }

    public int DeviceId { get; set; }

    public string Ipaddress { get; set; } = null!;

    public string? SubnetMask { get; set; }

    public string? DefaultGateway { get; set; }

    public string? PrimaryDns { get; set; }

    public string? SecondaryDns { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Vlanid { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Vlan? Vlan { get; set; }
}
