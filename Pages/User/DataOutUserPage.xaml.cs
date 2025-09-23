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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Razbor_DE.Pages.User
{
    /// <summary>
    /// Логика взаимодействия для DataOutUserPage.xaml
    /// </summary>
    public partial class DataOutUserPage : Page
    {
        private Frame _mainFrame;
        public ObservableCollection<Materials> materials;

        public DataOutUserPage(Frame frame)
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

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
