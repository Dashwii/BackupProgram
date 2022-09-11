using BackupProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BackupProgram
{
    [ValueConversion(typeof(string), typeof(bool))]
    internal class HeaderToImageConverter
    {
        public static HeaderToImageConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/{value}"));
        }
    }
}
