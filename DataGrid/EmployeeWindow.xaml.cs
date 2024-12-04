using Repositories.Models;
using Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        NotificationService notificationService = new NotificationService();
        public Employee selected_employee { get; set; } = null;
        public EmployeeWindow()
        {
            InitializeComponent();

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


        //Close Button Click Event
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            LoginForm lf = new LoginForm();
            this.Close();
            lf.Show();
            
        }


        //Load data
        EmployeeServices employeeServices = new EmployeeServices();
        DepartmentServices departmentServices = new DepartmentServices();
        JobpositionServices jobpositionServices = new JobpositionServices();
        RoleServices roleServices = new RoleServices();

        public void LoadAllEmployee()
        {
           //employeeDataGrid.Items.Clear();
           employeeDataGrid.ItemsSource = employeeServices.getEmployees().Where(e => e.EmployeeId==selected_employee.EmployeeId);
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


        public void LoadData()
        {
            selected_employee = Application.Current.Properties["saemployee"] as Employee;
            Application.Current.Properties["employee"] = selected_employee;
            txtName.Text = "Employee: " + selected_employee.FirstName + " " + selected_employee.LastName;
            Load_Image(selected_employee.Photo);
            txtCountEmployee.Text = "1 Employee";
            LoadAllEmployee();

        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //btnAddEmployee.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"));
            //btnAddEmployee.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("black"));
            LoadData();
        }

        private void btnEmployeeDetail_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = employeeDataGrid.SelectedItem as Employee;

            EmployeeWindowDetails employeeDetail = new EmployeeWindowDetails();
            employeeDetail.selected_employee = employee;
            employeeDetail.ShowDialog();
            
            LoadData();

        }

        


        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAddWindow employeeAddWindow = new EmployeeAddWindow();
            employeeAddWindow.ShowDialog();
            LoadData(); 
            
        }

        private void btnHideEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to change Status of this Employee!", "Change Status", MessageBoxButton.YesNo, MessageBoxImage.Question); 
            if(result == MessageBoxResult.Yes)
            {
                
                Employee employee = employeeDataGrid.SelectedItem as Employee;
                employee.StatusId = 2;
                employeeServices.UpdateEmployee(employee);
                MessageBox.Show("Change successful!", "Change Status", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData(); 
            }

        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            EmployeeTimeBook employeeTimeBook = new EmployeeTimeBook();
            employeeTimeBook.ShowDialog();
        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            AddNotification addNotification = new AddNotification();
            addNotification.ShowDialog();
            LoadData();
        }

        private void btnSalary_Click(object sender, RoutedEventArgs e)
        {
            EmployeeSalary employeeSalary = new EmployeeSalary();
            employeeSalary.Show();
            LoadData();
        }

        private void btnNotification_Click(object sender, RoutedEventArgs e)
        {
            if(notificationDatagrid.Visibility == Visibility.Visible)
            {
                notificationDatagrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                notificationDatagrid.Visibility = Visibility.Visible;
            }


        }

        private void btnNotificationList_Click(object sender, RoutedEventArgs e)
        {
            EmployeeNotification employeeNotification = new EmployeeNotification();
            employeeNotification.Show();
          


        }

        private void notificationDatagrid_Loaded(object sender, RoutedEventArgs e)
        {
            var loginemployee=Application.Current.Properties["loginEmployee"] as Employee;

                  var notification = notificationService.GetTop3NotificationEmployee(loginemployee.DepartmentId);
                  notificationDatagrid.ItemsSource = notification;
        }

        private void btnSendLeaveRequest_Click(object sender, RoutedEventArgs e)
        {
            SentLeaveRequest sentLeaveRequest = new SentLeaveRequest();
            sentLeaveRequest.ShowDialog();

        }
    }

 

}

