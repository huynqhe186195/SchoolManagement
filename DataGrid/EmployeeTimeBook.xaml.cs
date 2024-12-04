using Microsoft.Identity.Client.NativeInterop;
using Repositories.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for EmployeeTimeBook.xaml
    /// </summary>
    public partial class EmployeeTimeBook : Window
    {
        EmployeeServices es= new EmployeeServices();
        AttendanceServices attendanceServices = new AttendanceServices();
        public EmployeeTimeBook()
        {
            InitializeComponent();
            LoadAttendance();
            AddAbsentAttendance();
        }
        private TimeOnly InTime = TimeOnly.Parse("06:00");
        private TimeOnly EndTime = TimeOnly.Parse("23:00");
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Attendance attendance = new Attendance();
            DateOnly today= DateOnly.FromDateTime (DateTime.Now);
            TimeOnly timeIn= TimeOnly.FromDateTime(DateTime.Now);
            Employee? loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
            Attendance checkedAttendance = attendanceServices.CheckAttendance(today, loginEmployee.EmployeeId);
            if (checkedAttendance == null)
            {
                attendance.EmployeeId = loginEmployee.EmployeeId;
                attendance.AttendanceDate = today;
                attendance.TimeIn = timeIn;
                attendance.TimeOut = null;
                attendance.HoursWorked = 0;
                attendance.OvertimeHour = 0;
                if (timeIn > InTime)
                {
                    attendance.AttendanceStatusId = 3;
                }
                else
                {
                    attendance.AttendanceStatusId = 1;
                }
                attendanceServices.AddAttendance(attendance);
                LoadAttendance();
            }
            else
            {
                MessageBox.Show("You have already signed in today.");
                return;
            }
        }


        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly timeOut = TimeOnly.FromDateTime(DateTime.Now);
            Employee? loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
            Attendance checkedAttendance = attendanceServices.CheckAttendance(today, loginEmployee.EmployeeId);
            if (checkedAttendance == null)
            {
                MessageBox.Show("You haven't signed in today.");
                return;
            }
            else if(checkedAttendance != null && checkedAttendance.TimeOut != null) 
            {
                MessageBox.Show("You have already signed out today.");
                return;
            }else if(checkedAttendance != null && checkedAttendance.TimeOut == null)
            {
                
                checkedAttendance.TimeOut = timeOut;
                TimeSpan timeWorked = timeOut - checkedAttendance.TimeIn.Value;
                checkedAttendance.HoursWorked = (int)Math.Floor(timeWorked.TotalHours);
                if((int)Math.Floor(timeWorked.TotalHours)>8)
                {
                    checkedAttendance.AttendanceStatusId = 4;
                    TimeSpan standardHours = TimeSpan.FromHours(8);
                    TimeSpan timeOver = timeWorked - standardHours;
                    checkedAttendance.OvertimeHour = (int)Math.Floor(timeOver.TotalHours);
                }
                else
                {
                    checkedAttendance.OvertimeHour=0;
                }
                attendanceServices.UpdateAttendance(checkedAttendance);
                LoadAttendance();
            }

        }
        public void LoadAttendance()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            Employee? loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
            
            dataGridAttendanceHistory.ItemsSource = attendanceServices.GetAttendances().Where(a => a.EmployeeId == loginEmployee.EmployeeId && a.AttendanceDate == today);
        }

        private void AddAbsentAttendance()
        {
            DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            List<Employee> AllEmployees = es.getEmployees().Where(e=>e.RoleId==2).ToList();
            if (yesterday.DayOfWeek == DayOfWeek.Sunday)
            {
                return; 
            }
            foreach (var employee in AllEmployees)
            {
                Attendance checkedAttendance = attendanceServices.CheckAttendance(yesterday, employee.EmployeeId);

                if (checkedAttendance == null)
                {
                    Attendance attendance = new Attendance
                    {
                        EmployeeId = employee.EmployeeId,
                        AttendanceDate = yesterday,
                        TimeIn = null,
                        TimeOut = null,
                        HoursWorked = 0,
                        OvertimeHour = 0,
                        AttendanceStatusId = 2
                    };

                    attendanceServices.AddAttendance(attendance);
                }
            }
        }



        private void dataGridAttendanceHistory_Loaded(object sender, RoutedEventArgs e)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            Employee? loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;

            dataGridAttendanceHistory.ItemsSource = attendanceServices.GetAttendances().Where(a => a.EmployeeId == loginEmployee.EmployeeId && a.AttendanceDate == today);
        }

        private void dataGridAttendanceHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
