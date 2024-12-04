using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class RequestStatus
{
    public int RequestStatusId { get; set; }

    public string? RequestStatusName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
