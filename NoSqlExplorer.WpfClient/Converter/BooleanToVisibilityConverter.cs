using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NoSqlExplorer.WpfClient.Converter
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var flag = false;
      if (value is bool)
      {
        flag = (bool)value;
      }
      if (parameter != null)
      {
        if (bool.Parse((string)parameter))
        {
          flag = !flag;
        }
      }
      return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var back = ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
      if (parameter != null)
      {
        if ((bool)parameter)
        {
          back = !back;
        }
      }
      return back;
    }
  }
}
