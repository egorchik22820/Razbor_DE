﻿using Razbor_DE.AppData;
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

namespace Razbor_DE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppData.AppConnect.modelDB = new RazborDBEntities1();
            AppData.AppFrame.frameMain = frmMain;

            frmMain.Navigate(new Pages.Admin.AutorizationPage(frmMain)); // Передаём Frame в Page
        }
    }
}
