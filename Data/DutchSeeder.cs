using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<UserStorage> _userManager;

        public DutchSeeder(DutchContext ctx,
          IWebHostEnvironment hosting,
          UserManager<UserStorage> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            UserStorage user = await _userManager.FindByEmailAsync("shawn@dutchtreat.com");

            if (user == null)
            {
                user = new UserStorage()
                {
                    FirstName = "Hayden",
                    LastName = "Matz",
                    Email = "haydenmatz97@gmail.com",
                    UserName = "haydenmatz97@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "TeStP@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in Seeder");
                }
            }

            if (!_ctx.Products.Any())
            {
                // Need to create the Sample Data
                var file = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(file);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
          {
            new OrderItem()
            {
              Product = products.First(),
              Quantity = 5,
              UnitPrice = products.First().Price
            }
          };
                }

                _ctx.SaveChanges();
            }
        }
    }
}
