using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class AttendanceStatus
{
    public int AttendanceStatusId { get; set; }

    public string? StatusName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
