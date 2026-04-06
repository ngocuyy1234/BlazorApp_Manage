using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class IncidentReport
{
    public int ReportId { get; set; }

    public DateTime DetectedTime { get; set; }

    public int ReporterId { get; set; }

    public int? DeviceId { get; set; }

    public string? Site { get; set; }

    public string? Severity { get; set; }

    public string Description { get; set; } = null!;

    public string? ImpactScope { get; set; }

    public string? RootCause { get; set; }

    public string? ActionsTaken { get; set; }

    public DateTime? FixTime { get; set; }

    public string? Status { get; set; }

    public string? PreventiveMeasures { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Device? Device { get; set; }

    public virtual Account Reporter { get; set; } = null!;
}
