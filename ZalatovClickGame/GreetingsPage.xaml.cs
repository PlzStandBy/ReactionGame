using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ZalatovClickGame
{
    /// <summary>
    /// Логика взаимодействия для GreetingsPage.xaml
    /// </summary>
    public partial class GreetingsPage : Page
    {
        public GreetingsPage()
        {
            InitializeComponent();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}
