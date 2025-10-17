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
            if (string.IsNullOrEmpty(name) || _productService.GetAll().Exists(i => i.Name == name))
            {
                ErrorMessage = "Naam bestaat al voor ander product of naam veld is nog leeg";
            }
            else if (!Int32.TryParse(stock, out int stockValue))
            {
                ErrorMessage = "Voorraad geen geldig format, verwacht format is een heel getal";
            }
            else if (Int32.Parse(stock) < 0)
            {
                ErrorMessage = "Voorraad mag geen negatief getal zijn";
            }
            else if (!DateOnly.TryParse(date, out DateOnly dateValue))
            {
                ErrorMessage = "THT Datum geen geldig format, verwacht format is YYYY-MM-DD";
            }
            else if (DateOnly.Parse(date) < DateOnly.FromDateTime(DateTime.Now))
            {
                ErrorMessage = "THT Datum kan niet in het verleden liggen";
            }
            else if (!Decimal.TryParse(price, out Decimal priceValue))
            {
                ErrorMessage = "Prijs geen geldig format, verwacht format is euro format";
            }
            else
            {
                try
                {
                    Product newProduct = new Product(_productService.GetAll().Count + 1, name, stockValue, dateValue, priceValue);

                    _productService.Add(newProduct);

                    await Shell.Current.GoToAsync(nameof(ProductView));
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Toevoegen mislukt: {ex.Message}";
                }
            }
        }
    }
}
