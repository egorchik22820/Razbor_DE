using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Razbor_DE.AppData.Converters
{
    public class SupplierNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<MaterialSuppliers> materialSuppliers)
            {
                var supplierNames = materialSuppliers.Select(ms => ms.Suppliers?.Name)
                                                      .Where(name => !string.IsNullOrEmpty(name)).ToList();

                //if (supplierNames.Count > 2)
                //{
                //    List<string> strings = new List<string>();
                //    for (int i = 0; i < 4; i++)
                //    {
                //        strings[i] = supplierNames[i];
                //    }
                //    return string.Join(", ", strings) + " ...";
                //}
                //else
                //    return string.Join(", ", supplierNames);
                return string.Join(", ", supplierNames);

            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
