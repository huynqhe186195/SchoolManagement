using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class LeaveRequest
{
    public int RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly? SubmitedOn { get; set; }

    public int? RequestStatusId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual LeaveType? LeaveType { get; set; }

    public virtual RequestStatus? RequestStatus { get; set; }
}
