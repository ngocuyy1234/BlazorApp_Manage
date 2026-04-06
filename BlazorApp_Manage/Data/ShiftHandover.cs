using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class ShiftHandover
{
    public int HandoverId { get; set; }

    public DateTime HandoverTime { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public string? ShiftType { get; set; }

    public string? Site { get; set; }

    public string? ServerStatus { get; set; }

    public string? NetworkStatus { get; set; }

    public string? StorageStatus { get; set; }

    public string? SoftwareStatus { get; set; }

    public string? PowerCoolingStatus { get; set; }

    public string? PendingTasks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Account FromAccount { get; set; } = null!;

    public virtual Account ToAccount { get; set; } = null!;
}
