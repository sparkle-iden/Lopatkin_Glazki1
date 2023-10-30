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
            
            
            ObnovlenieStranicy();
        }
        private void ObnovlenieStranicy()
        {

            var currentGlazki = Lopatkin_GlazkiEntities.GetContext().Agent.ToList();

            currentGlazki = currentGlazki.Where(p => (p.Title.ToLower().Contains(TBSearch.Text.ToLower()))).ToList();


            if (Sortirovka.SelectedIndex == 0)
            {
                currentGlazki = currentGlazki.OrderBy(p => p.Title).ToList();
            }

            if (Sortirovka.SelectedIndex == 4)
            {
                currentGlazki = currentGlazki.OrderBy(p => p.Priority).ToList();
            }

            if (Sortirovka.SelectedIndex == 1)
            {
                currentGlazki = currentGlazki.OrderByDescending(p => p.Title).ToList();
            }

            if (Sortirovka.SelectedIndex == 5)
            {
                currentGlazki = currentGlazki.OrderByDescending(p => p.Priority).ToList();
            }


            if (Filtraciya.SelectedIndex == 0)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "МФО")).ToList();
            }
            if (Filtraciya.SelectedIndex == 1)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "ЗАО")).ToList();
            }
            if (Filtraciya.SelectedIndex == 2)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "МКК")).ToList();
            }
            if (Filtraciya.SelectedIndex == 3)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "ОАО")).ToList();
            }
            if (Filtraciya.SelectedIndex == 4)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "ООО")).ToList();
            }
            if (Filtraciya.SelectedIndex == 5)
            {
                currentGlazki = currentGlazki.Where(p => (p.AgentTypeString == "ПАО")).ToList();
            } 
            GlazkiListView.ItemsSource = currentGlazki;
        }

        private void Sortirovka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObnovlenieStranicy();
        }

        private void Filtraciya_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObnovlenieStranicy();
        }
  

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObnovlenieStranicy();
        }

    }
}
