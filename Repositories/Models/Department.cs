using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string? DepartmentAddress { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
