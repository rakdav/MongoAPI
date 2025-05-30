using Microsoft.EntityFrameworkCore;

namespace Lab3.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(DataContext context)
        {
            context.Database.Migrate();
            if (context.Products.Count() == 0&&
                context.Suppliers.Count()==0&&
                context.Categories.Count()==0)
            {
                Supplier s1 = new Supplier { Name = "Дудка компани", City = "Дудка сити" };
                Supplier s2 = new Supplier { Name = "Идриутка", City = "Дудка вилладж" };
                Supplier s3 = new Supplier { Name = "Хомячков корпорейтед", City = "Дудка бомж" };

                Category c1 = new Category { Name = "Хлебо-уточная" };
                Category c2 = new Category { Name = "Молочная" };
                Category c3 = new Category { Name = "Канцелярия" };

                context.Products.AddRange(
                    new Product 
                    { 
                        Name="Утка",
                        Price=10,
                        Category=c1,
                        Supplier=s1
                    },
                    new Product
                    {
                        Name = "Молоко",
                        Price = 45,
                        Category = c2,
                        Supplier = s3
                    },
                    new Product
                    {
                        Name = "Хомячок",
                        Price = 100,
                        Category = c3,
                        Supplier = s2
                    },
                    new Product
                    {
                        Name = "Сорока обыкновенная",
                        Price = 0,
                        Category = c2,
                        Supplier = s2
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
