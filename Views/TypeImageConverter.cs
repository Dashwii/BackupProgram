using BackupProgram.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BackupProgram.Views
{
    public class TypeImageConverter : IValueConverter
    {
        public static TypeImageConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path;
            switch (value)
            {
                case TargetType.GOOGLE:
                    path = "images/drive.png";
                    break;
                case TargetType.DROPBOX:
                    path = "images/dropbox.png";
                    break;
                case TargetType.MEGA:
                    path = "images/mega.png";
                    break;
                default:
                    path = "images/folder.png";
                    break;
            }
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
