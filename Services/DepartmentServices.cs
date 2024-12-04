using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DepartmentServices
    {
        Prn212Context context = new Prn212Context();

        public List<Department> GetDepartments()
        {
            return context.Departments.ToList();
        }

        public Department? GetDepartmentById(int? id)
        {
            return context.Departments.FirstOrDefault(x => x.DepartmentId == id);
        }

        public void AddDepartment(Department department)
        {
            context.Departments.Add(department);
            context.SaveChanges();
        }

        public void AssignEmployeeToDepartment(int employeeId, int departmentId)
        {
            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                employee.DepartmentId = departmentId;
                context.SaveChanges();
            }
        }
    }
}
