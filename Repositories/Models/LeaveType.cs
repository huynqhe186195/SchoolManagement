using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string? LeaveTypeName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
