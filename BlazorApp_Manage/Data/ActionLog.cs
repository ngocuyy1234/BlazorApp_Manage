using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace BlazorApp_Manage.Data;

public partial class ActionLog
{
    public long LogId { get; set; }

    public int AccountId { get; set; }

    public string? ActionType { get; set; }

    public string? Description { get; set; }

    public int? DeviceId { get; set; }

    public DateTime? LogTime { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Device? Device { get; set; }
}
