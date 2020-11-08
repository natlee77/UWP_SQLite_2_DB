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
    public sealed partial class CustomerPage : Page
    {
        private long _customerId;
        private long CustomerId { get; set; }
        private IEnumerable<Customer> customers { get; set; }



        public CustomerPage()
        {
            this.InitializeComponent();


            LoadAllCustomersAsync().GetAwaiter();
            LoadCustomersNameAsync().GetAwaiter();
        }


        #region Costumer
        private async Task LoadCustomersNameAsync()// ladda kund in combobox  --- var fel här ++ LoadCustomersNameAsync().GetAwaiter(); till main page
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

            _customerId = await SQLiteContext.CreateCustomerAsync(new Customer { FirstName = "Nataliya", LastName = "Lisjö " + Guid.NewGuid().ToString(), Adress = "Gatan", City = "Degerfors", PostCode = 69335 }); // + Guid.NewGuid().ToString() 
            _customerId = await SQLiteContext.CreateCustomerAsync(new Customer { FirstName = "Wiljam", LastName = "Berns " + Guid.NewGuid().ToString(), Adress = "Street", City = "Los Angeles", PostCode = 00000 });
            //++ CustomerId- for tillbacka Id  for att använda det
            await LoadAllCustomersAsync();
        }

        private async void btnAllCustomers_Click(object sender, RoutedEventArgs e)
        {
            await LoadAllCustomersAsync();
        }
        #endregion

    }
}
