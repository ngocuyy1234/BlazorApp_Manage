using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class Vlan
{
    public int Vlanid { get; set; }

    public string Vlanname { get; set; } = null!;

    public string? SubnetMask { get; set; }

    public string? GatewayIp { get; set; }

    public bool? Status { get; set; }

    public string? NetworkAddress { get; set; }

    public int? Cidr { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DeviceVlan> DeviceVlans { get; set; } = new List<DeviceVlan>();

    public virtual ICollection<NetworkConfig> NetworkConfigs { get; set; } = new List<NetworkConfig>();

    public virtual ICollection<Port> Ports { get; set; } = new List<Port>();
}
