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
    public sealed partial class ProductPage : Page
    {

        private long _productId;        
        private IEnumerable<Product> products { get; set; }
        public ProductPage()
        {
            this.InitializeComponent();
            LoadProductsAsync().GetAwaiter ();
            LoadStatusAsync().GetAwaiter();
        }
            


        #region Button Service
        private async void btnCreateProduct_Click(object sender, RoutedEventArgs e)
        {
            _productId = await SQLiteContext.CreateProductAsync(new Product { Name = "Iphone-"+  Guid.NewGuid().ToString(), Description = "Iphone (89guuy)", Price=4800, Status="closed" });
            _productId = await SQLiteContext.CreateProductAsync(new Product { Name = "Tavla-" + Guid.NewGuid().ToString(), Description = "", Price = 290, Status = "new" });
          
            await LoadProductsAsync();
        }

        private async void btnAllProduct_Click(object sender, RoutedEventArgs e)
        {
            await LoadProductsAsync();
        }
        #endregion
        #region Product
        private async Task LoadProductsAsync()//kan ladda all när köra
        {
            products = await SQLiteContext.GetProducts(); //hämta alla          
        }
        private async Task LoadAllProductsAsync()// ladda i LISTview
        {
            lvProducts.ItemsSource = await SQLiteContext.GetProducts();

        }
        #endregion

        //private async Task ProductUpdateAsync()
        //{
        //    await SQLiteContext.UpdateProductAsync(new Product());
        //}


        #region Status
        private async Task LoadStatusAsync()
        {
            cmbStatus.ItemsSource = await SettingsContext.GetStatus();
            //await ProductUpdateAsync();
            await LoadProductsAsync();//uppdatera list sortera ut
        }

        #endregion
    }
}

