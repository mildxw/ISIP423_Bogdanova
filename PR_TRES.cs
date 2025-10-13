using System;
using System.Text;

namespace TextAnalyzerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Анализатор текста ===");

            Console.WriteLine("\nВведите текст (минимум 100 символов):");
            string text = Console.ReadLine();

            if (text.Length < 100)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: текст слишком короткий!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Текст принят");
        }
    }
}