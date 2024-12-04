using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class JobPosition
{
    public int JobPositionId { get; set; }

    public string? JobPositionName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
