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
        List<Абоненты> CurrentPageList = new List<Абоненты>();
        List<Абоненты> TableList;
      
        public Glazki_Page()
        {
            InitializeComponent();
            var currentGlazki = ATS_LopatkinEntities4.GetContext().Абоненты.ToList();

            
           
            ObnovlenieStranicy();
        }
        private string GetAgentTypeStringByIndex(int index)
        {
            switch (index)
            {
                case 0: return "Основной";
                case 1: return "Параллельный";
                case 2: return "Спаренный";
               
                default: return string.Empty;
            }
        }

        
        public void ObnovlenieStranicy()
        {
            
            var currentGlazki = ATS_LopatkinEntities4.GetContext().Абоненты.ToList();

            currentGlazki = currentGlazki.Where(p =>
                                                p.Имя.ToLower().Contains(TBSearch.Text.ToLower())||
                                                p.Фамилия.ToLower().Contains(TBSearch.Text.ToLower())||
                                                p.Номер_телефона.ToLower().Replace("+","").Replace("(","").Replace(")","").Replace(" ","").Replace("-","").Contains(TBSearch.Text.ToLower())).ToList();



        
                switch (Sortirovka.SelectedIndex)
                {
                    case 0:
                        currentGlazki = currentGlazki.OrderBy(p => p.Имя).ToList();
                        break;
                    case 1:
                        currentGlazki = currentGlazki.OrderByDescending(p => p.Имя).ToList();
                        break;
                    case 2:
                        currentGlazki = currentGlazki.OrderBy(p => p.Фамилия).ToList();
                        break;
                    case 3:
                        currentGlazki = currentGlazki.OrderByDescending(p => p.Фамилия).ToList();
                        break;
                }
            if (Filtraciya.SelectedIndex >= 0 && Filtraciya.SelectedIndex <= 5)
            {
                string agentType = GetAgentTypeStringByIndex(Filtraciya.SelectedIndex);
                currentGlazki = currentGlazki.Where(p => p.Вид_телефона == agentType).ToList();
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

      
        private void edit(object sender, RoutedEventArgs e)
        {
            Абоненты selectedAgent = (sender as Button).DataContext as Абоненты;
            Manager.MainFrame.Navigate(new add_window((sender as Button).DataContext as Абоненты));
        }

        private void OpenAddEditPage_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new add_window(null));
        }

        private void GlazkiListView_Loaded(object sender, RoutedEventArgs e)
        {
            ObnovlenieStranicy();
        }
    }
}
