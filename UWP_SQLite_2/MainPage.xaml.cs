﻿using DataAcceessLibrary.Data;
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
        private long _orderId;
        private long CustomerId { get; set; }
        private IEnumerable<Order > orders { get; set; }
        private IEnumerable<Customer> customers { get; set; }
        

        public MainPage()
        {
            this.InitializeComponent();

            LoadOrdersAsync().GetAwaiter();//populera
            LoadAllCustomersAsync().GetAwaiter();
            LoadStatusAsync().GetAwaiter();

        }


        #region Order
        private async Task LoadOrdersAsync()//kan ladda all orders när köra
        {
            orders = await SQLiteContext.GetOrders(); //hämta alla
            LoadActiveOrders();                       
            LoadClosedOrders();

        }
        private async Task  OrderUpdateAsync()
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
        {//listview
            lvCompletedOrders.ItemsSource = orders.Where(i => i.Status == "closed").ToList();
        }
        #endregion


        #region Costumer
        private async Task LoadCustomersNameAsync()// ladda kund in combobox
        {
            cmbCustomers.ItemsSource = await SQLiteContext.GetCustomerLastNames();
        }
        private async Task LoadAllCustomersAsync()// ladda i LISTview
        {
            lvAllCustomer.ItemsSource = await SQLiteContext.GetCustomers();
        }
        #endregion


        #region Button Service
        private async void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {//om det finns kund hämta - om finns inte skapa kund
            //try
            //{
            //     await SQLiteContext.GetCustomers();
            //}
            
            _customerId = await SQLiteContext.CreateCustomerAsync(new Customer {FirstName="Nataliya", LastName = "Lisjö " + Guid.NewGuid().ToString(), Adress="Gatan" , City="Degerfors", PostCode=69335 }); // + Guid.NewGuid().ToString() 
            _customerId = await SQLiteContext.CreateCustomerAsync (new Customer {FirstName = "Wiljam", LastName = "Berns " + Guid.NewGuid().ToString(), Adress = "Street", City = "Los Angeles", PostCode = 00000 });
            //++ CustomerId- for tillbacka Id  for att använda det
            await LoadAllCustomersAsync();
        }
        private async void btnCreateOrder_Click(object sender, RoutedEventArgs e) // ???? har problem i den del fel ---Object reference not set to an instance of an object.'

        {
            _orderId = await SQLiteContext.CreateOrderAsync(new Order
           {CustomerId = await SQLiteContext.GetCustomerIdByLastName(cmbCustomers.SelectedItem.ToString()),   
               ProductId = 45,
               Quantity = 2,
               Description = "Detta är order",
               Status="new"
           });            
            _orderId = await SQLiteContext.CreateOrderAsync(new Order
            {
                CustomerId = await SQLiteContext.GetCustomerIdByLastName(cmbCustomers.SelectedItem.ToString()),
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
        private async void btnAllCustomers_Click(object sender, RoutedEventArgs e)
        {
           await LoadAllCustomersAsync();
        }
        #endregion


        #region Status
        private async Task LoadStatusAsync()
        {
            cmbStatus.ItemsSource = await SettingsContext.GetStatus();
            await OrderUpdateAsync();
            await LoadOrdersAsync();//uppdatera list sortera ut
        }

        #endregion
      
    }
}
//private void LoadActiveOrders()//dela ut orders 
//{//listview
// // lvActiveOrders.ItemSourse = await DataAcceessLibrary.Data.SQLiteContext.CreateOrderAsync();//DataContext
//    lvActiveOrders.ItemsSource = orders.Where(i => i.Status != "closed").ToList();
//}
