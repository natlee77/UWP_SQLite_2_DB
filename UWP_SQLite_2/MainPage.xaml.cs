using DataAcceessLibrary.Data;
using DataAcceessLibrary.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_SQLite_2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private long _customerId;
        
        private IEnumerable<Order > orders { get; set; }
        private long CustomerId { get; set; }



        public MainPage()
        {
            this.InitializeComponent();

            LoadOrdersAsync().GetAwaiter();//populera
           
        }



        private async Task LoadOrdersAsync()//kan ladda all orders när köra
        {
            orders = await SQLiteContext.GetOrders();
             LoadActiveOrders();     //tabort await
             LoadCompletedOrders();
        }

        private void  LoadActiveOrders()//dela ut orders 
        {//listview
         // lvActiveOrders.ItemSourse = await DataAcceessLibrary.Data.SQLiteContext.CreateOrderAsync();//DataContext
            lvActiveOrders.ItemsSource = orders.Where(i=> i.Status != "completed").ToList();
        }  //async Task ändra till void
        private void  LoadCompletedOrders()//dela ut orders 
        {//listview
         // lvActiveOrders.ItemSourse = await DataAcceessLibrary.Data.SQLiteContext.CreateOrderAsync();//DataContext
            lvCompletedOrders.ItemsSource = orders.Where(i => i.Status == "completed").ToList();
        }


        #region Button Service

        private async void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {//om det finns kund hämta - om finns inte skapa kund

            //try
            //{
            //     await SQLiteContext.GetCustomers();
            //}
            //var customer = new Customer("Nataliya");
            CustomerId = await SQLiteContext.CreateCustomerAsync(new Customer {FirstName="Nataliya", LastName = "Lisjö" + Guid.NewGuid().ToString() });
           
          //++ CustomerId- for tillbacka Id  for att använda det

        }

        private async void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            await SQLiteContext.CreateOrderAsync( new Order
            {
                CustomerId = _customerId,
                Description = "Detta är order"
            });

            await LoadOrdersAsync();//uppdatera list sortera ut



        }

        #endregion 
        //private async Task LoadCustomersAsync()
        //{
        //    cmbCustomers.ItemsSource = await SQLiteContext.GetCustomerLastNames();
        //}       
        //private async Task LoadStatusAsync()
        //{
        //    cmbStatus.ItemsSource = await SettingsContext.GetStatus();
        //}
        //private void LoadActiveIssues()
        //{
        //    lvActiveOrders.ItemsSource = orders
        //        .Where(i => i.Status != "closed")
        //        .OrderByDescending(i => i.Created)
        //        .Take(SettingsContext.GetMaxItemsCount())
        //        .ToList();
        //}







        //private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        //{
        //    await SQLiteContext.CreateOrderAsync(
        //        new Order
        //        {
        //            Title = "CAS-" + Guid.NewGuid().ToString(),
        //            Description = "Detta är ett ärende",
        //            CustomerId = await SQLiteContext.GetCustomerIdByLastName(cmbCustomers.SelectedItem.ToString())
        //        }
        //    );

        //    await GetOrdersAsync();
        //}



    }
}
