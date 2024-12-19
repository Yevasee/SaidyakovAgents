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
    /// Логика взаимодействия для ProductSaleForAgent.xaml
    /// </summary>
    public partial class ProductSaleForAgent : Page
    {
        private Agent _currentAgent = new Agent();
        public ProductSaleForAgent(Agent selectedAgent)
        {
            InitializeComponent();

            if (selectedAgent != null)
            {
                _currentAgent = selectedAgent;
            }

            UpdateListView();
        }

        private void UpdateListView()
        {
            var currentProductSales = SaidyakovEyesSaveEntities.GetContext().ProductSale.ToList();
            currentProductSales = currentProductSales.Where(p => (p.AgentID == _currentAgent.ID)).ToList();
            currentProductSales = currentProductSales.Where(p => (p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower()))).ToList();
            ListViewProductSales.ItemsSource = currentProductSales;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListView();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewProductSales.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну продажу", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedProductSales = ListViewProductSales.SelectedItems.Cast<ProductSale>().ToList();

            //var currentAgentPriorityHistory = SaidyakovEyesSaveEntities.
            //    GetContext().AgentPriorityHistory.ToList();
            //var currentShop = SaidyakovEyesSaveEntities.GetContext().Shop.ToList();
            //currentAgentPriorityHistory = currentAgentPriorityHistory.
            //    Where(p => p.AgentID == _currentAgent.ID).ToList();
            //currentShop = currentShop.Where(p => p.AgentID == _currentAgent.ID).ToList();


            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var productSale in selectedProductSales)
                    {
                        SaidyakovEyesSaveEntities.GetContext().ProductSale.Remove(productSale);
                    }
                    SaidyakovEyesSaveEntities.GetContext().SaveChanges();

                    MessageBox.Show("Информация удалена!");
                    UpdateListView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DialogAddProduct(_currentAgent.ID);
            dialog.ShowDialog();
            UpdateListView();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                SaidyakovEyesSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                //ListViewProductSales.ItemsSource = SaidyakovEyesSaveEntities.GetContext().ProductSale.ToList();
                UpdateListView();
            }
        }
    }
}
