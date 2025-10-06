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

    public Product FindByCode(int code) => _products.FirstOrDefault(p => p.Code == code);

    public List<Product> FindByName(string name) =>
        _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

    public List<Product> FindByCategory(Category category) =>
        _products.Where(p => p.Category == category).ToList();

   
    public void AddProduct()
    {
        try
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите цену товара: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Ошибка: неверный формат цены.");
                return;
            }

            Console.Write("Введите количество товара: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Ошибка: неверный формат количества.");
                return;
            }

            Console.WriteLine("Выберите категорию: 0 - Food, 1 - Electronics, 2 - Clothes");
            if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 0 || categoryIndex > 2)
            {
                Console.WriteLine("Ошибка: неверная категория.");
                return;
            }

            var category = (Category)categoryIndex;
            var product = new Product(name, price, quantity, category);
            _products.Add(product);

            Console.WriteLine($"\n✅ Товар \"{name}\" добавлен успешно!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
        }
    }

    public void RemoveProduct()
    {
        Console.Write("Введите код товара для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int code))
        {
            var product = FindByCode(code);
            if (product != null)
            {
                _products.Remove(product);
                Console.WriteLine($"✅ Товар \"{product.Name}\" удалён.");
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный формат кода.");
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Store store = new Store();

        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1 - Добавить товар");
            Console.WriteLine("2 - Удалить товар");
            Console.WriteLine("3 - Найти товар по названию");
            Console.WriteLine("4 - Найти товар по категории");
            Console.WriteLine("5 - Показать все товары");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    store.AddProduct();
                    break;
                case "2":
                    store.RemoveProduct();
                    break;
                case "3":
                    Console.Write("Введите название: ");
                    var name = Console.ReadLine();
                    var foundByName = store.FindByName(name);
                    if (foundByName.Any())
                        foundByName.ForEach(p => Console.WriteLine(p));
                    else
                        Console.WriteLine("Ничего не найдено.");
                    break;
                case "4":
                    Console.WriteLine("Выберите категорию: 0 - Food, 1 - Electronics, 2 - Clothes");
                    if (int.TryParse(Console.ReadLine(), out int catIndex) && catIndex >= 0 && catIndex <= 2)
                    {
                        var category = (Category)catIndex;
                        var foundByCategory = store.FindByCategory(category);
                        if (foundByCategory.Any())
                            foundByCategory.ForEach(p => Console.WriteLine(p));
                        else
                            Console.WriteLine("Товары этой категории отсутствуют.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверная категория.");
                    }
                    break;
                case "5":
                    store.ShowAllProducts();
                    break;
                case "0":
                    Console.WriteLine("Выход из программы...");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                    break;
            }
        }
    }
}

