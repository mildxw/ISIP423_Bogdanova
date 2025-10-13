/*using System;
using System.Collections.Generic;
using System.Linq;

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

        public void ChangeQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");
            Quantity = newQuantity;
        }

        public override string ToString()
        {
            return $"{Code,-5} | {Name,-20} | {Price,8} руб. | {Quantity,5} | {Category,-12} | {(InStock ? "Да" : "Нет"),-3}";
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nКод   | Название             | Цена      | Кол-во | Категория     | В наличии");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.ResetColor();
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
                    WriteError("Ошибка: неверный формат цены.");
                    return;
                }

                Console.Write("Введите количество товара: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    WriteError("Ошибка: неверный формат количества.");
                    return;
                }

                Console.WriteLine("Выберите категорию: 0 - Food, 1 - Electronics, 2 - Clothes");
                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 0 || categoryIndex > 2)
                {
                    WriteError("Ошибка: неверная категория.");
                    return;
                }

                var category = (Category)categoryIndex;
                var product = new Product(name, price, quantity, category);
                _products.Add(product);

                WriteSuccess($"Товар \"{name}\" добавлен успешно!");
            }
            catch (Exception ex)
            {
                WriteError($"Ошибка при добавлении товара: {ex.Message}");
            }
        }

        public void RemoveProduct()
        {
            Console.Write("Введите код товара для удаления: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int code))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверный формат кода. Введите число.");
                Console.ResetColor();
                return;
            }

            var product = FindByCode(code);
            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Товар с таким кодом не найден.");
                Console.ResetColor();
                return;
            }

            _products.Remove(product);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Товар \"{product.Name}\" удалён успешно.");
            Console.ResetColor();
        }


        public void OrderProduct()
        {
            Console.Write("Введите код товара для поставки: ");
            if (int.TryParse(Console.ReadLine(), out int code))
            {
                var product = FindByCode(code);
                if (product != null)
                {
                    Console.Write("Введите количество для поставки: ");
                    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                    {
                        product.ChangeQuantity(product.Quantity + amount);
                        WriteSuccess($"Товар \"{product.Name}\" пополнен на {amount} шт. Новый остаток: {product.Quantity}");
                    }
                    else
                    {
                        WriteError("Количество должно быть положительным числом.");
                    }
                }
                else
                {
                    WriteError("Товар с таким кодом не найден.");
                }
            }
            else
            {
                WriteError("Ошибка: неверный формат кода.");
            }
        }

        public void SellProduct()
        {
            Console.Write("Введите код товара для продажи: ");
            if (int.TryParse(Console.ReadLine(), out int code))
            {
                var product = FindByCode(code);
                if (product != null)
                {
                    Console.Write("Введите количество для продажи: ");
                    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                    {
                        if (product.Quantity >= amount)
                        {
                            product.ChangeQuantity(product.Quantity - amount);
                            WriteSuccess($"Продано {amount} шт. товара \"{product.Name}\". Остаток: {product.Quantity}");
                        }
                        else
                        {
                            WriteError($"Недостаточно товара на складе. В наличии: {product.Quantity}");
                        }
                    }
                    else
                    {
                        WriteError("Количество должно быть положительным числом.");
                    }
                }
                else
                {
                    WriteError("Товар с таким кодом не найден.");
                }
            }
            else
            {
                WriteError("Ошибка: неверный формат кода.");
            }
        }

        private void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    internal class PR_DOS
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Store store = new Store();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== Добро пожаловать в систему учёта товаров магазина! ===");
            Console.ResetColor();

            while (true)
            {
                Console.WriteLine("\n=== МЕНЮ ===");
                Console.WriteLine("1 - Добавить товар");
                Console.WriteLine("2 - Удалить товар");
                Console.WriteLine("3 - Найти товар по названию");
                Console.WriteLine("4 - Найти товар по категории");
                Console.WriteLine("5 - Показать все товары");
                Console.WriteLine("6 - Заказать поставку товара");
                Console.WriteLine("7 - Продать товар");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                Console.WriteLine("\n---------------------------------------------------");

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
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\nНайденные товары:");
                            Console.WriteLine("Код   | Название             | Цена      | Кол-во | Категория     | В наличии");
                            Console.WriteLine("----------------------------------------------------------------------");
                            foreach (var p in foundByName)
                                Console.WriteLine(p);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ничего не найдено.");
                            Console.ResetColor();
                        }
                        break;
                    case "4":
                        Console.WriteLine("Выберите категорию: 0 - Food, 1 - Electronics, 2 - Clothes");
                        if (int.TryParse(Console.ReadLine(), out int catIndex) && catIndex >= 0 && catIndex <= 2)
                        {
                            var category = (Category)catIndex;
                            var foundByCategory = store.FindByCategory(category);
                            if (foundByCategory.Any())
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("\nТовары выбранной категории:");
                                Console.WriteLine("Код   | Название             | Цена      | Кол-во | Категория     | В наличии");
                                Console.WriteLine("----------------------------------------------------------------------");
                                foreach (var p in foundByCategory)
                                    Console.WriteLine(p);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Товары этой категории отсутствуют.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка: неверная категория.");
                            Console.ResetColor();
                        }
                        break;
                    case "5":
                        store.ShowAllProducts();
                        break;
                    case "6":
                        store.OrderProduct();
                        break;
                    case "7":
                        store.SellProduct();
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                Console.ReadKey();
            }
        }
    }
}
*/