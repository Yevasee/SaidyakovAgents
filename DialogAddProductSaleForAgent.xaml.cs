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
    /// Логика взаимодействия для DialogAddProduct.xaml
    /// </summary>
    public partial class DialogAddProduct : Window
    {
        private ProductSale _productSale = new ProductSale();
        public DialogAddProduct(int agentID)
        {
            InitializeComponent();
            _productSale.AgentID = agentID;
            
            var items = SaidyakovEyesSaveEntities.GetContext().Product.ToList();
            cmbProducts.ItemsSource = items;
            cmbProducts.DisplayMemberPath = "Title";
            cmbProducts.SelectedValuePath = "ID";
            //cmbProducts.SelectedIndex = 0;
            TextBoxCount.Text = "1";
            TextBoxSaleDate.Text = "2024-12-31";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if(cmbProducts.SelectedItem == null)
            {
                errors.AppendLine("Введите имя продукта");
            }

            if (string.IsNullOrWhiteSpace(TextBoxCount.Text))
            {
                errors.AppendLine("Введите количество продукции");
            }
            if (!int.TryParse(TextBoxCount.Text, out int count))
            {
                errors.AppendLine("Количество продукции должно быть целочисленным!");
            }
            if (string.IsNullOrWhiteSpace(TextBoxSaleDate.Text))
            {
                errors.AppendLine("Введите дату продажи");
            }
            ;
            if (!DateTime.TryParse(TextBoxSaleDate.Text, out DateTime date))
            {
                errors.AppendLine("Неправильный формат даты!");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _productSale.ProductID = cmbProducts.SelectedIndex + 1;
            _productSale.ProductCount = count;
            _productSale.SaleDate = date;
            try
            {
                SaidyakovEyesSaveEntities.GetContext().ProductSale.Add(_productSale);
                SaidyakovEyesSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = SaidyakovEyesSaveEntities.GetContext().Product.ToList();
            if (/*cmbProducts.SelectedIndex == -1 ||*/ TextBoxSearch.Text != cmbProducts.Text)
            {
                items = items.Where(p => (p.Title.ToLower().Contains(TextBoxSearch.Text.ToLower()))).ToList();
                //cmbProducts.DisplayMemberPath = "Title";
                //cmbProducts.SelectedValuePath = "ID";
                cmbProducts.IsDropDownOpen = true;
                cmbProducts.ItemsSource = items;
            }
        }

        private void cmbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TextBoxSearch.Text = cmbProducts.Text;
            var items = SaidyakovEyesSaveEntities.GetContext().Product.ToList();
            cmbProducts.IsDropDownOpen = true;
            cmbProducts.ItemsSource = items;

            if (!string.IsNullOrEmpty(cmbProducts.Text))
            {
                TextBoxSearch.Text = cmbProducts.Text;
            }
        }
        //private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var items = SaidyakovEyesSaveEntities.GetContext().Product.ToList();
        //    items = items.Where(p => (p.Title.ToLower().Contains(SearchTextBox.Text.ToLower()))).ToList();
        //    cmbProducts.ItemsSource = items;
        //    cmbProducts.DisplayMemberPath = "Title";
        //    cmbProducts.SelectedValuePath = "ID";
        //}
    }
}
