using Microsoft.Win32;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using MaterialDesignThemes.Wpf;
using Repositories.Models;
using Services;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    

    public partial class SentLeaveRequest : Window
    {
        EmployeeServices employeeServices = new EmployeeServices();
        RoleServices roleServices = new RoleServices();
        LeaveTypeServices leaveTypeService = new LeaveTypeServices();   
        LeaveRequestServices leaveRequestServices = new LeaveRequestServices();
        

        public Employee selected_employee { get; set; } = null;
        string uri_after_upload_file = "";
        public SentLeaveRequest()
        {
            selected_employee = Application.Current.Properties["saemployee"] as Employee;
            InitializeComponent();
            Load_Image("phuong.jpg");
        }



        public void Load_Image(String uri)
        {
            String fullPath = Path.GetFullPath("Images");
            int lastIndex = 0;
            for (int i = 0; i < fullPath.Length; i++)
            {
                if (fullPath[i] == '\\')
                {
                    lastIndex = i;
                }
            }
            int startSubStringIndex = lastIndex - 24;
            String filePath = fullPath.Substring(0, startSubStringIndex) + "Images";
            String fileName = filePath + "\\" + uri; //Lấy absolute path của Image để ko phải thay đổi Build Action => Resource
            ibImage.ImageSource = new BitmapImage(new Uri(fileName));
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            LoadRole();
            LoadLeaveType();
            LoadEmployeeInfo();
            LoadLeaveRequest();
        }

        public void LoadEmployeeInfo()
        {
            Load_Image(selected_employee.Photo);
            cboRole.SelectedValue = selected_employee.RoleId;
            txtFirstName.Text = selected_employee.FirstName;    
            txtLastName.Text = selected_employee.LastName;
            txtUsername.Text = selected_employee.FirstName;
            txtAvailableDays.Text = selected_employee.AvailableLeaveDays+"";
        }



        public void LoadRole()
        {
            cboRole.ItemsSource = roleServices.GetRoles();
            cboRole.DisplayMemberPath = "RoleName";
            cboRole.SelectedValuePath = "RoleId";
        }


        public void LoadLeaveType()
        {
            cboLeaveType.ItemsSource = leaveTypeService.getAllLeaveType();
            cboLeaveType.DisplayMemberPath = "LeaveTypeName";
            cboLeaveType.SelectedValuePath = "LeaveTypeId";
            cboLeaveType.SelectedIndex = 0; 

        }







        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            selected_employee = null;
            this.Close();
        }

        public double validateDate(DateTime? startDate, DateTime? enDate)
        {
            TimeSpan t = (TimeSpan)(enDate - startDate);
            double ndays = t.TotalDays;
            return ndays;
        }


        public void LoadLeaveRequest()
        {
            DataGridRequest.ItemsSource = leaveRequestServices.getAllLeaveRequestByEmployeeId(selected_employee.EmployeeId).OrderByDescending(e => e.SubmitedOn);

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int employeeId = selected_employee.EmployeeId;
            string leaveTypeIdstring = cboLeaveType.SelectedValue + "";
            string startDate = dpStartDate.Text;
            string endDate = dpEndDate.Text;
            if(string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(leaveTypeIdstring)) {
                MessageBox.Show("Please input all fields", "Empty Fields", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            if (validateDate(DateTime.Today, dpStartDate.SelectedDate) < 0)
            {
                MessageBox.Show("Start Date must >= Today", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (validateDate(dpStartDate.SelectedDate, dpEndDate.SelectedDate) < 0)
            {
                MessageBox.Show("End Date must > Start Date", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(validateDate(dpStartDate.SelectedDate, dpEndDate.SelectedDate) > selected_employee.AvailableLeaveDays)
            {
                MessageBox.Show("Your available leave days are not enough!", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int leaveTypeId = int.Parse(leaveTypeIdstring);
            DateOnly dostartDate = DateOnly.Parse(startDate);
            DateOnly doEndDate = DateOnly.Parse(endDate);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            //MessageBox.Show(today + "");

            LeaveRequest leaveRequest = new LeaveRequest();
            leaveRequest.EmployeeId = employeeId;
            leaveRequest.LeaveTypeId = leaveTypeId;
            leaveRequest.StartDate = dostartDate;
            leaveRequest.EndDate = doEndDate;   
            leaveRequest.SubmitedOn = today;
            leaveRequest.RequestStatusId = 1;

            leaveRequestServices.AddLeaveRequest(leaveRequest);
            LoadLeaveRequest();
            MessageBox.Show("Your request is in process", "Sent successfull", MessageBoxButton.OK, MessageBoxImage.Information);



        }
    }
}