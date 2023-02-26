using MVC.ViewModels;

namespace MVC.Services;

public static class MockProductService
{
    public static async Task<IEnumerable<Product>> GetProducts()
    {
        await Task.Delay(1);
        var products = new List<Product>()
        {
           new Product()
           {
               Id = 1,
               Name = "Fender American Professional II Jazz Bass",
               Description = "Classic low-top sneakers in white leather with signature swoosh logo.",
               Price = 89.99m,
               Amount = 10,
               PictureUrl = "https://static.nike.com/a/images/t_PDP_1280_v1/f_auto,q_auto:eco/18f94c30-87d2-4027-b0d1-12ef75f9fa5a/air-force-1-07-shoe-ZfwvBz.jpg",
               Type = "Sneakers",
               Brand = "Nike"
           },
           new Product()
           {
               Id = 2,
               Name = "Apple iPad Air",
               Description = "10.9-inch Retina display, A14 Bionic chip, Touch ID.",
               Price = 599.99m,
               Amount = 7,
               PictureUrl = "https://store.storeimages.cdn-apple.com/4668/as-images.apple.com/is/ipad-air-select-202009?wid=2000&hei=2000&fmt=jpeg&qlt=95&.v=1599249317000",
               Type = "Tablet",
               Brand = "Apple"
           },
           new Product()
           {
               Id = 3,
               Name = "Samsung 55-Inch Class QLED Q70A Series",
               Description = "4K Ultra HD Smart TV with Quantum HDR, Alexa Built-In.",
               Price = 1199.99m,
               Amount = 3,
               PictureUrl = "https://store.storeimages.cdn-apple.com/4668/as-images.apple.com/is/ipad-air-select-202009?wid=2000&hei=2000&fmt=jpeg&qlt=95&.v=1599249317000",
               Type = "TV",
               Brand = "Samsung"
           }
        };

        return products;
    }
}
