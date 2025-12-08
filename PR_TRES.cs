/*
using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalyzerApp
{
    class TextStatistics
    {
        public string Text;
        public int WordCount;
        public int SentenceCount;
        public int VowelCount;
        public int ConsonantCount;
        public string ShortestWord;
        public string LongestWord;
        public Dictionary<char, int> Frequency = new Dictionary<char, int>();
    }

    internal class Program
    {
        static List<TextStatistics> history = new List<TextStatistics>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Анализатор текста ===");

            while (true)
            {
                Console.WriteLine("\n1 - Ввести новый текст");
                Console.WriteLine("2 - Показать статистику прошлых текстов");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0") break;
                else if (choice == "1")
                {
                    Console.WriteLine("\nВведите текст (минимум 100 символов):");
                    string text = Console.ReadLine();

                    if (text.Length < 100)
                    {
                        Console.WriteLine("Ошибка: текст слишком короткий!");
                        continue;
                    }

                    TextStatistics stats = AnalyzeText(text);
                    history.Add(stats);
                    PrintStats(stats);
                }
                else if (choice == "2")
                {
                    if (history.Count == 0)
                        Console.WriteLine("Пока нет статистики.");
                    else
                    {
                        for (int i = 0; i < history.Count; i++)
                        {
                            Console.WriteLine($"\nТекст №{i + 1}:");
                            PrintStats(history[i]);
                        }
                    }
                }
            }
        }

        static TextStatistics AnalyzeText(string text)
        {
            TextStatistics s = new TextStatistics();
            s.Text = text;

            string[] separators = { " ", "\t", "\n", ",", ".", "!", "?", ";", ":", "(", ")", "\"", "'" };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            s.WordCount = words.Length;

            s.ShortestWord = words[0];
            s.LongestWord = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < s.ShortestWord.Length)
                    s.ShortestWord = words[i];
                if (words[i].Length > s.LongestWord.Length)
                    s.LongestWord = words[i];
            }

            // Подсчёт предложений
            foreach (char c in text)
                if (c == '.' || c == '!' || c == '?')
                    s.SentenceCount++;

            // Подсчёт гласных и согласных
            string vowels = "аеёиоуыэюяaeiou";
            string consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (vowels.IndexOf(c) >= 0)
                        s.VowelCount++;
                    else if (consonants.IndexOf(c) >= 0)
                        s.ConsonantCount++;

                    // частота букв
                    if (s.Frequency.ContainsKey(c))
                        s.Frequency[c]++;
                    else
                        s.Frequency[c] = 1;
                }
            }

            return s;
        }

        static void PrintStats(TextStatistics s)
        {
            Console.WriteLine($"Слов: {s.WordCount}");
            Console.WriteLine($"Предложений: {s.SentenceCount}");
            Console.WriteLine($"Самое короткое слово: {s.ShortestWord}");
            Console.WriteLine($"Самое длинное слово: {s.LongestWord}");
            Console.WriteLine($"Гласных: {s.VowelCount}, Согласных: {s.ConsonantCount}");
            Console.WriteLine("Частота букв:");
            foreach (var kvp in s.Frequency)
                Console.WriteLine($"  {kvp.Key} : {kvp.Value}");
        }
    }
}
*\