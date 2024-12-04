using Repositories.Models;
using Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataGrid
{
    public partial class SalaryManagement : Window
    {
        public Employee SelectedEmployee { get; set; } = null;
        private readonly SalaryService _salaryService;

        public SalaryManagement()
        {
            InitializeComponent();
            _salaryService = new SalaryService(new Prn212Context());

            LoadMonthsAndYears();
            LoadAllEmployeeSalaries();
        }

        private void LoadMonthsAndYears()
        {
            cbMonth.ItemsSource = Enumerable.Range(1, 12).ToList();
            cbMonth.SelectedIndex = DateTime.Now.Month - 1;

            int currentYear = DateTime.Now.Year;
            cbYear.ItemsSource = Enumerable.Range(currentYear - 10, 11).ToList();
            cbYear.SelectedItem = currentYear;
        }

        private void LoadAllEmployeeSalaries()
        {
            int? selectedMonth = cbMonth.SelectedItem as int?;
            int? selectedYear = cbYear.SelectedItem as int?;

            var salaries = _salaryService.GetAllEmployeeSalaries();

            if (selectedMonth.HasValue && selectedYear.HasValue)
            {
                salaries = salaries.Where(s => s.PaymentDate.HasValue &&
                                               s.PaymentDate.Value.Month == selectedMonth.Value &&
                                               s.PaymentDate.Value.Year == selectedYear.Value).ToList();
            }

            SalaryDataGrid.ItemsSource = SelectedEmployee != null
                ? salaries.Where(s => s.EmployeeId == SelectedEmployee.EmployeeId).ToList()
                : salaries.ToList();
        }

        private void LoadAllEmployeeSalaries2()
        {
            int? selectedMonth = cbMonth.SelectedItem as int?;
            int? selectedYear = cbYear.SelectedItem as int?;

            var salaries = _salaryService.GetAllEmployeeSalaries();

            if (selectedMonth.HasValue && selectedYear.HasValue)
            {
                salaries = salaries.Where(s => s.PaymentDate.HasValue &&
                                               s.PaymentDate.Value.Month == selectedMonth.Value &&
                                               s.PaymentDate.Value.Year == selectedYear.Value).ToList();
            }

            SalaryDataGrid.ItemsSource = salaries.ToList();
        }
        private void SalaryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
            {
                SelectedEmployee = selectedSalary.Employee;

                txtTotalIncome.Text = $"Total Income: {selectedSalary.TotalIncome}";

                LoadAllEmployeeSalaries();
            }
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllEmployeeSalaries2();
        }
        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayManagement leaveDayManagement = new LeaveDayManagement();
            leaveDayManagement.Show();
            this.Close();
        }
        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAllEmployeeSalaries();
        }

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAllEmployeeSalaries();
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
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}