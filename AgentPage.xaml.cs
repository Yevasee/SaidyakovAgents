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
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = SaidyakovEyesSaveEntities.GetContext().Agent.ToList();

            AgentListView.ItemsSource = currentAgents;

            ComboTypeDiscnt.SelectedIndex = 0;
            ComboTypeSort.SelectedIndex = 0;

            UpdateAgents();
        }

        private void UpdateAgents()
        {
            var currentAgents = SaidyakovEyesSaveEntities.GetContext().Agent.ToList();

            currentAgents = currentAgents.Where(p=>p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            AgentListView.ItemsSource = currentAgents.ToList();

            switch(ComboTypeDiscnt.SelectedIndex)
            {
                case 1: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 1)).ToList(); break;
                case 2: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 2)).ToList(); break;
                case 3: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 3)).ToList(); break;
                case 4: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 4)).ToList(); break;
                case 5: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 5)).ToList(); break;
                case 6: currentAgents = currentAgents.Where(p => (p.AgentTypeID == 6)).ToList(); break;
            }

            switch(ComboTypeSort.SelectedIndex)
            {
                case 0: AgentListView.ItemsSource = currentAgents.ToList(); break;
                case 1: AgentListView.ItemsSource = currentAgents.OrderBy(p => p.Title).ToList(); break;
                case 2: AgentListView.ItemsSource = currentAgents.OrderByDescending(p => p.Title).ToList(); break;
                case 3: AgentListView.ItemsSource = currentAgents.OrderBy(p => p.Priority).ToList(); break;
                case 4: AgentListView.ItemsSource = currentAgents.OrderByDescending(p => p.Priority).ToList(); break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboTypeDiscnt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboTypeSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnAtoZ_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnZtoA_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnPrioryUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnPrioryDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnDiscntUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RBtnDiscntDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
