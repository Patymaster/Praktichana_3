using System;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            // Шлях до файлів JSON
            string directoryPath = @"C:\MyDirectory\JSONFiles\";

            // Параметри фільтра
            double minPrice = 10.0;
            double maxPrice = 50.0;
            int minQuantity = 5;
            int maxQuantity = 20;

            // Визначення делегата фільтру
            Predicate<Product> filter = p =>
                p.Price >= minPrice && p.Price <= maxPrice &&
                p.Quantity >= minQuantity && p.Quantity <= maxQuantity;

            // Визначення делегата виводу
            Action<Product> display = p =>
                Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Price: {p.Price}, Quantity: {p.Quantity}");

            // Читання файлів JSON та фільтрація
            for (int i = 1; i <= 10; i++)
            {
                string filePath = directoryPath + i + ".json";
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    Product[] products = JsonConvert.DeserializeObject<Product[]>(json);
                    foreach (Product product in products)
                    {
                        if (filter(product))
                        {
                            display(product);
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}