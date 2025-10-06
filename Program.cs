using System;
using System.Reflection.Metadata;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Введите кол-во операций(2 - 40)");
        int n;
        while (true)
        {
            string input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out n) && n >= 2 && n <= 40)
                break;
            Console.WriteLine("Некорректный ввод!(Х_Х) Введите число от 2 до 40!");
        }

        string[] names = new string[n];
        double[] amounts = new double[n];

        Console.WriteLine("Введите траты в формате: Название; Сумма");

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Операция {i + 1}:");
            string input = Console.ReadLine();
            string[] parts = input.Split(';');

            if (parts.Length != 2 || !double.TryParse(parts[1], out double sum))
            {
                Console.WriteLine("Некорректный ввод!!! (＃`Д´)");
                i--;
                continue;
            }

            names[i] = parts[0].Trim();
            amounts[i] = sum;
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            Console.WriteLine("Ваш выбор:");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВаши траты:");
                    for (int i = 0; i < n; i++)
                    {
                        Console.WriteLine($"{names[i]}: {amounts[i]} руб.");
                    }
                    break;

                case "2":
                    double total = 0;
                    double max = double.MinValue;
                    double min = double.MaxValue;

                    for (int i = 0; i < n; i++)
                    {
                        total += amounts[i];
                        if (amounts[i] > max) max = amounts[i];
                        if (amounts[i] < min) min = amounts[i];
                    }

                    Console.WriteLine($"\nСумма: {total} руб.");
                    Console.WriteLine($"Среднее: {total / n:F2} руб.");
                    Console.WriteLine($"Максимум: {max} руб.");
                    Console.WriteLine($"Минимум: {min} руб.");
                    break;

                case "3":
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = 0; j < n - 1; j++)
                        {
                            if (amounts[j] > amounts[j + 1])
                            {
                                double tempAmount = amounts[j];
                                amounts[j] = amounts[j + 1];
                                amounts[j + 1] = tempAmount;

                                string tempName = names[j];
                                names[j] = names[j + 1];
                                names[j + 1] = tempName;


                            }
                        }
                    }
                    Console.WriteLine("\nОтсортированные траты:");
                    for (int i = 0; i < n; i++)
                        Console.WriteLine($"{names[i]}: {amounts[i]} руб.");
                    break;

                case "4":
                    Console.WriteLine("\nВвыберите валюту для конвертации");
                    Console.WriteLine("1. Доллары (курс 95)");
                    Console.WriteLine("2. Евро (курс 100)");
                    Console.WriteLine("3. Ввести свой курс");
                    Console.Write("Ваш выбор: ");
                    string currencyChoice = Console.ReadLine();
                    double rate = 1;
                    string currency = "руб.";
                    switch (currencyChoice) {
                        case "1": rate = 95; currency = "USD"; break;
                        case "2": rate = 100; currency = "EUR"; break;
                        case "3":
                            Console.Write("Введите курс: ");
                            double.TryParse(Console.ReadLine(), out rate);
                            Console.Write("Введите обозначение валюты: ");
                            currency = Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Ошибка выбора! (-_-)");
                            continue;
                    }

                    for (int i = 0; i < n; i++)
                    {
                        Console.WriteLine($"{names[i]}: {amounts[i] / rate} {currency}");
                    }
                    break;
                case "5":
                    Console.Write("Введите название для поиска: ");
                    string search = Console.ReadLine().ToLower();
                    bool found = false;

                    for (int i = 0; i < n; i++)
                    {
                        if (names[i].ToLower().Contains(search))
                        {
                            Console.WriteLine($"{names[i]}: {amounts[i]} руб.");
                            found = true;
                        }
                    }

                    if (!found) Console.WriteLine("Ничего не найдено :(");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
    }
}