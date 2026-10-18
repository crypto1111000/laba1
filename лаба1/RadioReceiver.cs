using System;
using System.Globalization;

namespace AudioVideoTech
{
    ///Производный класс «Радиоприёмник»: добавляет диапазон частот и Bluetooth.
    public class RadioReceiver : AudioVideoDevice
    {
        public const double MinFrequencyMHz = 0.1;
        public const double MaxFrequencyMHz = 3000.0;

        // Стандартный FM-диапазон
        public const double DefaultFrequencyMinMHz = 87.5;
        public const double DefaultFrequencyMaxMHz = 108.0;

        private double _frequencyMinMHz = DefaultFrequencyMinMHz;
        private double _frequencyMaxMHz = DefaultFrequencyMaxMHz;

        public RadioReceiver()
        {
        }

        public RadioReceiver(string brand, string model, decimal price,
                             double frequencyMinMHz, double frequencyMaxMHz, bool hasBluetooth)
            : base(brand, model, price)
        {
            SetFrequencyRange(frequencyMinMHz, frequencyMaxMHz);
            HasBluetooth = hasBluetooth;
        }

        public double FrequencyMinMHz
        {
            get => _frequencyMinMHz;
            set
            {
                ValidateFrequency(value, nameof(FrequencyMinMHz));
                ValidateRange(value, _frequencyMaxMHz);
                _frequencyMinMHz = value;
            }
        }

        public double FrequencyMaxMHz
        {
            get => _frequencyMaxMHz;
            set
            {
                ValidateFrequency(value, nameof(FrequencyMaxMHz));
                ValidateRange(_frequencyMinMHz, value);
                _frequencyMaxMHz = value;
            }
        }

        public bool HasBluetooth { get; set; }

        public double BandwidthMHz => Math.Round(FrequencyMaxMHz - FrequencyMinMHz, 2);

        public override string DeviceType => "Радиоприёмник";


        /// Установка обеих границ сразу. Через свойства по одной это не всегда возможно:
        /// например, при диапазоне 87,5–108 нельзя первым задать min = 150,
        /// потому что он окажется больше текущего max.
        public void SetFrequencyRange(double minMHz, double maxMHz)
        {
            ValidateFrequency(minMHz, nameof(minMHz));
            ValidateFrequency(maxMHz, nameof(maxMHz));
            ValidateRange(minMHz, maxMHz);

            _frequencyMinMHz = minMHz;
            _frequencyMaxMHz = maxMHz;
        }

        /// Переопределение виртуального метода: характеристики радиоприёмника.
        public override string GetSpecifications()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "диапазон {0:N1}-{1:N1} МГц (ширина {2:N1} МГц), Bluetooth: {3}",
                FrequencyMinMHz, FrequencyMaxMHz, BandwidthMHz, HasBluetooth ? "есть" : "нет");
        }

        // Частота вне физически допустимых границ
        private static void ValidateFrequency(double value, string paramName)
        {
            if (double.IsNaN(value) || value < MinFrequencyMHz || value > MaxFrequencyMHz)
            {
                throw new ArgumentOutOfRangeException(
                    paramName, value,
                    $"Частота должна быть в диапазоне [{MinFrequencyMHz}; {MaxFrequencyMHz}] МГц.");
            }
        }

        // Нижняя граница больше верхней — одно и то же исключение во всех местах
        private static void ValidateRange(double minMHz, double maxMHz)
        {
            if (minMHz > maxMHz)
            {
                throw new ArgumentException("Нижняя граница диапазона не может превышать верхнюю.");
            }
        }
    }
}
