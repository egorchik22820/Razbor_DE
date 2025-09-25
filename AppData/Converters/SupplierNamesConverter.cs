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
                                                      .Where(name => !string.IsNullOrEmpty(name));

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
