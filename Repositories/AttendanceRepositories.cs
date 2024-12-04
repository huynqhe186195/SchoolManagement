using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AttendanceRepositories
    {
        Prn212Context context = new Prn212Context();
        public List<Attendance> GetAll()
        {
            return context.Attendances.Include(e=>e.Employee).Include(a=>a.AttendanceStatus).ToList();
        }
        
        public List<AttendanceSummary> employeeAttendanceSummaries()
        {
            var attendance = context.Attendances;

            var result = (from a in attendance
                          group a by new { a.EmployeeId, a.Employee.FirstName,a.Employee.LastName, a.AttendanceDate.Value.Year, a.AttendanceDate.Value.Month } into g
                          select new AttendanceSummary
                          {
                              EmployeeId = (int)g.Key.EmployeeId,
                              FirstName = g.Key.FirstName,
                              LastName = g.Key.LastName,
                              Month = g.Key.Month,
                              Year = g.Key.Year,
                              AbsentDays = g.Count(a => a.AttendanceStatusId == 2),
                              PresentDays = g.Count(a => a.AttendanceStatusId == 1),
                              LateDays = g.Count(a => a.AttendanceStatusId == 3),
                              OvertimeDays = g.Count(a => a.AttendanceStatusId == 4),
                          }).ToList();

            return result;
        }
        public List<Attendance> GetAttendanceInMonth(int EmployeeId, int Month, int Year)
        {
            return context.Attendances.Where(a => a.EmployeeId == EmployeeId && a.AttendanceDate.Value.Year == Year && a.AttendanceDate.Value.Month == Month).Include(e => e.Employee).Include(a => a.AttendanceStatus).ToList();
        }
    }
}
