using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? CreateBy { get; set; }

    public string? Content { get; set; }

    public DateOnly? SendAt { get; set; }

    public TimeOnly? Time { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Employee? CreateByNavigation { get; set; }

    public virtual Department? Department { get; set; }
}
