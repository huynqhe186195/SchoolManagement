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
    

    public partial class EmployeeDetails : Window
    {
        EmployeeServices employeeServices = new EmployeeServices();
        DepartmentServices departmentServices = new DepartmentServices();
        JobpositionServices jobpositionServices = new JobpositionServices();
        RoleServices roleServices = new RoleServices();
        StatusServices statusServices = new StatusServices();
        ActivityHistoryServices activityHistoryServices = new ActivityHistoryServices();

        public Employee selected_employee { get; set; } = null;
        string uri_after_upload_file = "";
        public EmployeeDetails()
        {
            InitializeComponent();
            Load_Image("phuong.jpg");
        }

        private void txtTotalLeaveDays_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
            
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            uri_after_upload_file = UploadImages();
            //txtFirstName.Text = image_uri;  
        }


        public String UploadImages()
        {
            String saveInSQlPath = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmg;*.jpg;*.png";

            String fullPath = Path.GetFullPath("Images");  //Lấy ra absolute path của folder Image trong EmployeeWPF project

            //Tách chuỗi để lấy đúng tên nơi lưu trữ: bỏ đoạn "bin\Debug\net8.0-windows\" - 24 kí tự, trong fullpath
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

            if (openFileDialog.ShowDialog() == true)
            {
                string images_uri = openFileDialog.FileName;
                ibImage.ImageSource = new BitmapImage(new Uri(images_uri));


                string source = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(source);
                String destination = filePath + "\\" + Path.GetFileName(source);
                try
                {
                    fileInfo.CopyTo(destination);  //Copy file vào nơi lưu trữ

                }
                catch
                {
                    //Phòng trường hợp trùng tên file
                    saveInSQlPath = Path.GetFileName(source);
                }
                //C:\Users\Dell\source\repos\PRN_Project\EmployeeWPF\EImages\

                //Tách chuỗi Images để lưu trong database
                saveInSQlPath = Path.GetFileName(source);

            }
            return saveInSQlPath;
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
            LoadStatus();   
            LoadRole(); 
            LoadJobPosition();  
            LoadDepartment(); 
            LoadEmployeeInfo();
        }

        public void LoadEmployeeInfo()
        {
            if (selected_employee != null)
            {
                txtUsername .Text = selected_employee.UserName; 
                txtPassword.Password = "***************";
                txtFirstName.Text = selected_employee.FirstName;    
                txtLastName.Text = selected_employee.LastName;  
                txtAddress.Text = selected_employee.Address;    
                txtEmail.Text = selected_employee.Email;    
                txtPhoneNumber.Text = selected_employee.PhoneNumber;    
                txtAvailableDays.Text = selected_employee.AvailableLeaveDays+"";
                txtTotalLeaveDays.Text = selected_employee.TotalLeaveDays + "";


                dpDob.Text = selected_employee.Dob + "";
                dpStartDate.Text = selected_employee.StartDate + "";
                

                cboRole.SelectedValue = selected_employee.RoleId;
                cboDepartment.SelectedValue = selected_employee.DepartmentId;
                cboJobPosition.SelectedValue = selected_employee.JobPositionId;
                cboStatus.SelectedValue = selected_employee.StatusId;

                Load_Image(selected_employee.Photo);

                if (selected_employee.Gender==true)
                {
                    rbMale.IsChecked = true;
                }
                else
                {
                    rbFemale.IsChecked = true;
                }
                
            }
        }


        public void LoadRole()
        {
            cboRole.ItemsSource = roleServices.GetRoles();
            cboRole.DisplayMemberPath = "RoleName";
            cboRole.SelectedValuePath = "RoleId";
        }

        public void LoadStatus()
        {
              
            cboStatus.ItemsSource = statusServices.GetStatuses();
            cboStatus.DisplayMemberPath = "StatusDescription";
            cboStatus.SelectedValuePath = "StatusId";
        }

        public void LoadJobPosition()
        {
            
            cboJobPosition.ItemsSource = jobpositionServices.GetJobPositions();
            cboJobPosition.DisplayMemberPath = "JobPositionName";
            cboJobPosition.SelectedValuePath = "JobPositionId";
        }


        public void LoadDepartment()
        {
           
            cboDepartment.ItemsSource = departmentServices.GetDepartments().Where(d => d.DepartmentId!=6);
            cboDepartment.DisplayMemberPath = "DepartmentName";
            cboDepartment.SelectedValuePath = "DepartmentId";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            selected_employee = null;
            this.Close();
        }

        
        public bool CheckEmptyFields()
        {
            bool result = false;
            string department = cboDepartment.SelectedValue + "";
            string dob = dpDob.Text;
            string startDate = dpStartDate.Text;
            string jobposition = cboJobPosition.SelectedValue+"";
            if (string.IsNullOrEmpty(department) || string.IsNullOrEmpty(dob) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(jobposition))
            {
                MessageBox.Show("Please input all fields", "Empty Fields", MessageBoxButton.OK, MessageBoxImage.Error);
                result = true;
            }

            return result;
        }

        public bool CheckDuplicate(Employee employee)
        {
            employee = null;
            employee = employeeServices.checkDuplicateUsername(txtUsername.Text, selected_employee.EmployeeId);
            if (employee != null)
            {
                MessageBox.Show("This username has already existed!", "Existing username", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            employee = null;
            employee = employeeServices.getEmployeeByEmail(txtEmail.Text, selected_employee.EmployeeId);
            if (employee != null)
            {
                MessageBox.Show("This email has already exist!", "Existing email", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            employee = null;
            employee = employeeServices.getEmployeeByPhoneNumber(txtPhoneNumber.Text, selected_employee.EmployeeId);
            if (employee != null)
            {
                MessageBox.Show("This phone number has already exist!", "Existing phone number", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        public bool CheckEmailFormat()
        {
            bool result = true;
            string email = txtEmail.Text;
            if (email.Contains("@") == false)
            {
                MessageBox.Show("Email is wrong format!", "Wrong format email", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            return result;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Employee employee =  new Employee();
            selected_employee.EmployeeId = selected_employee.EmployeeId;
            selected_employee.UserName = selected_employee.UserName;
            selected_employee.Password = selected_employee.Password;
            if(uri_after_upload_file.Length > 0)
            {
                selected_employee.Photo = uri_after_upload_file;
            }


            if (CheckEmptyFields())
            {
                MessageBox.Show("Please fill all the Input!", "Fill all input", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (txtFirstName.Text.Length == 0 || txtLastName.Text.Length==0 || txtAddress.Text.Length==0 || txtEmail.Text.Length==0
                || txtPhoneNumber.Text.Length==0 || txtAvailableDays.Text.Length==0 || txtTotalLeaveDays.Text.Length == 0)
            {
                MessageBox.Show("Please fill all the Input!", "Fill all input", MessageBoxButton.OK, MessageBoxImage.Information);
            } else 
            {

                if (CheckDuplicate(employee) == true)
                {
                    return;
                }

                if (CheckEmailFormat() == false)
                {
                    return;
                }
                selected_employee.FirstName = txtFirstName.Text;
                selected_employee.LastName = txtLastName.Text;
                selected_employee.Email = txtEmail.Text;
                selected_employee.PhoneNumber = txtPhoneNumber.Text;
                selected_employee.Address = txtAddress.Text;

                selected_employee.Dob = DateOnly.Parse(dpDob.Text);
                selected_employee.StartDate = DateOnly.Parse(dpStartDate.Text);
                selected_employee.TotalLeaveDays = int.Parse(txtTotalLeaveDays.Text);
                selected_employee.AvailableLeaveDays = int.Parse(txtAvailableDays.Text);
                selected_employee.JobPositionId = int.Parse(cboJobPosition.SelectedValue + "");
                selected_employee.DepartmentId = int.Parse(cboDepartment.SelectedValue + "");
                selected_employee.RoleId = int.Parse(cboRole.SelectedValue + "");
                selected_employee.StatusId = int.Parse(cboStatus.SelectedValue + "");
                selected_employee.IsActive = true;
                
                if (rbMale.IsChecked == true)
                {
                    selected_employee.Gender = true;
                }
                else
                {
                    selected_employee.Gender = false;
                }

                employeeServices.UpdateEmployee(selected_employee);
                SaveInHistory(selected_employee);
                MessageBox.Show("Update successful!", "Update successful", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadEmployeeInfo();
            }

            

        }

        public void SaveInHistory(Employee employee)
        {
            //Luu history
            
            Employee admin = Application.Current.Properties["admin"] as Employee;
            ActivityHistory activityHistory = new ActivityHistory();
            activityHistory.EmployeeId = admin.EmployeeId;
            activityHistory.Action = "Update";
            activityHistory.Target = "Update Employee with id = " + employee.EmployeeId;
            activityHistory.Date = DateOnly.FromDateTime(DateTime.Now);
            activityHistory.Time = TimeOnly.FromDateTime(DateTime.Now);
            activityHistoryServices.addActivityHistory(activityHistory);
        }

        private void txtPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
        }
    }
}