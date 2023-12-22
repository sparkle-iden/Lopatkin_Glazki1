using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    /// 
    public partial class AddEditPage : Page
    {
        private ProductSale _selectedSale;
        private Agent _currentGlazki = new Agent();
        private List<Product> _availableProducts;
        public int id;
        List<AgentType> AgentTypesDBList = Lopatkin_GlazkiEntities.GetContext().AgentType.ToList();
        public AddEditPage(Agent selectedGlazki)
        {
            InitializeComponent();
           
            if (selectedGlazki != null)
            {
                _currentGlazki = selectedGlazki;
                id = _currentGlazki.ID;
                ComboType.SelectedIndex = _currentGlazki.AgentTypeID - 1;
            }
            DataContext = _currentGlazki;

        
        }


     

       
        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentGlazki.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorsList = new List<string>();

            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Title), "Укажите названия агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Address), "Укажите адрес");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.DirectorName), "Укажите директора");
            ValidateAndAddError(() => ComboType.SelectedItem == null, "Укажите тип агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Priority.ToString()), "Укажите приоритет агента");
            ValidateAndAddError(() => _currentGlazki.Priority < 0, "Укажите положительный приоритет агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.INN), "Укажите ИНН агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.KPP), "Укажите КПП агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Phone) || !IsValidPhoneNumber(_currentGlazki.Phone), "Укажите правильный телефон");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Email), "Укажите почту агента");

            var currentTypeContent = ((TextBlock)ComboType.SelectedItem)?.Text;
            var selectedAgentType = AgentTypesDBList.FirstOrDefault(type => type.Title.ToString() == currentTypeContent);

            if (selectedAgentType != null)
            {
                _currentGlazki.AgentType = selectedAgentType;
                _currentGlazki.AgentTypeID = selectedAgentType.ID;
            }

            if (errorsList.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorsList));
                return;
            }

            SaveAgentInformation();

            void ValidateAndAddError(Func<bool> condition, string errorMessage)
            {
                if (condition.Invoke())
                {
                    errorsList.Add(errorMessage);
                }
            }

            bool IsValidPhoneNumber(string phoneNumber)
            {
                string cleanedPhoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
                return (cleanedPhoneNumber[1] == '9' || cleanedPhoneNumber[1] == '4' || cleanedPhoneNumber[1] == '8') && cleanedPhoneNumber.Length == 11
                       || cleanedPhoneNumber[1] == '3' && cleanedPhoneNumber.Length == 12;
            }

            void SaveAgentInformation()
            {
                if (_currentGlazki.ID == 0)
                {
                    Lopatkin_GlazkiEntities.GetContext().Agent.Add(_currentGlazki);
                }

                try
                {
                    Lopatkin_GlazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void HisBtn_Click(object sender, RoutedEventArgs e)
        {
            History window = new History(_currentGlazki);
            window.Show();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;
            var curruntProductSale = Lopatkin_GlazkiEntities.GetContext().ProductSale.ToList();
            curruntProductSale = curruntProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (curruntProductSale.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление,так как существует реализация продукции");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Lopatkin_GlazkiEntities.GetContext().Agent.Remove(currentAgent);
                        Lopatkin_GlazkiEntities.GetContext().SaveChanges();
                        MessageBox.Show("Запись удалена");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
    

