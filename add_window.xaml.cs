using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для add_window.xaml
    /// </summary>
    public partial class add_window : Page
    {

        private Абоненты _currentGlazki = new Абоненты();
    
        public int id;
        public string name;
        List<Абоненты> AgentTypesDBList = ATS_LopatkinEntities4.GetContext().Абоненты.ToList();
        public add_window(Абоненты selectedGlazki)
        {

            InitializeComponent();
            if (selectedGlazki != null)
            {
                _currentGlazki = selectedGlazki;
                id = _currentGlazki.ID_абонента;
                name = _currentGlazki.Вид_телефона;
                switch (name)
                {
                    case "Основной":
                        ComboType.SelectedIndex = 0;
                        break;
                    case "Параллельный":
                        ComboType.SelectedIndex = 1;
                        break;
                    case "Спаренный":
                        ComboType.SelectedIndex = 2;
                        break;
                }

            }
            DataContext = _currentGlazki;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorsList = new List<string>();

            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Имя), "Укажите названия агента");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Фамилия), "Укажите адрес");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Отчество), "Укажите директора");
            ValidateAndAddError(() => ComboType.SelectedItem == null, "Укажите тип Номера");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.ID_ATS.ToString()), "Укажите ID станции");

            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Дом), "Укажите дом");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Улица), "Укажите улицу");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Номер_телефона) || !IsValidPhoneNumber(_currentGlazki.Номер_телефона), "Укажите правильный телефон");
            ValidateAndAddError(() => string.IsNullOrWhiteSpace(_currentGlazki.Район), "Укажите район");

            var currentTypeContent = ((TextBlock)ComboType.SelectedItem)?.Text;

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
                if (_currentGlazki.ID_абонента == 0) // Проверка на нулевое значение ID
                {
                    _currentGlazki.Вид_телефона = ComboType.Text;
                    ATS_LopatkinEntities4.GetContext().Абоненты.Add(_currentGlazki);
                   
                }

                try
                {
                    ATS_LopatkinEntities4.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");

                    // Вызов обновления страницы
                    var parentPage = Manager.MainFrame.Content as Glazki_Page;
                    parentPage?.ObnovlenieStranicy();

                    Manager.MainFrame.GoBack();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Ошибка свойства: {validationError.PropertyName}\nСообщение: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
        }

    




    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        var currentAgent = (sender as Button).DataContext as Абоненты;
        if (currentAgent.End_contract > DateTime.Now)
        {
            MessageBox.Show("Невозможно выполнить удаление, так как контракт ещё не закончен");
        }
        else
        {
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ATS_LopatkinEntities4.GetContext().Абоненты.Remove(currentAgent);
                    ATS_LopatkinEntities4.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена");

                    // Вызов обновления страницы
                    var parentPage = Manager.MainFrame.Content as Glazki_Page;
                    parentPage?.ObnovlenieStranicy();
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

