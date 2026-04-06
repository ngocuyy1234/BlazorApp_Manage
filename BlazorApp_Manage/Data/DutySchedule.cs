using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class DutySchedule
{
    public int ScheduleId { get; set; }

    public DateOnly ShiftDate { get; set; }

    public string ShiftType { get; set; } = null!;

    public string Site { get; set; } = null!;

    public int AccountId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Account? CreatedByNavigation { get; set; }
}
