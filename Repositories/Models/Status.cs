using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string? StatusDescription { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
