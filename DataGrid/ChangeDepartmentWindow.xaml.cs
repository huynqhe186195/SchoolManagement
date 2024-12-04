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
    /// Interaction logic for ChangeDepartmentWindow.xaml
    /// </summary>
    public partial class ChangeDepartmentWindow : Window
    {
        private Employee _employee;
        private DepartmentServices _departmentServices = new DepartmentServices();
        public EmployeeServices employeeServices = new EmployeeServices();

        public ChangeDepartmentWindow()
        {
            InitializeComponent();
            
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            departmentComboBox.ItemsSource = _departmentServices.GetDepartments();
            departmentComboBox.DisplayMemberPath = "DepartmentName";
            departmentComboBox.SelectedValuePath = "DepartmentId";
        }

        private void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentComboBox.SelectedValue is int newDepartmentId)
            {
                List<Employee> selectedEmployees = (List<Employee>)Application.Current.Properties["selectedEmployees"];
                foreach (Employee employee in selectedEmployees)
                {
                    employee.DepartmentId = newDepartmentId;
                    employeeServices.UpdateEmployee(employee);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a department.");
            }
        }
    }
}
