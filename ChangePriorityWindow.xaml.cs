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
using System.Windows.Shapes;

namespace Lopatkin_Glazki
{
    /// <summary>
    /// Логика взаимодействия для ChangePriorityWindow.xaml
    /// </summary>
    public partial class ChangePriorityWindow : Window
    {
        public int NewPriority { get; set; }
        public ChangePriorityWindow(List<Agent> selectedAgents)
        {
            InitializeComponent();
            int maxPriority = selectedAgents.Max(agent => agent.Priority);
            MaxPriorityTextBox.Text = maxPriority.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            NewPriority = int.Parse(NewPriorityTextBox.Text);
         
            DialogResult = true;

           
            Close();
        }
    }
}
