using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeServices
    {
        Prn212Context context = new Prn212Context();    
        public Employee? getEmployeeByUserName(string username)
        {
            Employee employee = context.Employees.FirstOrDefault(x => x.UserName.ToLower().Equals(username.ToLower()));  

            return employee;    
           
        }


        public List<Employee> getEmployees()
        {
           return context.Employees.ToList();
        }

        public int getTotalEmployee()
        {
            return context.Employees.Count();
        }

        public void UpdateEmployee(Employee employee)
        {
 
            context.ChangeTracker.Clear();
            context.Employees.Update(employee);
            context.SaveChanges();  

        }

        public void AddEmployee(Employee employee)
        {

            context.ChangeTracker.Clear();
            context.Employees.Add(employee);
            context.SaveChanges();

        }

        public Employee? checkDuplicateUsername(string username, int id)
        {
            return context.Employees.Where(e => e.UserName.ToLower().Equals(username.ToLower()) && e.EmployeeId!=id).FirstOrDefault();
        }

        public Employee? getEmployeeByEmail(string email, int id)
        {
            return context.Employees.Where(e => e.Email.ToLower().Equals(email.ToLower()) && e.EmployeeId != id).FirstOrDefault();
        }

        public Employee? getEmployeeByPhoneNumber(string phone, int id)
        {
            return context.Employees.Where(e => e.PhoneNumber.ToLower().Equals(phone.ToLower()) && e.EmployeeId != id).FirstOrDefault();
        }


        public Employee? checkDuplicateUsername(string username)
        {
            return context.Employees.Where(e => e.UserName.ToLower().Equals(username.ToLower())).FirstOrDefault();
        }

        public Employee? getEmployeeByEmail(string email)
        {
            return context.Employees.Where(e => e.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();
        }

        public Employee? getEmployeeByPhoneNumber(string phone)
        {
            return context.Employees.Where(e => e.PhoneNumber.ToLower().Equals(phone.ToLower())).FirstOrDefault();
        }

        public List<Employee> GetAllEmployeeLeaveDay()
        {
            return context.Employees
                .Include(e => e.Department)
                .ToList();
        }

        public Employee GetEmployeeById(int? id)
        {
            return context.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.EmployeeId == id);
        }


    }
}
