using System;
using System.Collections.Generic;
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

namespace Razbor_DE.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        private Frame _mainFrame;
        public AutorizationPage(Frame frame)
        {
            InitializeComponent();
            _mainFrame = frame;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameBox.Text;
            string password = PasswordBox.Password;

            var user = AppData.AppConnect.modelDB.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                MessageBox.Show($"Добро пожаловать, {user.Login}!");
                if (user.UserTypes.Name.ToString() == "admin")
                    _mainFrame.Navigate(new DataOutPage(_mainFrame)); // переход на главную страницу
                else
                    _mainFrame.Navigate(new Pages.User.DataOutUserPage(_mainFrame));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
