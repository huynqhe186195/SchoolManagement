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
    /// Interaction logic for AddDepartmentWindow.xaml
    /// </summary>
    public partial class AddDepartmentWindow : Window
    {
        private DepartmentServices _departmentServices = new DepartmentServices();

        public AddDepartmentWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Department Name" || textBox.Text == "Department Address"))
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Name == "txtDepartmentName" ? "Department Name" : "Department Address";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDepartmentName.Text) && txtDepartmentName.Text != "Department Name" &&
                !string.IsNullOrWhiteSpace(txtDepartmentAddress.Text) && txtDepartmentAddress.Text != "Department Address")
            {
                var newDepartment = new Department
                {
                    DepartmentName = txtDepartmentName.Text,
                    DepartmentAddress = txtDepartmentAddress.Text,
                    IsActive = chkIsActive.IsChecked == true ? true : false
                };
                _departmentServices.AddDepartment(newDepartment);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid department name and address.");
            }
        }
    }
}
