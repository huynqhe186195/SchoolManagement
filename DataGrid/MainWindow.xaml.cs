using Repositories.Models;
using Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Xml.Linq;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Microsoft.Win32;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Employee selected_employee { get; set; } = null;
        public MainWindow()
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
            this.Close();
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
        }


        //Load data
        EmployeeServices employeeServices = new EmployeeServices();
        DepartmentServices departmentServices = new DepartmentServices();
        JobpositionServices jobpositionServices = new JobpositionServices();
        RoleServices roleServices = new RoleServices();

        public void LoadAllEmployee()
        {
           //employeeDataGrid.Items.Clear();
           employeeDataGrid.ItemsSource = employeeServices.getEmployees();
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

        public void LoadGender()
        {
            List<string> sList = new List<string>();
            sList.Add("Female");
            sList.Add("Male"); 
            cboGender.ItemsSource = sList;
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
            Application.Current.Properties["admin"] = selected_employee;
            txtName.Text = "Admin: "+selected_employee.FirstName+" "+selected_employee.LastName;
            Load_Image(selected_employee.Photo);
            txtCountEmployee.Text = employeeServices.getTotalEmployee() + " Employees";
            LoadAllEmployee();
            LoadDepartment();
            LoadJobPosition();
            LoadGender();
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

            EmployeeDetails employeeDetail = new EmployeeDetails();
            employeeDetail.selected_employee = employee;
            employeeDetail.ShowDialog();
            LoadAllEmployee();

        }

        public void FilterEmployee()
        {
            List<Employee> eList = employeeServices.getEmployees();
            string name = txtEmployeeName.Text;
            if (name.Length > 0)
            {

                eList = eList.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(name.ToLower())).ToList();
            }

            int departmentId = -1;
            int jobpositionId = -1;
            int gender = -1;

            if (cboDepartment.SelectedIndex > -1)
            {
                departmentId = int.Parse(cboDepartment.SelectedValue + "");
                eList = eList.Where(e => e.DepartmentId == departmentId).ToList();
            }
            if (cboJobPosition.SelectedIndex > -1)
            {
                jobpositionId = int.Parse(cboJobPosition.SelectedValue + "");
                eList = eList.Where(e => e.JobPositionId == jobpositionId).ToList();
            }
            if (cboGender.SelectedIndex > -1)
            {
                gender = cboGender.SelectedIndex;   // index = 1 => Male, Index = 0 => Female
                if (gender == 1)
                {
                    eList = eList.Where(e => e.Gender == true).ToList();
                }
                if (gender == 0)
                {
                    eList = eList.Where(e => e.Gender == false).ToList();
                }
            }
            // employeeDataGrid.Items.Clear(); 
            employeeDataGrid.ItemsSource = eList;
        }

        private void txtEmployeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEmployee();
        }

        private void cboDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployee();
        }

        private void cboJobPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployee();
        }

        private void cboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployee();
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            cboDepartment.SelectedIndex = -1;
            cboJobPosition.SelectedIndex = -1;  
            cboGender.SelectedIndex = -1;
            txtEmployeeName.Text = "";
            LoadAllEmployee();
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

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            NotifactionList notifactionList = new NotifactionList();
            notifactionList.Show();
            this.Close();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportFile();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            importFile();
        }


        public void ExportFile()
        {
            string filepath = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xlsx;*.xlsm;*.xls"; ;

            if (dialog.ShowDialog() == true)
            {
                filepath = dialog.FileName;
            }

            if (string.IsNullOrEmpty(filepath))
            {
                MessageBox.Show("Invalid file path", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelPackage package = new ExcelPackage();

                //tạo 1 sheet để làm việc trên đó
                package.Workbook.Worksheets.Add("Test export");

                //Lấy sheet vừa tạo để thao tác
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                //Đặt tên cho sheet
                sheet.Name = "Test export";
                sheet.Cells.Style.Font.Size = 13;  // Đặt font size 
                sheet.Cells.Style.Font.Name = "Times New Roman";


                //Tạo header cho file
                string[] headers = new string[] {
                "Employee Id",
                "Username",
                "FirstName",
                "LastName",
                "Email",
                "Gender",
                "Dob",
                "Address",
                "PhoneNumber",
                "Job Position Id",
                "Department Id",
                "Photo",
                "StartDate",
                "EndDate",
                "TotalLeaveDays",
                "AvailableLeaveDays",
                "Status Id",
                "Role Id",
                "Is Active"

            };

                var countColHeader = headers.Count();
                int colIndex = 1;
                int rowIndex = 1;

                //Add header
                foreach (var header in headers)
                {
                    //Lấy cell tại vị trị (row, col) ra
                    var cell = sheet.Cells[rowIndex, colIndex];

                    //Set backgroung cho header
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    cell.Value = header;    //Add header
                    cell.Style.Font.Bold = true;    //In đậm
                    colIndex++;

                }

                //Add value
                List<Employee> cList = employeeServices.getEmployees();

                rowIndex = 2;
                //Bắt đầu gán data từ datagrid vào excel bắt đầu từ dòng 2 cột 1
                foreach (Employee employee in cList)
                {
                    colIndex = 1;
                    sheet.Cells[rowIndex, colIndex].Value = employee.EmployeeId; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.UserName; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.FirstName; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.LastName; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.Email; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.Gender; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.Dob.ToString(); colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.Address; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.PhoneNumber; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.JobPositionId; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.DepartmentId; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.Photo; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.StartDate.ToString(); colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.EndDate.ToString(); colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.TotalLeaveDays; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.AvailableLeaveDays; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.StatusId; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.RoleId; colIndex++;
                    sheet.Cells[rowIndex, colIndex].Value = employee.IsActive; colIndex++;

                    rowIndex++;
                }

                //Lưu file excel
                Byte[] bin = package.GetAsByteArray();
                File.WriteAllBytes(filepath, bin);
                MessageBox.Show("Export successful", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);


            } catch
            {
                MessageBox.Show("Can not export to this file", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private void importFile()
        {
            string filepath = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; ;

            if (dialog.ShowDialog() == true)
            {
                filepath = dialog.FileName;
            }

            if (string.IsNullOrEmpty(filepath))
            {
                MessageBox.Show("Invalid file path", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


           
                List<Employee> cList = new List<Employee>();

                //Mở file Excel
                var package = new ExcelPackage(new FileInfo(filepath));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                //Trong file excel có nhiều sheet => lấy data từ sheet 0 đầu tiên
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


                //Duyệt từng dòng trong file excel, file excel bắt đầu từ dòng 1, ko phải từ 0
                //Bắt đầy lấy data từ dòng số 2 trong excel
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    // j => biểu thị cột
                    int j = 1;
                try
                {
                    //Lấy id tại vị trí (2,1): dòng 2 cột 1
                    int EmployeeId = int.Parse(worksheet.Cells[i, j].Value + ""); j++;
                    string UserName = worksheet.Cells[i, j].Value.ToString(); j++;
                    string FirstName = worksheet.Cells[i, j].Value.ToString(); j++;
                    string LastName = worksheet.Cells[i, j].Value.ToString(); j++;
                    string Email = worksheet.Cells[i, j].Value.ToString(); j++;
                    bool gender = (bool)worksheet.Cells[i, j].Value; j++;

                    string DobString = worksheet.Cells[i, j].Value.ToString(); j++;
                    DateOnly Dob = DateOnly.Parse(DobString);

                    string Address = worksheet.Cells[i, j].Value.ToString(); j++;
                    string PhoneNumber = worksheet.Cells[i, j].Value.ToString(); j++;
                    int JobPositionId = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    int DepartmentId = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    string Photo = worksheet.Cells[i, j].Value.ToString(); j++;

                    DateOnly StartDate = DateOnly.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    var EndDateString = worksheet.Cells[i, j].Value; j++;
                    DateOnly? EndDate = null;
                    if (EndDateString.ToString().Length > 0)
                    {
                        EndDate = DateOnly.Parse(EndDateString + "");
                    }

                    int TotalLeaveDays = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    int AvailableLeaveDays = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    int StatusId = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    int RoleId = int.Parse(worksheet.Cells[i, j].Value.ToString()); j++;
                    bool IsActive = (bool)worksheet.Cells[i, j].Value; j++;


                    Employee employee = new Employee();
                    employee.EmployeeId = EmployeeId;
                    employee.UserName = UserName;
                    employee.FirstName = FirstName;
                    employee.LastName = LastName;
                    employee.Email = Email;
                    employee.Gender = gender;
                    employee.Dob = Dob;
                    employee.Address = Address;
                    employee.PhoneNumber = PhoneNumber;
                    employee.JobPositionId = JobPositionId;
                    employee.DepartmentId = DepartmentId;
                    employee.Photo = Photo;
                    employee.StartDate = StartDate;
                    employee.EndDate = EndDate;
                    employee.TotalLeaveDays = TotalLeaveDays;
                    employee.AvailableLeaveDays = AvailableLeaveDays;
                    employee.StatusId = StatusId;
                    employee.RoleId = RoleId;
                    employee.IsActive = IsActive;

                    Employee employee1 = employeeServices.getEmployeeByUserName(UserName);
                    employee.Password = employee1.Password;


                    cList.Add(employee);
                }
                catch
                {
                    MessageBox.Show("Can not import this file", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                

                }

                //Nếu customer tồn tại => update, nếu ko => Add
                foreach (Employee employee1 in cList)
                {
                    if (employeeServices.getEmployees().Any(e => e.EmployeeId == employee1.EmployeeId))
                    {
                        employeeServices.UpdateEmployee(employee1);
                    }
                    else
                    {
                        employeeServices.AddEmployee(employee1);
                    }
                }

                employeeDataGrid.ItemsSource = employeeServices.getEmployees();
                MessageBox.Show("Import successful", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        private void employeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayManagement leaveDayManagement = new LeaveDayManagement();
            leaveDayManagement.Show();
            this.Close();
        }
        private void btnSalary_Click(object sender, RoutedEventArgs e)
        {
            SalaryManagement salaryManagement = new SalaryManagement();
            salaryManagement.Show();
            //this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmployeeAttendanceManagement employeeAttendanceManagement = new EmployeeAttendanceManagement();
            employeeAttendanceManagement.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DepartmentManagement department = new DepartmentManagement();
            department.Show();
            this.Close();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            ActivityHistoryWindow activityHistoryWindow = new ActivityHistoryWindow();  
            activityHistoryWindow.Show();
            this.Close();
        }
    }
}

 



