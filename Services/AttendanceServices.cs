using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Services
{
    public class AttendanceServices
    {
        Prn212Context context = new Prn212Context();
        public void AddAttendance(Attendance attendance)
        {
            context.ChangeTracker.Clear();
            context.Attendances.Add(attendance);
            context.SaveChanges();
        }
        public void UpdateAttendance(Attendance attendance)
        {
            context.ChangeTracker.Clear();
            context.Attendances.Update(attendance);
            context.SaveChanges();
        }
        public Attendance? CheckAttendance(DateOnly AttendanceDate, int EmployeeId)
        {
            return context.Attendances.Where(a =>  a.AttendanceDate == AttendanceDate && a.EmployeeId == EmployeeId).FirstOrDefault();
        }
        
        public List<Attendance> GetAttendances() => new AttendanceRepositories().GetAll();

        public List<Attendance> GetAttendanceInMonth(int EmployeeId, int month, int year) => new AttendanceRepositories().GetAttendanceInMonth(EmployeeId, month, year);

        public List<AttendanceSummary> employeeAttendanceSummaries() => new AttendanceRepositories().employeeAttendanceSummaries();


    }
}
