using Razbor_DE.AppData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Razbor_DE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataOutPage.xaml
    /// </summary>
    public partial class DataOutPage : Page
    {
        private Frame _mainFrame;
        public ObservableCollection<Materials> materials;
        public DataOutPage(Frame frame)
        {
            InitializeComponent();
            materials = new ObservableCollection<Materials>();
            listRecipes.ItemsSource = materials;
            _mainFrame = frame;
            LoadView();
        }

        public void LoadView()
        {
            materials.Clear();
            bool exists = AppData.AppConnect.modelDB.Materials.Any();

            if (exists)
            {
                foreach (var mat in AppData.AppConnect.modelDB.Materials)
                {
                    materials.Add(mat);
                }
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listRecipes_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
