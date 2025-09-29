using Razbor_DE.AppData;
using Razbor_DE.Pages.Admin;
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

namespace Razbor_DE.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataOutPage.xaml
    /// </summary>
    public partial class DataOutPage : Page
    {
        private Frame _mainFrame;
        public List<Materials> materials;

        private List<Materials> filteredMaterials = new List<Materials>();
        private List<MaterialType> materialTypes;

        private int currentPage = 1;
        private int pageSize = 3; // Количество элементов на странице
        private int totalPages;

        public ObservableCollection<int> PageNumbers {  get; set; } = new ObservableCollection<int>();

        public DataOutPage(Frame frame)
        {
            InitializeComponent();
            materials = new List<Materials>();
            listMaterials.ItemsSource = materials;
            _mainFrame = frame;
            this.DataContext = this;
            ComboSort.SelectedIndex = 2;

            LoadMaterialTypes();
            LoadView();
            UpdatePagination();
        }

        private void LoadMaterialTypes()
        {
            // Загружаем типы материалов из БД
            materialTypes = AppData.AppConnect.modelDB.MaterialType.ToList();

            // Добавляем вариант "Все типы"
            var allTypes = new MaterialType { Id = 0, Name = "Все типы" };
            materialTypes.Insert(0, allTypes);

            ComboBox.ItemsSource = materialTypes;
            ComboBox.SelectedIndex = 0; // Выбираем "Все типы" по умолчанию
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

                filteredMaterials = materials;
            }

            totalPages = (int)Math.Ceiling((double)materials.Count / pageSize);
            UpdatePagination();
            ShowCurrentPage();
        }

        private void ShowCurrentPage()
        {
            if (filteredMaterials.Count == 0)
            {
                listMaterials.ItemsSource = new ObservableCollection<Materials>(); // пустой список
                return;
            }

            // Берем данные из filteredMaterials
            var currentPageMaterials = filteredMaterials
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            listMaterials.ItemsSource = currentPageMaterials;
        }

        private void UpdatePagination()
        {
            PageNumbers.Clear();

            // Используем filteredMaterials для расчета страниц
            totalPages = (int)Math.Ceiling((double)filteredMaterials.Count / pageSize);

            if (totalPages == 0) totalPages = 1; // минимум 1 страница

            int totalButtons = 5;
            int startPage = Math.Max(1, currentPage);
            int endPage = Math.Min(totalPages, startPage + totalButtons - 1);

            if (endPage - startPage + 1 < totalButtons && totalPages > totalButtons)
            {
                startPage = Math.Max(1, totalPages - totalButtons + 1);
                endPage = totalPages;
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
            _mainFrame.Navigate(new AddEditMaterial(_mainFrame));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listMaterials.SelectedItem is Materials selectedMaterial)
            {
                if (listMaterials.SelectedItem != null)
                    _mainFrame.Navigate(new AddEditMaterial(_mainFrame, selectedMaterial));
                else
                    MessageBox.Show("Выберите материал для редактирования!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = listMaterials.SelectedItems.Cast<Materials>().ToList();

            if (listMaterials.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
                return;
            }

            if (MessageBox.Show("Вы точно хотите удалить элемент?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.AppConnect.modelDB.Materials.RemoveRange(selectedItems);
                    AppData.AppConnect.modelDB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    LoadView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TextSearch.Text.ToLower();

            // Фильтруем весь список
            filteredMaterials = materials
                .Where(x => x.Name.ToLower().Contains(searchText))
                .ToList();

            // Сбрасываем на первую страницу
            currentPage = 1;

            // Обновляем пагинацию и показываем текущую страницу
            UpdatePagination();
            ShowCurrentPage();
        }

        private void listRecipes_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSort.SelectedItem is ComboBoxItem selectedItem)
            {

                switch (ComboSort.SelectedIndex)
                {
                    case 0:
                        filteredMaterials = filteredMaterials.OrderBy(x => x.Name).ToList();
                        break;

                    case 1:
                        filteredMaterials = filteredMaterials.OrderBy(x => x.QuantityInStorage).ToList();
                        break;

                    default:
                        string searchText = TextSearch.Text.ToLower();

                        filteredMaterials = materials.Where(x =>
                        x.Name.ToLower().Contains(searchText))
                        .ToList();
                        break;

                }

                // Сбрасываем на первую страницу
                currentPage = 1;

                // Обновляем пагинацию и показываем текущую страницу
                UpdatePagination();
                ShowCurrentPage();
            }
        }

        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox.SelectedItem is MaterialType selectedType)
            {
                // Фильтруем по типу материала
                if (selectedType.Id == 0) // "Все типы"
                {
                    filteredMaterials = materials.ToList();
                }
                else
                {
                    filteredMaterials = materials
                        .Where(x => x.MaterialType.Id == selectedType.Id)
                        .ToList();
                }

                // Применяем текущий поиск и сортировку
                ApplyFiltersAndSort();
            }
        }

        private void ApplyFiltersAndSort()
        {
            // Применяем текстовый поиск
            string searchText = TextSearch.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredMaterials = filteredMaterials
                    .Where(x => x.Name.ToLower().Contains(searchText))
                    .ToList();
            }

            // Применяем сортировку
            if (ComboSort.SelectedItem is ComboBoxItem selectedSort)
            {
                switch (ComboSort.SelectedIndex)
                {
                    case 0:
                        filteredMaterials = filteredMaterials.OrderBy(x => x.Name).ToList();
                        break;
                    case 1:
                        filteredMaterials = filteredMaterials.OrderBy(x => x.QuantityInStorage).ToList();
                        break;
                }
            }

            // Сбрасываем на первую страницу и обновляем
            currentPage = 1;
            UpdatePagination();
            ShowCurrentPage();
        }
    }
}
