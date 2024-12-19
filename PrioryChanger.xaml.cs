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

namespace SaidyakovAgents
{
    /// <summary>
    /// Логика взаимодействия для PrioryChanger.xaml
    /// </summary>
    public partial class PrioryChanger : Window
    {
        private System.Collections.IList _currentAgents;

        public PrioryChanger(System.Collections.IList selectedAgents)
        {
            InitializeComponent();
            _currentAgents = selectedAgents;
            TextBoxPriority.Text = Convert.ToString(_currentAgents.
                Cast<Agent>().ToList().Max(p => p.Priority));
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(TextBoxPriority.Text))
            {
                errors.AppendLine("Введите проритет");
            }
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                int newPriority = int.Parse(TextBoxPriority.Text);

                foreach (Agent agent in _currentAgents)
                {
                    agent.Priority = newPriority;
                }
                SaidyakovEyesSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
