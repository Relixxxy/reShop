using Catalog.Data;
using Catalog.Data.Entities;

namespace Catalog.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Products.Any())
        {
            await context.Products.AddRangeAsync(GetPreconfiguredProducts());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<ProductEntity> GetPreconfiguredProducts()
    {
        return new List<ProductEntity>()
        {
            new ProductEntity()
            {
                Name = "Taylor 214ce",
                Description = "The Taylor 214ce is a Grand Auditorium-size acoustic-electric guitar that is great for both strumming and fingerpicking. It features a solid Sitka spruce top, layered rosewood back and sides, and an ES2 pickup system.",
                Price = 999.99m,
                AvailableStock = 10,
                PictureFileName = "1.jpg",
                Type = "Acoustic",
                Brand = "Taylor"
            },
            new ProductEntity()
            {
                Name = "Gibson Les Paul Standard '50s",
                Description = "The Gibson Les Paul Standard '50s is a classic electric guitar that features a mahogany body with a maple top, a rounded '50s-style mahogany neck with a rosewood fingerboard, and Burstbucker pickups.",
                Price = 2399.00m,
                AvailableStock = 10,
                PictureFileName = "2.jpg",
                Type = "Electric",
                Brand = "Gibson"
            },
            new ProductEntity()
            {
                Name = "Fender American Professional II Jazz Bass",
                Description = "The Fender American Professional II Jazz Bass is a versatile and great-sounding bass guitar that is perfect for any style of music. It features a slim C-shaped neck, V-Mod II Jazz Bass pickups, and a HiMass Vintage bridge.",
                Price = 1799.99m,
                AvailableStock = 10,
                PictureFileName = "3.jpg",
                Type = "Bass",
                Brand = "Fender"
            },
            new ProductEntity()
            {
                Name = "Epiphone Sheraton II Pro",
                Description = "The Epiphone Sheraton II Pro is a semi-hollow electric guitar that is based on the classic Gibson ES-335. It features a laminated maple body, a 5-piece maple/walnut neck, and ProBucker pickups.",
                Price = 699.00m,
                AvailableStock = 10,
                PictureFileName = "4.jpg",
                Type = "Electric",
                Brand = "Epiphone"
            },
            new ProductEntity()
            {
                Name = "Ibanez SR500E",
                Description = "The Ibanez SR500E is a high-performance bass guitar that is great for both live and studio work. It features a lightweight ash body, a 5-piece jatoba/walnut neck, and Bartolini pickups.",
                Price = 799.99m,
                AvailableStock = 10,
                PictureFileName = "5.jpg",
                Type = "Bass",
                Brand = "Ibanez"
            },
            new ProductEntity()
            {
                Name = "Martin D-28",
                Description = "The Martin D-28 is a classic dreadnought-size acoustic guitar that is beloved by many players. It features a solid Sitka spruce top, solid East Indian rosewood back and sides, and a modified low oval neck shape.",
                Price = 2999.00m,
                AvailableStock = 10,
                PictureFileName = "6.jpg",
                Type = "Acoustic",
                Brand = "Martin"
            },
            new ProductEntity()
            {
                Name = "Gibson Les Paul Classic 2019",
                Description = "The Gibson Les Paul Classic 2019 offers traditional Les Paul style with a modern edge. The guitar features a solid mahogany body with a maple top, a slim-tapered mahogany neck, and a bound rosewood fingerboard. The guitar also has a pair of humbucking pickups and a Tune-O-Matic bridge with stopbar tailpiece.",
                Price = 2499.00m,
                AvailableStock = 10,
                PictureFileName = "7.jpg",
                Type = "Electric",
                Brand = "Gibson"
            },
            new ProductEntity()
            {
                Name = "Fender American Professional Telecaster",
                Description = "The Fender American Professional Telecaster is a modern take on a classic guitar. The guitar features a solid alder body, a deep C-shaped maple neck, and a 22-fret maple fingerboard. The guitar also has a pair of single-coil pickups and a six-saddle string-through-body bridge.",
                Price = 1499.99m,
                AvailableStock = 10,
                PictureFileName = "8.jpg",
                Type = "Electric",
                Brand = "Fender"
            },
            new ProductEntity()
            {
                Name = "Ibanez SR500E Bass Guitar",
                Description = "The Ibanez SR500E Bass Guitar offers premium features at an affordable price. The guitar features a solid mahogany body, a five-piece jatoba and walnut neck, and a rosewood fingerboard. The guitar also has a pair of Bartolini MK-1 pickups and a custom 3-band EQ.",
                Price = 799.99m,
                AvailableStock = 10,
                PictureFileName = "9.jpg",
                Type = "Bass",
                Brand = "Ibanez"
            },
            new ProductEntity()
            {
                Name = "Gretsch G6120T Players Edition Nashville",
                Description = "The Gretsch G6120T Players Edition Nashville is a classic guitar with modern upgrades. The guitar features a laminated maple body, a maple neck, and an ebony fingerboard. The guitar also has a pair of High Sensitive Filter'Tron pickups and a Bigsby B6GP vibrato tailpiece.",
                Price = 2799.99m,
                AvailableStock = 10,
                PictureFileName = "10.jpg",
                Type = "Electric",
                Brand = "Gretsch"
            },
            new ProductEntity()
            {
                Name = "Taylor 214ce DLX Grand Auditorium",
                Description = "The Taylor 214ce DLX Grand Auditorium is a versatile acoustic guitar that's great for a variety of playing styles. The guitar features a solid Sitka spruce top, layered rosewood back and sides, and a mahogany neck. The guitar also has a Venetian cutaway, Taylor's Expression System 2 pickup, and a deluxe hardshell case.",
                Price = 1099.00m,
                AvailableStock = 10,
                PictureFileName = "11.jpg",
                Type = "Acoustic",
                Brand = "Taylor"
            },
            new ProductEntity()
            {
                Name = "Fender CD-60 Dreadnought Acoustic Guitar",
                Description = "The Fender CD-60 Dreadnought Acoustic Guitar is an excellent entry-level acoustic guitar. With a dreadnought-style body, this guitar is designed to provide a big, full sound. It features a laminated spruce top and laminated mahogany back and sides, which produce a warm and balanced tone.",
                Price = 249.99m,
                AvailableStock = 10,
                PictureFileName = "12.jpg",
                Type = "Acoustic",
                Brand = "Fender"
            },
            new ProductEntity()
            {
                Name = "Gibson Les Paul Studio Electric Guitar",
                Description = "The Gibson Les Paul Studio Electric Guitar is a classic model that has been used by countless musicians over the years. It features a mahogany body and neck, a maple top, and a rosewood fingerboard. The two humbucking pickups provide a thick and powerful sound that is perfect for rock and blues.",
                Price = 1499.99m,
                AvailableStock = 10,
                PictureFileName = "13.jpg",
                Type = "Electric",
                Brand = "Gibson"
            },
            new ProductEntity()
            {
                Name = "Ibanez GSRM20 Mikro Bass Guitar",
                Description = "The Ibanez GSRM20 Mikro Bass Guitar is a great choice for beginners and young players. It features a small-scale neck and a compact body that make it easy to play. The mahogany body and maple neck provide a warm and punchy tone that is perfect for a variety of styles.",
                Price = 179.99m,
                AvailableStock = 10,
                PictureFileName = "14.jpg",
                Type = "Bass",
                Brand = "Ibanez"
            },
            new ProductEntity()
            {
                Name = "Taylor 114ce Acoustic-Electric Guitar",
                Description = "The Taylor 114ce Acoustic-Electric Guitar is a high-quality instrument that is designed to provide a rich and balanced sound. It features a solid sitka spruce top and layered walnut back and sides, which produce a warm and articulate tone. The built-in electronics make it easy to plug in and play.",
                Price = 899.99m,
                AvailableStock = 10,
                PictureFileName = "15.jpg",
                Type = "Acoustic",
                Brand = "Taylor"
            },
            new ProductEntity()
            {
                Name = "Fender Player Stratocaster Electric Guitar",
                Description = "The Fender Player Stratocaster Electric Guitar is a versatile instrument that is perfect for a wide range of styles. It features an alder body, a maple neck, and a pau ferro fingerboard. The three single-coil pickups provide a bright and clear sound that is ideal for classic rock and blues.",
                Price = 699.99m,
                AvailableStock = 10,
                PictureFileName = "16.jpg",
                Type = "Electric",
                Brand = "Fender"
            },
        };
    }
}