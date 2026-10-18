using System;
using System.Globalization;

namespace AudioVideoTech
{
    /// Производный класс «Телевизор»: добавляет диагональ экрана и Smart TV.
    public class Television : AudioVideoDevice
    {
        public const int MinScreenInch = 10;
        public const int MaxScreenInch = 120;
        public const int DefaultScreenInch = 32;

        public const double InchToCm = 2.54;

        private int _screenInch = DefaultScreenInch;

        public Television()
        {
        }

        // : base(...) — сначала вызывается конструктор базового класса
        public Television(string brand, string model, decimal price, int screenInch, bool isSmartTv)
            : base(brand, model, price)
        {
            ScreenInch = screenInch;
            IsSmartTv = isSmartTv;
        }

        public int ScreenInch
        {
            get => _screenInch;
            set
            {
                if (value < MinScreenInch || value > MaxScreenInch)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(ScreenInch), value,
                        $"Диагональ должна быть в диапазоне [{MinScreenInch}; {MaxScreenInch}] дюймов.");
                }

                _screenInch = value;
            }
        }

        // Автосвойство: проверка не нужна, компилятор сам создаёт скрытое поле
        public bool IsSmartTv { get; set; }

        // Вычисляемое свойство только для чтения: значение не хранится, а считается
        public double ScreenCm => Math.Round(ScreenInch * InchToCm, 1);

        public override string DeviceType => "Телевизор";

        ///Переопределение виртуального метода: характеристики телевизора.
        public override string GetSpecifications()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "диагональ {0} дюйм. ({1:N1} см), Smart TV: {2}",
                ScreenInch, ScreenCm, IsSmartTv ? "да" : "нет");
        }
    }
}
