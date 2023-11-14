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
    public partial class AddEditPage : Page
    {
        private Agent _currentGlazki = new Agent();
        List<AgentType> AgentTypesDBList = Lopatkin_GlazkiEntities.GetContext().AgentType.ToList();
        public AddEditPage(Agent selectedGlazki)
        {
            InitializeComponent();
            if (selectedGlazki != null)
            {
                _currentGlazki = selectedGlazki;
                ComboType.SelectedIndex = _currentGlazki.AgentTypeID-1;
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
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentGlazki.Title))
                errors.AppendLine("Укажите названия агента");
            if (string.IsNullOrWhiteSpace(_currentGlazki.Address))
                errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(_currentGlazki.DirectorName))
                errors.AppendLine("Укажите директора");
            if (ComboType.SelectedItem==null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentGlazki.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentGlazki.Priority<0)
                errors.AppendLine("Укажите положительные приоритет   агента");
            if (string.IsNullOrWhiteSpace(_currentGlazki.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentGlazki.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentGlazki.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentGlazki.Phone.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
                if ((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11
                    || ph[1] == '3' && ph.Length != 12) 
                errors.AppendLine("Укажите правильный телефон");
            }
            if (string.IsNullOrWhiteSpace(_currentGlazki.Email))
                errors.AppendLine("Укажите почту агента");

            var currentType = (TextBlock)ComboType.SelectedItem;
            string currentTypeContent = currentType.Text;
            foreach(AgentType type in AgentTypesDBList)
            {
                if (type.Title.ToString() == currentTypeContent)
                {
                    _currentGlazki.AgentType= type;
                    _currentGlazki.AgentTypeID = type.ID;
                    break;
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentGlazki.ID == 0)
            {
                Lopatkin_GlazkiEntities.GetContext().Agent.Add(_currentGlazki);  
            }
            try
            {
                Lopatkin_GlazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentGlazki = (sender as Button).DataContext as Agent;
            var currentGlazkkiAgent = Lopatkin_GlazkiEntities.GetContext().ProductSale.ToList();
            currentGlazkkiAgent = currentGlazkkiAgent.Where(p => p.AgentID == currentGlazki.ID).ToList();

            if (currentGlazkkiAgent.Count != 0)
                MessageBox.Show("Невозможно удалить так как есть продажи");
            else
            {


                if (MessageBox.Show("Вы точно хотите выполнить удаление?","Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Lopatkin_GlazkiEntities.GetContext().Agent.Remove(currentGlazki);
                        Lopatkin_GlazkiEntities.GetContext().SaveChanges();
                        MessageBox.Show("Запись удалена");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ToString());
                    }
                }
            }
        }
    }
}
