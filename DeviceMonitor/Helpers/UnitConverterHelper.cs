using System;
using System.Linq;

namespace DeviceMonitor.Helpers
{
    public enum Unit
    {
        Kilobyte,
        Megabyte,
        Gigabyte,
        Terabyte
    }
    public static class UnitConverterHelper
    {

        public static double ConvertToUnit(string source, Unit unit)
        {
            switch (unit)
            {
                case Unit.Kilobyte:
                    return ConvertToKilobyte(source);
                case Unit.Megabyte:
                    return ConvertToMegabyte(source);
                case Unit.Gigabyte:
                    return ConvertToGigabyte(source);
                case Unit.Terabyte:
                    return ConvertToTerabyte(source);
            }

            return 0;
        }
        private static double ConvertToKilobyte(string source)
        {
            var currentUnit = source.Last();
            var value = source.Remove(source.Length - 1, 1);

            switch (currentUnit)
            {
                case 'K':
                    return Convert.ToDouble(value);
                case 'M':
                    return Convert.ToDouble(value) * 1000;
                case 'G':
                    return Convert.ToDouble(value) * 1000000;
                case 'T':
                    return Convert.ToDouble(value) * 1000000000;

            }

            return 0;
        }
        private static double ConvertToMegabyte(string source)
        {
            var currentUnit = source.Last();
            var value = source.Remove(source.Length - 1, 1);

            switch (currentUnit)
            {
                case 'K':
                    return Convert.ToDouble(value) / 1000;
                case 'M':
                    return Convert.ToDouble(value);
                case 'G':
                    return Convert.ToDouble(value) * 1000;
                case 'T':
                    return Convert.ToDouble(value) * 1000000;

            }

            return 0;
        }
        private static double ConvertToGigabyte(string source)
        {
            var currentUnit = source.Last();
            var value = source.Remove(source.Length - 1, 1);
            
            switch (currentUnit)
            {
                case 'K':
                    return Convert.ToDouble(value) / 1000000;
                case 'M':
                    return Convert.ToDouble(value) / 1000;
                case 'G':
                    return Convert.ToDouble(value);
                case 'T':
                    return Convert.ToDouble(value) * 1000;
                
            }

            return 0;
        }
        private static double ConvertToTerabyte(string source)
        {
            var currentUnit = source.Last();
            var value = source.Remove(source.Length - 1, 1);

            switch (currentUnit)
            {
                case 'K':
                    return Convert.ToDouble(value) / 1000000000;
                case 'M':
                    return Convert.ToDouble(value) / 1000000;
                case 'G':
                    return Convert.ToDouble(value) / 1000;
                case 'T':
                    return Convert.ToDouble(value);

            }

            return 0;
        }
        public static DateTime ConvertTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
