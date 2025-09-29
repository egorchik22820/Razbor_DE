using Microsoft.Win32;
using Razbor_DE.AppData;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;

namespace Razbor_DE.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddEditMaterial.xaml
    /// </summary>
    public partial class AddEditMaterial : Page
    {
        private Materials _currentMaterial = new Materials();
        private Frame _mainFrame;

        private string _selectedImagePath = null;
        public AddEditMaterial(Frame mainFrame)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _currentMaterial = new Materials();
            DataContext = _currentMaterial;

            LoadUnits();
            LoadTypes();
        }

        public AddEditMaterial(Frame mainFrame, Materials material)
        {
            InitializeComponent();

            if (_currentMaterial != null)
                _currentMaterial = material;

            _mainFrame = mainFrame;
            DataContext = _currentMaterial;

            LoadUnits();
            LoadTypes();
        }

        private void LoadUnits()
        {
            var units = AppData.AppConnect.modelDB.Units.ToList();
            if (units.Any())
                txtUnit.ItemsSource = units;
        }

        private void LoadTypes()
        {
            var types = AppData.AppConnect.modelDB.MaterialType.ToList();
            if(types.Any())
                txtTypeName.ItemsSource = types;
        }

        public string SaveImg(string path)
        {
            try
            {
                string imgFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                Directory.CreateDirectory(imgFolderPath);

                string fileName = Path.GetFileName(path);
                string destinationPath = Path.Combine(imgFolderPath, fileName);

                File.Copy(path, destinationPath, true);

                return Path.Combine(imgFolderPath, fileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var type = AppData.AppConnect.modelDB.MaterialType.FirstOrDefault(x => x.Name == txtTypeName.Text);
                var unit = AppData.AppConnect.modelDB.Units.FirstOrDefault(x => x.Name == txtUnit.Text);

                if (_currentMaterial == null || type == null || unit == null)
                {
                    MessageBox.Show("Проверьте введенные данные.");
                    return;
                }

                _currentMaterial.TypeId = type.Id;
                _currentMaterial.UnitId = unit.Id;

                if (_currentMaterial.Id == 0)
                    AppData.AppConnect.modelDB.Materials.Add(_currentMaterial);

                AppData.AppConnect.modelDB.SaveChanges();
                MessageBox.Show("Данные сохранены успешно!");
                _mainFrame.Navigate(new DataOutPage(_mainFrame));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*jpeg)|*.png;*.jpg;*jpeg",
                Title = "Выберите изображение"
            };

            if (dialog.ShowDialog() == true)
            {
                string savedPath = SaveImg(dialog.FileName);

                if (savedPath != null)
                {
                    _currentMaterial.ImagePath = savedPath;
                    MessageBox.Show("Изображение успешно добавлено!" + $"\n{savedPath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DataOutPage(_mainFrame));
        }
    }
}
