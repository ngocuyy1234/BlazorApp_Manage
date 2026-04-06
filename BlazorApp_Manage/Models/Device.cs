using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Models;

public partial class Device
{
    public int DeviceId { get; set; }

    public string DeviceName { get; set; } = null!;

    public int DeviceTypeId { get; set; }

    public int? LocationId { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public string? Macaddress { get; set; }

    public string? FirmwareVersion { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual DeviceType DeviceType { get; set; } = null!;

    public virtual ICollection<DeviceVlan> DeviceVlans { get; set; } = new List<DeviceVlan>();

    public virtual Location? Location { get; set; }

    public virtual NetworkConfig? NetworkConfig { get; set; }

    public virtual ICollection<Port> PortConnectedToDevices { get; set; } = new List<Port>();

    public virtual ICollection<Port> PortDevices { get; set; } = new List<Port>();
}
