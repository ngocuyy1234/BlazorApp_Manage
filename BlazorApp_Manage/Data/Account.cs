using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual ICollection<DutySchedule> DutyScheduleAccounts { get; set; } = new List<DutySchedule>();

    public virtual ICollection<DutySchedule> DutyScheduleCreatedByNavigations { get; set; } = new List<DutySchedule>();

    public virtual ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ShiftHandover> ShiftHandoverFromAccounts { get; set; } = new List<ShiftHandover>();

    public virtual ICollection<ShiftHandover> ShiftHandoverToAccounts { get; set; } = new List<ShiftHandover>();
}
