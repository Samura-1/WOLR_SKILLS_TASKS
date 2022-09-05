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

namespace CompanyNine
{
    /// <summary>
    /// Логика взаимодействия для AddNewProduct.xaml
    /// </summary>
    public partial class AddNewProduct : Window
    {
        private Product _addproduct = new Product();
        public AddNewProduct()
        {
            InitializeComponent();
            DataContext = _addproduct;
            var alltypes = NineBase.Getcontext().ProductType.ToList();
            Type.ItemsSource = alltypes;
            Type.SelectedIndex = 0;
            

        }

        private void productadd_Click(object sender, RoutedEventArgs e)
        {
            _addproduct.ProductTypeID = Convert.ToInt32(Type.SelectedValue);
            StringBuilder erros = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_addproduct.Title))
                erros.AppendLine("Наименование товара не может быть пустым");
            if (_addproduct.ProductionPersonCount < 0)
                erros.AppendLine("Наименование товара не может быть пустым");
            if (_addproduct.MinCostForAgent < 0)
                erros.AppendLine("Минимальная ценна не может быть!");
            if (Type.SelectedIndex == -1)
                erros.AppendLine("Минимальная ценна не может быть!");
            if (string.IsNullOrWhiteSpace(_addproduct.ArticleNumber))
                erros.AppendLine("ArticleNumber не может быть!");
            if (_addproduct.ProductionWorkshopNumber == null)
                erros.AppendLine("ProductionWorkshopNumber не может быть!");

            if (NineBase.Getcontext().Product.Where(p => p.ArticleNumber == _addproduct.ArticleNumber).FirstOrDefault() != null)
                erros.AppendLine("Такой артикл уже существует");

            if (erros.Length > 0)
            {
                MessageBox.Show(erros.ToString(), "Ошибка ввода данных", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }

            if (_addproduct.ID == 0)
                NineBase.Getcontext().Product.Add(_addproduct);
            try
            {
                NineBase.Getcontext().SaveChanges();
                MessageBox.Show("Данные успешно добавленны");
                MainWindow win = new MainWindow();
                win.Show();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }


    }
}
