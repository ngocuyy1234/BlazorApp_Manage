using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlazorApp_Manage.Data;

public partial class Vlan
{
    public int Vlanid { get; set; }

    public string Vlanname { get; set; } = null!;

    public string? SubnetMask { get; set; }

    public string? GatewayIp { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<DeviceVlan> DeviceVlans { get; set; } = new List<DeviceVlan>();

    public virtual ICollection<Port> Ports { get; set; } = new List<Port>();

    [NotMapped]
    public int TotalDevices => DeviceVlans?.Count ?? 0;
}
