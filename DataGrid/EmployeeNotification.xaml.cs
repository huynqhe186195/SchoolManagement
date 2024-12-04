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
    /// Interaction logic for EmployeeNotification.xaml
    /// </summary>
    public partial class EmployeeNotification : Window
    {
        NotificationService notificationService = new NotificationService();

        public EmployeeNotification()
        {
            InitializeComponent();
        }

        private void NotificationDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;


            var notification = notificationService.GetNotificationForEmployee(loginEmployee.DepartmentId);

            NotificationDataGrid.ItemsSource = notification;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        

        private void btnNotificationClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEmployeeWindow_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeWindow = new EmployeeWindow();
            employeeWindow.Show();
            this.Close();
        }
    }
}
