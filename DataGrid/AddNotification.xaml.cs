using Microsoft.Identity.Client.NativeInterop;
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
    /// Interaction logic for AddNotification.xaml
    /// </summary>
    public partial class AddNotification : Window
    {
        DepartmentServices departmentService = new DepartmentServices();
        NotificationService notificationService = new NotificationService();
        ActivityHistoryService activityHistoryService = new ActivityHistoryService();
        public AddNotification()
        {
            InitializeComponent();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private void txtNotificationContent_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cboDepartment_Loaded(object sender, RoutedEventArgs e)
        {
           
            var listdepartment = departmentService.GetDepartments();
          

            cboDepartment.ItemsSource = listdepartment;
            cboDepartment.DisplayMemberPath = "DepartmentName";
            cboDepartment.SelectedValuePath = "DepartmentId";
            cboDepartment.SelectedIndex = 0;


        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Notification notification = new Notification();
                notification.Content = txtNotificationContent.Text;

                if (cboDepartment.SelectedValue == null)
                {
                    throw new Exception("Please select department");
                }
             

                    if (cboDepartment.SelectedValue.ToString() != "0")
                    {
                        notification.DepartmentId = int.Parse(cboDepartment.SelectedValue.ToString());
                    }
                    Employee? loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
                    //=  Employee loginEmployee = (Employee)session.setAtribute("loginEmployee");

                    notification.CreateBy = loginEmployee.EmployeeId;
                    notification.SendAt = DateOnly.FromDateTime(DateTime.Now);
                    notification.Time = TimeOnly.FromDateTime(DateTime.Now);

                    notificationService.AddNotification(notification);

                    ActivityHistory activityHistory = new ActivityHistory();

                    activityHistory.EmployeeId = loginEmployee.EmployeeId;
                    activityHistory.Action = "Send notification";
                    activityHistory.Target = departmentService.GetDepartmentById(notification.DepartmentId).DepartmentName;
                    activityHistory.Date = DateOnly.FromDateTime(DateTime.Now);
                    activityHistory.Time = TimeOnly.FromDateTime(DateTime.Now);
                    activityHistoryService.AddActivityHistory(activityHistory);



                    MessageBox.Show("Send notification successfully");
                }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
