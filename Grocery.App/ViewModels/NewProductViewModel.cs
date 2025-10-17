using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{

    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string stock;

        [ObservableProperty]
        private string date;

        [ObservableProperty]
        private string price;

        [ObservableProperty]
        private string errorMessage;

        public NewProductViewModel(IProductService productService)
        {
            _productService = productService;
        }

        [RelayCommand]
        public async Task AddNewProduct()
        {
            if (!string.IsNullOrEmpty(name) || !_productService.GetAll().Exists(i => i.Name == name))
            {
                errorMessage = "Naam bestaat al voor ander product of naam veld is nog leeg";
            }

            if (!Int32.TryParse(stock, out int stockValue))
            {
                errorMessage = "Voorraad geen geldig format, verwacht format is een heel getal";
            }

            if(Int32.Parse(stock) < 0)
            {
                errorMessage = "Voorraad mag geen negatief getal zijn";
            }

            if(!DateOnly.TryParse(date, out DateOnly dateValue))
            {
                errorMessage = "THT Datum geen geldig format, verwacht format is YYYY-MM-DD";
            }

            if(DateOnly.Parse(date) < DateOnly.FromDateTime(DateTime.Now))
            {
                errorMessage = "THT Datum kan niet in het verleden liggen";
            }

            if (!Decimal.TryParse(price, out Decimal priceValue))
            {
                errorMessage = "Prijs geen geldig format, verwacht format is euro format";
            }

            try
            {
                Product newProduct = new Product(_productService.GetAll().Count + 1, name, stockValue, dateValue, priceValue);

                _productService.Add(newProduct);

                await Shell.Current.GoToAsync(nameof(ProductView));
            } 
            catch(Exception ex)
            {
                errorMessage = $"Toevoegen mislukt: {ex.Message}";
            }
        }
    }
}
