using System;
using System.Globalization;

namespace AudioVideoTech
{
    /// Базовый класс иерархии — аудио- или видеоустройство.
    /// Хранит общие характеристики: производитель, модель, цена.

    public class AudioVideoDevice
    {
        // Границы и значения по умолчанию — именованные константы вместо «магических чисел»
        public const decimal MinPrice = 0m;
        public const decimal MaxPrice = 10_000_000m;

        public const string DefaultBrand = "Неизвестный производитель";
        public const string DefaultModel = "Без модели";

        // Закрытые поля, доступ к которым идёт только через свойства
        private string _brand = DefaultBrand;
        private string _model = DefaultModel;
        private decimal _price;

        public AudioVideoDevice()
        {
        }

        public AudioVideoDevice(string brand, string model, decimal price)
        {
            // Присваиваем через свойства, чтобы сработали проверки в set
            Brand = brand;
            Model = model;
            Price = price;
        }

        public string Brand
        {
            get => _brand;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Производитель не может быть пустым.", nameof(Brand));
                }

                _brand = value.Trim();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Модель не может быть пустой.", nameof(Model));
                }

                _model = value.Trim();
            }
        }

        /// Цена в рублях. Тип decimal — точный, без ошибок округления, как у double.
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < MinPrice || value > MaxPrice)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(Price), value, $"Цена должна быть в диапазоне [{MinPrice}; {MaxPrice}] руб.");
                }

                _price = value;
            }
        }

        /// Название типа устройства. Виртуальное свойство —
        /// каждый производный класс возвращает своё название.

        public virtual string DeviceType => "Аудио/видео устройство";
        /// ВИРТУАЛЬНЫЙ МЕТОД. Возвращает характеристики, специфичные для типа устройства.
        /// У базового устройства своих характеристик нет, поэтому строка пустая.
        /// Производные классы переопределяют метод (override) и возвращают свои данные.
        public virtual string GetSpecifications()
        {
            return string.Empty;
        }

        public override string ToString()
        {
            string text = string.Format(
                CultureInfo.CurrentCulture,
                "{0}: {1} {2}, цена {3:N2} руб.",
                DeviceType, Brand, Model, Price);

            string specifications = GetSpecifications();

            return specifications.Length == 0 ? text : text + ", " + specifications;
        }
    }
}
