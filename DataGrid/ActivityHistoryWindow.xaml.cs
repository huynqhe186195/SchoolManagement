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
    public partial class ActivityHistoryWindow : Window
    {
        public Employee selected_employee { get; set; } = null;
        public ActivityHistoryWindow()
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
        ActivityHistoryServices activityHistoryServices = new ActivityHistoryServices();    

        public void LoadAllHistory()
        {
            //employeeDataGrid.Items.Clear();
            historyDataGrid.ItemsSource = activityHistoryServices.getAllHistory();
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
            LoadAllHistory();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtCountActivites.Text = activityHistoryServices.getAllHistory().Count+" Activities";
            LoadData();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = DateTime.Parse(dpDate.Text);
            DateOnly date = DateOnly.FromDateTime(dt);
            
            historyDataGrid.ItemsSource = activityHistoryServices.getAllHistory().Where(a => a.Date==date);
            txtCountActivites.Text = activityHistoryServices.getAllHistory().Where(a => a.Date == date).Count()+ " Activities";
            
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            txtCountActivites.Text = activityHistoryServices.getAllHistory().Count + " Activities";
            LoadData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(); 
            main.Show();
            this.Close();
        }
    }
}

 



