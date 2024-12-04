using Repositories.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for NotifactionList.xaml
    /// </summary>
    public partial class NotifactionList : Window
    {
        NotificationService notificationService = new NotificationService();
        public NotifactionList()
        {
            InitializeComponent();
        }

        public void NotificationDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

           var notifications = notificationService.GetAll();
            foreach (var notification in notifications)
            {
                if(notification.DepartmentId == null)
                {
                      notification.DepartmentId = 0;
                     
                }
            }


            NotificationDataGrid.ItemsSource = notifications;
        }

        private void NotificationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var department = value as Department; // Giả sử `Department` là kiểu dữ liệu của thuộc tính này
            if (department == null || department.DepartmentId == 0)
                return "All";

            return department?.DepartmentName ?? "All";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNotification_Click(object sender, RoutedEventArgs e)
        {
            AddNotification addNotification = new AddNotification();
            addNotification.Show();
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnNotificationRefresh_Click(object sender, RoutedEventArgs e)
        {
            NotificationDataGrid_Loaded(sender, e);
        }

        private void btnDeleteNotification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Notification? notification = NotificationDataGrid.SelectedItem as Notification;
                notificationService.DeleteNotification(notification.NotificationId);
                MessageBox.Show("Delete successfully");
                NotificationDataGrid_Loaded(sender, e);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayManagement leaveDayManagement = new LeaveDayManagement();
            leaveDayManagement.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            NotificationDataGrid_Loaded(sender, e);
        }
    }
}
