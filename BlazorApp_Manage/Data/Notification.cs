using System;
using System.Collections.Generic;

namespace BlazorApp_Manage.Data;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int AccountId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? ActionLink { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
