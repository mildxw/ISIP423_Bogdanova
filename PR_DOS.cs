using StoreInventory;
using System;

namespace StoreInventory
{
    
    public enum Category
    {
        Food,
        Electronics,
        Clothes
    }

   
    public class Product
    {
        private static int _nextCode = 1000; 
        public int Code { get; private set; }     
        public string Name { get; private set; }  
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public Category Category { get; private set; }

        
        public bool InStock => Quantity > 0;

        
        public Product(string name, decimal price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым.");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть больше нуля.");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");

            Code = _nextCode++;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        
        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price} руб., Кол-во: {Quantity}, Категория: {Category}, В наличии: {(InStock ? "Да" : "Нет")}";
        }
    }
}

public class Store
{
    private List<Product> _products;

    public Store()
    {
        _products = new List<Product>
            {
                new Product("Хлеб", 50m, 20, Category.Food),
                new Product("Молоко", 70m, 15, Category.Food),
                new Product("Футболка", 1200m, 10, Category.Clothes),
                new Product("Телефон", 30000m, 5, Category.Electronics),
                new Product("Наушники", 2500m, 8, Category.Electronics)
            };
    }

    public void ShowAllProducts()
    {
        Console.WriteLine("\nСписок всех товаров:\n");
        foreach (var product in _products)
            Console.WriteLine(product);
    }

    public Product FindByCode(int code)
    {
        return _products.FirstOrDefault(p => p.Code == code);
    }

    public bool IsEmpty() => _products.Count == 0;
}


internal class PR_DOS
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Store store = new Store();

        store.ShowAllProducts();

        Console.WriteLine("\nВведите код товара для поиска:");
        if (int.TryParse(Console.ReadLine(), out int code))
        {
            var product = store.FindByCode(code);
            if (product != null)
                Console.WriteLine("\nНайден товар:\n" + product);
            else
                Console.WriteLine("Товар с таким кодом не найден.");
        }
        else
        {
            Console.WriteLine("Ошибка: введено некорректное значение.");
        }
    }
}
