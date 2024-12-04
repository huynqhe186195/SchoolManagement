using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SalaryService
    {
        private readonly Prn212Context context = new Prn212Context();

        public SalaryService(Prn212Context _context)
        {
            context = _context;
        }
        public List<Salary> GetAllEmployeeSalaries()
        {
            return context.Salaries.Include(s => s.Employee).ToList();
        }
        public List<Salary> GetEmployeeSalaries()
        {
            return context.Salaries.Include(s => s.Employee).ToList();
        }
        public int CalculateTotalIncome(int basicSalary, int? allowance, int? bonus, int? penalty)
        {
            return basicSalary + (allowance ?? 0) + (bonus ?? 0) - (penalty ?? 0);
        }

        public void AddSalary(Salary salary)
        {
            salary.TotalIncome = CalculateTotalIncome(salary.BasicSalary ?? 0, salary.Allowance, salary.Bonus, salary.Penalty);
            context.Salaries.Add(salary);
            context.SaveChanges();
        }

        public void UpdateSalary(Salary salary)
        {
            context.ChangeTracker.Clear();
            salary.TotalIncome = CalculateTotalIncome(salary.BasicSalary ?? 0, salary.Allowance, salary.Bonus, salary.Penalty);
            context.Salaries.Update(salary);
            context.SaveChanges();
        }

        public List<Salary> GetSalaryHistoryByEmployeeId(int employeeId)
        {
            return context.Salaries
                          .Where(s => s.EmployeeId == employeeId)
                          .OrderByDescending(s => s.PaymentDate)
                          .ToList();
        }
    }
}
