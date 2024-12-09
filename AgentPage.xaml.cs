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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = SaidyakovEyesSaveEntities.GetContext().Agent.ToList();

            AgentListView.ItemsSource = currentAgents;

            ComboTypeAgentType.SelectedIndex = 0;
            ComboTypeSort.SelectedIndex = 0;

            UpdateAgents();
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if(CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean Ifupdate = true;

            int min;

            if(selectedPage.HasValue)
            {
                if(selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for(int i = CurrentPage*10; i < min; i++)
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
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage *10+10:CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for(int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                AgentListView.ItemsSource = CurrentPageList;

                AgentListView.Items.Refresh();
            }
        }
        private void UpdateAgents()
        {
            var currentAgents = SaidyakovEyesSaveEntities.GetContext().Agent.ToList();

            currentAgents = currentAgents.Where(p=>(p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("(","").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(TBoxSearch.Text)
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()))).ToList();


            switch(ComboTypeAgentType.SelectedIndex)
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
                //case 0: currentAgents = currentAgents.ToList(); break;
                case 1: currentAgents = currentAgents.OrderBy(p => p.Title).ToList(); break;
                case 2: currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList(); break;
                case 5: currentAgents = currentAgents.OrderBy(p => p.Priority).ToList(); break;
                case 6: currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList(); break;
            }

            AgentListView.ItemsSource = currentAgents;

            TableList = currentAgents;
            ChangePage(0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
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

        private void ComboTypeAgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void LeftDirBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

    }
}
