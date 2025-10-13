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

            AnalyzeText(text);
        }
        static void AnalyzeText(string text)
        {
            // Подсчёт слов
            string[]  separators = {" ", "\t", "\n", ",", ".", "!", "?", ";", ":", "(", ")", "\"", "'"}
            string[]  words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            int wordCount  = words.Length;

            // Поиск самого длиноого/самого короткого слова
            string shortest = words[0];
            string longest = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < shortest.Length)
                    shortest = words[i];
                if (words[i].Length > longest.Length)
                    longest = words[i];
            }

            Console.WriteLine($"Самое короткое слово: {shortest}");
            Console.WriteLine($"Самое длинное слово: {longest}");

            // Подсчёт предложений
            int sentenceCount = 0;
            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                sentenceCount++;
            }

            // Подсчёт гласных/согласных
            string vowels = "аеёиоуыэюяaeiou";
            string consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";
            int vowelCount = 0, consonantCount = 0;

            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (vowels.IndexOf(c) >= 0)
                        vowelCount++;
                    else if (consonants.IndexOf(c) >= 0)
                        consonantCount++;
                }
            }

            Console.WriteLine($"Количество слов: {wordCount}");
            Console.WriteLine($"Количество предложений: {sentenceCount}");
            Console.WriteLine($"Гласных: {vowelCount}, Согласных: {consonantCount}");
        }
    }
}