using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioVideoTech
{
    /// Тестирующая программа: коллекция ссылок базового типа на объекты разных классов
    /// и демонстрация полиморфизма.
    public static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Тип элементов — базовый класс, а хранятся в ней объекты всех трёх классов
            List<AudioVideoDevice> devices = CreateDemoDevices();

            ShowDemo(devices);
            RunMenu(devices);

            Console.WriteLine("Работа завершена.");
        }

        private static List<AudioVideoDevice> CreateDemoDevices()
        {
            return new List<AudioVideoDevice>
            {
                new Television("Samsung", "UE55AU7100", 49990m, 55, true),
                new Television("LG", "32LQ570B", 17500m, 32, false),
                new RadioReceiver("Sony", "ICF-P36", 3200m, 87.5, 108.0, false),
                new RadioReceiver("Philips", "TAR3205", 4500m, 87.5, 108.0, true),
                new AudioVideoDevice("Generic", "AVD-100", 9999m)
            };
        }

        private static void ShowDemo(List<AudioVideoDevice> devices)
        {
            Console.WriteLine("=== Исходные объекты (полиморфный вызов ToString) ===");
            PrintDevices(devices);

            Console.WriteLine();
            Console.WriteLine("=== Изменение характеристик через свойства ===");

            // Общее свойство доступно через ссылку базового типа
            devices[0].Price = 45000m;

            // Свойства производного класса — только после проверки и приведения типа
            if (devices[1] is Television lgTv)
            {
                lgTv.ScreenInch = 43;
                lgTv.IsSmartTv = true;
            }

            if (devices[2] is RadioReceiver sonyRadio)
            {
                sonyRadio.HasBluetooth = true;
                sonyRadio.SetFrequencyRange(64.0, 108.0);
            }

            PrintDevices(devices);

            Console.WriteLine();
            Console.WriteLine("=== Вызов виртуального метода через ссылку базового типа ===");
            foreach (AudioVideoDevice device in devices)
            {
                string specifications = device.GetSpecifications();
                Console.WriteLine($"{device.GetType().Name,-16} -> " +
                                  (specifications.Length == 0 ? "(нет своих характеристик)" : specifications));
            }

            Console.WriteLine();
            Console.WriteLine("=== Статистика по коллекции ===");
            Console.WriteLine($"Всего устройств: {devices.Count}");
            Console.WriteLine($"Телевизоров: {devices.OfType<Television>().Count()}");
            Console.WriteLine($"Радиоприёмников: {devices.OfType<RadioReceiver>().Count()}");
            Console.WriteLine($"Суммарная стоимость: {devices.Sum(d => d.Price):N2} руб.");
        }

        private static void RunMenu(List<AudioVideoDevice> devices)
        {
            int action;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Действие:");
                Console.WriteLine("  1 — добавить устройство");
                Console.WriteLine("  2 — вывести список");
                Console.WriteLine("  3 — удалить устройство по номеру");
                Console.WriteLine("  0 — завершить");

                action = ConsoleInput.ReadInt("Ваш выбор: ", 0, 3);

                switch (action)
                {
                    case 1:
                        AddDevice(devices);
                        break;
                    case 2:
                        PrintDevices(devices);
                        break;
                    case 3:
                        RemoveDevice(devices);
                        break;
                }
            }
            while (action != 0);
        }

        private static void AddDevice(List<AudioVideoDevice> devices)
        {
            Console.WriteLine("Тип объекта:");
            Console.WriteLine("  1 — AudioVideoDevice (базовый)");
            Console.WriteLine("  2 — Television (телевизор)");
            Console.WriteLine("  3 — RadioReceiver (радиоприёмник)");

            int type = ConsoleInput.ReadInt("Ваш выбор: ", 1, 3);

            string brand = ConsoleInput.ReadNonEmptyString("Производитель: ");
            string model = ConsoleInput.ReadNonEmptyString("Модель: ");
            decimal price = ConsoleInput.ReadDecimal(
                "Цена (руб.): ", AudioVideoDevice.MinPrice, AudioVideoDevice.MaxPrice);

            AudioVideoDevice device;

            switch (type)
            {
                case 2:
                    int screenInch = ConsoleInput.ReadInt(
                        "Диагональ (дюймы): ", Television.MinScreenInch, Television.MaxScreenInch);
                    bool isSmartTv = ConsoleInput.ReadBool("Smart TV");
                    device = new Television(brand, model, price, screenInch, isSmartTv);
                    break;

                case 3:
                    double freqMin = ConsoleInput.ReadDouble(
                        "Мин. частота (МГц): ", RadioReceiver.MinFrequencyMHz, RadioReceiver.MaxFrequencyMHz);
                    // Нижняя граница для max — введённый min, поэтому min > max ввести нельзя
                    double freqMax = ConsoleInput.ReadDouble(
                        "Макс. частота (МГц): ", freqMin, RadioReceiver.MaxFrequencyMHz);
                    bool hasBluetooth = ConsoleInput.ReadBool("Bluetooth");
                    device = new RadioReceiver(brand, model, price, freqMin, freqMax, hasBluetooth);
                    break;

                default:
                    device = new AudioVideoDevice(brand, model, price);
                    break;
            }

            devices.Add(device);
            Console.WriteLine($"Добавлено: {device}");
        }

        private static void RemoveDevice(List<AudioVideoDevice> devices)
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("Список пуст, удалять нечего.");
                return;
            }

            PrintDevices(devices);
            int number = ConsoleInput.ReadInt("Номер удаляемого устройства: ", 1, devices.Count);

            Console.WriteLine($"Удалено: {devices[number - 1]}");
            devices.RemoveAt(number - 1);
        }

        // Для каждого элемента вызывается ToString(), а внутри него — GetSpecifications()
        // того класса, к которому объект относится на самом деле
        private static void PrintDevices(List<AudioVideoDevice> devices)
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            Console.WriteLine($"--- Список устройств ({devices.Count}) ---");

            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {devices[i]}");
            }
        }
    }
}
