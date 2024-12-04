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
    /// Interaction logic for EmployeeSalary.xaml
    /// </summary>
    public partial class EmployeeSalary : Window
    {
        public Employee SelectedEmployee { get; set; } = null;
        private readonly SalaryService _salaryService;

        public EmployeeSalary()
        {
            InitializeComponent();
            _salaryService = new SalaryService(new Prn212Context());
            LoadEmployeeSalaries();
        }

        private void LoadEmployeeSalaries()
        {
            SelectedEmployee = Application.Current.Properties["saemployee"] as Employee;
            SalaryDataGrid.ItemsSource = SelectedEmployee != null
                ? _salaryService.GetEmployeeSalaries().Where(s => s.EmployeeId == SelectedEmployee.EmployeeId).ToList()
                : _salaryService.GetEmployeeSalaries().ToList();
        }
        private void SalaryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
            {
                SelectedEmployee = selectedSalary.Employee;
                LoadEmployeeSalaries();
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployeeSalaries();
        }
        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeWindow = new EmployeeWindow();   
            employeeWindow.Show();
            this.Close();
        }
    }
}
