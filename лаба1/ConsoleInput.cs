using System;
using System.Globalization;

namespace AudioVideoTech
{

    /// Безопасный ввод с консоли: запрос повторяется, пока не введено корректное значение.

    public static class ConsoleInput
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                string line = ReadLine(prompt);

                if (!string.IsNullOrWhiteSpace(line))
                {
                    return line.Trim();
                }

                Console.WriteLine("Значение не может быть пустым, повторите ввод.");
            }
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                string line = ReadLine(prompt);

                if (int.TryParse(line, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Введите целое число в диапазоне [{min}; {max}].");
            }
        }

        public static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                // Принимаем и запятую, и точку как разделитель дробной части
                string line = ReadLine(prompt).Replace(',', '.');

                if (double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                    && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Введите число в диапазоне [{min}; {max}].");
            }
        }

        public static decimal ReadDecimal(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                string line = ReadLine(prompt).Replace(',', '.');

                if (decimal.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value)
                    && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Введите число в диапазоне [{min}; {max}].");
            }
        }

        public static bool ReadBool(string prompt)
        {
            while (true)
            {
                string line = ReadLine($"{prompt} (1 — да / 0 — нет): ").Trim();

                if (line == "1")
                {
                    return true;
                }

                if (line == "0")
                {
                    return false;
                }

                Console.WriteLine("Введите 1 (да) или 0 (нет).");
            }
        }

        // ReadLine() возвращает null, если поток ввода закрыт (Ctrl+Z).
        // Без этой проверки программа ушла бы в бесконечный цикл.
        private static string ReadLine(string prompt)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();

            if (line == null)
            {
                Console.WriteLine();
                Console.WriteLine("Ввод прерван, работа завершена.");
                Environment.Exit(0);
            }

            return line;
        }
    }
}
