using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public bool? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public int? JobPositionId { get; set; }

    public int? DepartmentId { get; set; }

    public string? Photo { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? TotalLeaveDays { get; set; }

    public int? AvailableLeaveDays { get; set; }

    public int? StatusId { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }

    [NotMapped]
    public bool IsSelected { get; set; }

    public virtual ICollection<ActivityHistory> ActivityHistories { get; set; } = new List<ActivityHistory>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual JobPosition? JobPosition { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual Status? Status { get; set; }
}
