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

namespace Lopatkin_Glazki
{
    /// <summary>
    /// Логика взаимодействия для Glazki_Page.xaml
    /// </summary>
    public partial class Glazki_Page : Page
    {
        public Glazki_Page()
        {
            InitializeComponent();
            var currentGlazki = Lopatkin_GlazkiEntities.GetContext().Agent.ToList();
            GlazkiListView.ItemsSource = currentGlazki;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
