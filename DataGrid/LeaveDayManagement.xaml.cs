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
    /// Interaction logic for LeaveDayManagement.xaml
    /// </summary>
    public partial class LeaveDayManagement : Window
    {
        EmployeeServices employeeServices = new EmployeeServices();
        LeaveDayService leaveDayService = new LeaveDayService();
        ActivityHistoryService activityHistoryService = new ActivityHistoryService();
        public LeaveDayManagement()
        {
            InitializeComponent();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnLeaveDayList1_Click(object sender, RoutedEventArgs e)
        {
             LeaveDayListDataGrid.Visibility = Visibility.Visible;
            LeaveDayRequestDataGrid.Visibility = Visibility.Collapsed;
            txtTitle.Text = "Leave Day List";
            btnAccept.Visibility = Visibility.Collapsed;
            btnReject.Visibility = Visibility.Collapsed;
        }

        private void btnLeaveDayRequest_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayListDataGrid.Visibility = Visibility.Collapsed;
            LeaveDayRequestDataGrid.Visibility = Visibility.Visible;
            txtTitle.Text = "Leave Day Request";
            btnAccept.Visibility = Visibility.Visible;
            btnReject.Visibility = Visibility.Visible;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LeaveDayListDataGrid_Loaded(sender, e);
            LeaveDayRequestDataGrid_Loaded(sender, e);
        }

        private void LeaveDayListDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var leaveDayList = employeeServices.GetAllEmployeeLeaveDay();
            LeaveDayListDataGrid.ItemsSource = leaveDayList;
        }

        private void LeaveDayRequestDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var leaveDayRequest = leaveDayService.GetAllLeaveDay();
            LeaveDayRequestDataGrid.ItemsSource = leaveDayRequest;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnLeaveDay_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeaveRequest leaveRequest = LeaveDayRequestDataGrid.SelectedItem as LeaveRequest;
                if(leaveRequest==null) throw new Exception("Please select leave request");

                if (leaveRequest.RequestStatusId != 1)
                {
                    throw new Exception("This request is processed");
                }
                else
                {
                    //leaveRequest.RequestStatusId = 2;
                    leaveDayService.Update(leaveRequest,2);
                    MessageBox.Show("Accepted");
                    Employee? employee = employeeServices.GetEmployeeById(leaveRequest.EmployeeId);
                    DateTime startDate = DateTime.Parse(leaveRequest.StartDate.ToString());
                    DateTime endDate = DateTime.Parse(leaveRequest.EndDate.ToString());
                    int totalLeaveDays = (int)(endDate - startDate).TotalDays;
                    employee.AvailableLeaveDays -= totalLeaveDays;
                    employeeServices.UpdateEmployee(employee);
                    ActivityHistory activityHistory = new ActivityHistory();
                    Employee loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
                    activityHistory.EmployeeId = loginEmployee.EmployeeId;
                    activityHistory.Action = "Processed";
                    activityHistory.Target = "Leave Request";
                    activityHistory.Date = DateOnly.FromDateTime(DateTime.Now);
                    activityHistory.Time = TimeOnly.FromDateTime(DateTime.Now);
                    activityHistoryService.AddActivityHistory(activityHistory);
                    LeaveDayRequestDataGrid_Loaded(sender, e);
                    LeaveDayListDataGrid_Loaded(sender, e);
                }
               
                

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }

        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeaveRequest leaveRequest = LeaveDayRequestDataGrid.SelectedItem as LeaveRequest;
                if (leaveRequest == null) throw new Exception("Please select leave request");

                if (leaveRequest.RequestStatusId != 1)
                {
                    throw new Exception("This request is processed");
                }
                else
                {
                    //leaveRequest.RequestStatusId = 2;
                    leaveDayService.Update(leaveRequest, 3);
                    MessageBox.Show("Rejected");
                    ActivityHistory activityHistory = new ActivityHistory();
                    Employee loginEmployee = Application.Current.Properties["loginEmployee"] as Employee;
                    activityHistory.EmployeeId = loginEmployee.EmployeeId;
                    activityHistory.Action = "Processed";
                    activityHistory.Target = "Leave Request";
                    activityHistory.Date = DateOnly.FromDateTime(DateTime.Now);
                    activityHistory.Time = TimeOnly.FromDateTime(DateTime.Now);
                    activityHistoryService.AddActivityHistory(activityHistory);
                    LeaveDayRequestDataGrid_Loaded(sender, e);
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
