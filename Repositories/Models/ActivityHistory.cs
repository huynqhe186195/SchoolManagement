using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class ActivityHistory
{
    public int HistoryId { get; set; }

    public int? EmployeeId { get; set; }

    public string? Action { get; set; }

    public string? Target { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual Employee? Employee { get; set; }
}
