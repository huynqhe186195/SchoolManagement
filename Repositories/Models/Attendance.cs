using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? AttendanceDate { get; set; }

    public TimeOnly? TimeIn { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public int? AttendanceStatusId { get; set; }

    public int? HoursWorked { get; set; }

    public int? OvertimeHour { get; set; }

    public virtual AttendanceStatus? AttendanceStatus { get; set; }

    public virtual Employee? Employee { get; set; }
}
