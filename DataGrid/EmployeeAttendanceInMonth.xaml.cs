using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
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
    /// Interaction logic for EmployeeAttendanceInMonth.xaml
    /// </summary>
    public partial class EmployeeAttendanceInMonth : Window
    {
        AttendanceServices attendanceServices = new AttendanceServices();

        public EmployeeAttendanceInMonth()
        {
            InitializeComponent();
        }

        private void AttendanceDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            AttendanceSummary? selected_item = Application.Current.Properties["selected_employee"] as AttendanceSummary;

            // Check if selected_item is not null
            AttendanceDataGrid.ItemsSource = attendanceServices.GetAttendanceInMonth(selected_item.EmployeeId, selected_item.Month,selected_item.Year);


        }
        
    }
}
