using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int? EmployeeId { get; set; }

    public int? BasicSalary { get; set; }

    public int? Allowance { get; set; }

    public int? Bonus { get; set; }

    public int? Penalty { get; set; }

    public int? TotalIncome { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public virtual Employee? Employee { get; set; }
}
