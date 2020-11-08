using DataAcceessLibrary.Data;
using DataAcceessLibrary.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_SQLite_2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {

        private long _orderId;
        private IEnumerable<Order> orders { get; set; }

        public OrderPage()
        {
            this.InitializeComponent();
            LoadOrdersAsync().GetAwaiter();//populera
            LoadStatusAsync().GetAwaiter();
        }


        #region Order
        private async Task LoadOrdersAsync()//kan ladda all orders när köra
        {
            orders = await SQLiteContext.GetOrders(); //hämta alla
            LoadActiveOrders();
            LoadClosedOrders();

        }
        private async Task OrderUpdateAsync()
        {
            await SQLiteContext.UpdateOrderAsync(new Order());
        }
        private void LoadActiveOrders()
        {
            lvActiveOrders.ItemsSource = orders
                .Where(i => i.Status != "closed")
                .OrderByDescending(i => i.Created)
                .Take(SettingsContext.GetMaxItemsCount())
                .ToList();
        }
        private void LoadClosedOrders()//dela ut orders 
        {
            lvCompletedOrders.ItemsSource = orders.Where(i => i.Status == "closed").ToList();
        }
        #endregion


       
       

        private async void btnCreateOrder_Click(object sender, RoutedEventArgs e) // ???? har problem i den del fel ---Object reference not set to an instance of an object.'

        {
            _orderId = await SQLiteContext.CreateOrderAsync(new Order
            {
                CustomerId =  33,//await SQLiteContext.GetCustomerIdByLastName(cmbCustomers.SelectedItem.ToString()),
                ProductId = 45,
                Quantity = 2,
                Description = "Detta är order",
                Status = "new"
            });
            _orderId = await SQLiteContext.CreateOrderAsync(new Order
            {
                CustomerId = 234,//await SQLiteContext.GetCustomerIdByLastName(cmbCustomers.SelectedItem.ToString()),
                ProductId = 654,
                Quantity = 2,
                Description = "Detta är order",
                Status = "new"
            });
            await LoadOrdersAsync();//uppdatera list sortera ut
        }
        private async void btnAllOrders_Click(object sender, RoutedEventArgs e)
        {
            await LoadOrdersAsync();
        }


      



        #region Status
        private async Task LoadStatusAsync()
        {
            cmbStatus.ItemsSource = await SettingsContext.GetStatus();
            await OrderUpdateAsync();
            await LoadOrdersAsync();//uppdatera list sortera ut
        }

        #endregion


        //private void LoadActiveOrders()//dela ut orders 
        //{//listview
        // // lvActiveOrders.ItemSourse = await DataAcceessLibrary.Data.SQLiteContext.CreateOrderAsync();//DataContext
        //    lvActiveOrders.ItemsSource = orders.Where(i => i.Status != "closed").ToList();
        //}
    }
}
