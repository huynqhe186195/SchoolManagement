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
    /// Interaction logic for EmployeeAttendanceManagement.xaml
    /// </summary>
    public partial class EmployeeAttendanceManagement : Window
    {
        AttendanceServices attendanceServices = new AttendanceServices();
        EmployeeServices employeeServices = new EmployeeServices();
        public EmployeeAttendanceManagement()
        {
            InitializeComponent();
            LoadDate();
            LoadMonths();
            LoadYears();
        }
        private Dictionary<string, int> monthMapping = new Dictionary<string, int>
        {
            { "January", 1 },
            { "February", 2 },
            { "March", 3 },
            { "April", 4 },
            { "May", 5 },
            { "June", 6 },
            { "July", 7 },
            { "August", 8 },
            { "September", 9 },
            { "October", 10 },
            { "November", 11 },
            { "December", 12 }
        };
        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            cboMonth.SelectedIndex = -1;
            cboYear.SelectedIndex = -1;
            txtEmployeeName.Text = "";
        }

        private void employeeDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
                employeeDataGrid.ItemsSource = attendanceServices.employeeAttendanceSummaries().Where(a=>a.Month==DateTime.Now.Month && a.Year==DateTime.Now.Year);
            
        }

        private void cboMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fillter();
        }
        private void LoadMonths()
        {
            cboMonth.ItemsSource = new List<string>(monthMapping.Keys);
        }
        private void LoadYears()
        {
            // Tạo danh sách năm, ví dụ từ 2000 đến 2030
            List<int> years = new List<int>();
            for (int year = 2000; year <= 2030; year++)
            {
                years.Add(year);
            }

            // Gán danh sách năm cho ComboBox
            cboYear.ItemsSource = years;
        }
        private void LoadDate()
        {
            txtDate.Text = "Date: "+DateTime.Now.Month + "/" + DateTime.Now.Year;
        }
        private void cboYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fillter();
        }

        public void Fillter()
        {
            List<AttendanceSummary> attendanceSummaries = attendanceServices.employeeAttendanceSummaries();
            String name= txtEmployeeName.Text;
            if (name.Length > 0)
            {
                attendanceSummaries= attendanceSummaries.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(name.ToLower()) && e.Month == DateTime.Now.Month && e.Year == DateTime.Now.Year).ToList();
            }

            int month = -1;
            int year = -1;
            if (cboMonth.SelectedIndex > -1)
            {
                month = monthMapping[cboMonth.SelectedItem.ToString()];
            }
            if (cboYear.SelectedIndex > -1)
            {
                year = int.Parse(cboYear.SelectedItem.ToString());
            }
            if (month > -1 && year > -1)
            {
                attendanceSummaries = attendanceSummaries.Where(e => e.Month == month && e.Year == year).ToList();
            }else if (month > -1 )
            {
                attendanceSummaries = attendanceSummaries.Where(e => e.Month == month).ToList();
            }else if (year > -1)
            {
                attendanceSummaries=attendanceSummaries.Where(e => e.Year == year).ToList();
            }
            employeeDataGrid.ItemsSource = attendanceSummaries;
        }

        

        

        

        
        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayManagement leaveDayManagement = new LeaveDayManagement();
            leaveDayManagement.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
        }

        private void employeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmployeeAttendanceInMonth employeeAttendanceInMonth = new EmployeeAttendanceInMonth();
            AttendanceSummary selected_attendance = employeeDataGrid.SelectedItem as AttendanceSummary;
                if(selected_attendance != null)
                {
                    Application.Current.Properties["selected_employee"] = selected_attendance;
                    employeeAttendanceInMonth.ShowDialog();
                }
                
                
            
        }

        private void txtEmployeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Fillter();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
