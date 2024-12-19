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

namespace SaidyakovAgents
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
            }

            CmbTypeAgentType.SelectedIndex = _currentAgent.AgentTypeID-1;
            DataContext = _currentAgent;
        }

        private void ChangePicBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog  myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;

                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
            {
                errors.AppendLine("Укажите наименование агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
            {
                errors.AppendLine("Укажите адрес агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
            {
                errors.AppendLine("Укажите ФИО директора");
            }
            if(CmbTypeAgentType.SelectedItem == null)
            {
                errors.AppendLine("Укажите тип агента");
            }
            else
            {
                _currentAgent.AgentTypeID = CmbTypeAgentType.SelectedIndex+1;
                _currentAgent.AgentTypeTitle = CmbTypeAgentType.SelectedItem.ToString();
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
            {
                errors.AppendLine("Укажите приоритет агента");
            }
            if(_currentAgent.Priority <= 0)
            {
                errors.AppendLine("Укажите положительный приоритет агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
            {
                errors.AppendLine("Укажите ИНН агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
            {
                errors.AppendLine("Укажите КПП агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
            {
                errors.AppendLine("Укажите телефон агента");
            }
            else
            {
                string ph = _currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1]=='9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11)
                    || (ph[1] == '3' && ph.Length != 12)){
                    errors.AppendLine("Укажите правильно телефон агента");
                }
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
            {
                errors.AppendLine("Укажите почту агента");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(_currentAgent.ID == 0)
            {
                SaidyakovEyesSaveEntities.GetContext().Agent.Add(_currentAgent);
            }


            try
            {
                SaidyakovEyesSaveEntities.GetContext().SaveChanges();
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
            var _currentAgent = (sender as Button).DataContext as Agent;

            var currentProductSale = SaidyakovEyesSaveEntities.GetContext().ProductSale.ToList();
            currentProductSale = currentProductSale.Where(p => p.AgentID == _currentAgent.ID).ToList();

            if (currentProductSale.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существует история реализации продуктов");
            else
            {
                var currentAgentPriorityHistory = SaidyakovEyesSaveEntities.
                    GetContext().AgentPriorityHistory.ToList();
                var currentShop = SaidyakovEyesSaveEntities.GetContext().Shop.ToList();
                currentAgentPriorityHistory = currentAgentPriorityHistory.
                    Where(p => p.AgentID == _currentAgent.ID).ToList();
                currentShop = currentShop.Where(p => p.AgentID == _currentAgent.ID).ToList();


                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaidyakovEyesSaveEntities.GetContext().Agent.Remove(_currentAgent);

                        if (currentAgentPriorityHistory.Count != 0)
                        {
                            for (int i = 0; currentAgentPriorityHistory.Count == i; i++)
                            { 
                                SaidyakovEyesSaveEntities.GetContext().AgentPriorityHistory.
                                    Remove(currentAgentPriorityHistory[i]); 
                            }
                        }
                        if (currentShop.Count != 0)
                        {
                            for (int i = 0; currentShop.Count == i; i++)
                            {
                                SaidyakovEyesSaveEntities.GetContext().Shop.Remove(currentShop[i]);
                            }
                        }
                        SaidyakovEyesSaveEntities.GetContext().SaveChanges();

                        MessageBox.Show("Информация удалена!");
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void BtnHistorySaleForAgent_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductSaleForAgent((sender as Button).DataContext as Agent));
        }
    }
}
