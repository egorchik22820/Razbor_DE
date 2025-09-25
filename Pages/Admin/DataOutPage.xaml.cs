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

        private int currentPage = 1;
        private int pageSize = 3; // Количество элементов на странице
        private int totalPages;

        public ObservableCollection<int> PageNumbers {  get; set; } = new ObservableCollection<int>();

        public DataOutPage(Frame frame)
        {
            InitializeComponent();
            materials = new ObservableCollection<Materials>();
            listRecipes.ItemsSource = materials;
            _mainFrame = frame;
            this.DataContext = this;
            LoadView();
            UpdatePagination();
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

            totalPages = (int)Math.Ceiling((double)materials.Count / pageSize);
            ShowCurrentPage();
        }

        private void ShowCurrentPage()
        {
            // Показываем только элементы текущей страницы
            var currentPageMaterials = materials
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            listRecipes.ItemsSource = currentPageMaterials;
        }

        private void UpdatePagination()
        {
            PageNumbers.Clear();

            int totalButtons = 5; // Фиксированное количество кнопок
            int startPage = 1;
            int endPage = totalPages;

            // Если страниц больше чем кнопок, вычисляем диапазон
            if (totalPages > totalButtons)
            {
                // Стараемся показывать текущую страницу в центре
                startPage = Math.Max(1, currentPage - totalButtons / 2);
                endPage = startPage + totalButtons - 1;

                // Если вышли за пределы справа - корректируем
                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    startPage = Math.Max(1, endPage - totalButtons + 1);
                }
                // Если вышли за пределы слева - корректируем
                else if (startPage < 1)
                {
                    startPage = 1;
                    endPage = Math.Min(totalPages, totalButtons);
                }
            }

            for (int i = startPage; i <= endPage; i++)
            {
                PageNumbers.Add(i);
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ShowCurrentPage();
                UpdatePagination();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ShowCurrentPage();
                UpdatePagination();
            }
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            currentPage = (int)button.Tag;
            ShowCurrentPage();
            UpdatePagination();
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
