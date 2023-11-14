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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
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

            currentGlazki = currentGlazki.Where(p =>
                                                p.Title.ToLower().Contains(TBSearch.Text.ToLower())||
                                                p.Email.ToLower().Contains(TBSearch.Text.ToLower())||
                                                p.Phone.ToLower().Replace("+","").Replace("(","").Replace(")","").Replace(" ","").Replace("-","").Contains(TBSearch.Text.ToLower())).ToList();



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
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "МФО").ToList();
                }
                if (Filtraciya.SelectedIndex == 1)
                {
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ЗАО").ToList();
                }
                if (Filtraciya.SelectedIndex == 2)
                {
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "МКК").ToList();
                }
                if (Filtraciya.SelectedIndex == 3)
                {
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ОАО").ToList();
                }
                if (Filtraciya.SelectedIndex == 4)
                {
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ООО").ToList();
                }
                if (Filtraciya.SelectedIndex == 5)
                {
                    currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ПАО").ToList();
                }
                GlazkiListView.ItemsSource = currentGlazki;
            TableList = currentGlazki;
            ChangePage(0, 0);

           
      
            GlazkiListView.ItemsSource = currentGlazki;
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if(CountRecords %10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupadate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if(selectedPage>=0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for(int i = CurrentPage*10;i< min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if(CurrentPage > 0)
                        {
                            CurrentPage--;
                            min=CurrentPage*10+10<CountRecords ? CurrentPage*10+10 : CountRecords;
                            for(int i= CurrentPage*10;i< min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupadate = false;
                        }
                        break;

                        case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min= CurrentPage*10+10<CountRecords ? CurrentPage*10+10 : CountRecords;
                            for(int i = CurrentPage*10;i< min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupadate = false;
                        }
                        break;
                }
            }
            if (Ifupadate)
            {
                PageListBox.Items.Clear();
                for(int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBcount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                GlazkiListView.ItemsSource = CurrentPageList;
                GlazkiListView.Items.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            ObnovlenieStranicy();
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

        private void Sortirovka_LostFocus(object sender, RoutedEventArgs e)
        {
            ObnovlenieStranicy();
        }

        private void PoYbovaniy_Checked(object sender, RoutedEventArgs e)
        {
            ObnovlenieStranicy();
        }

        private void PoVozrast_Checked(object sender, RoutedEventArgs e)
        {
            ObnovlenieStranicy();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);
        }

        private void OpenAddEditPage_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
           
        }

        private void Redactirovanie_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility==Visibility.Visible)
            {
                Lopatkin_GlazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                GlazkiListView.ItemsSource = Lopatkin_GlazkiEntities.GetContext().Agent.ToList();
            }
            ObnovlenieStranicy();
        }
    }
}
