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

namespace CompanyNine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateTub();
            var filtr = NineBase.Getcontext().ProductType.ToList();
            filtr.Insert(0, new ProductType
            {
                Title = "Все типы"
            });
            Filtr.ItemsSource = filtr;
            Filtr.SelectedIndex = 0;
            SortComb.SelectedIndex = 0;

        }
        private void UpdateTub()
        {
            NineBase.Getcontext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            ListProduct.ItemsSource = NineBase.Getcontext().Product.ToList();
        }
        private void Search()
        {
            var Result = NineBase.Getcontext().Product.ToList();
            if (Filtr.SelectedIndex > 0)
                Result = Result.Where(p => Convert.ToString(p.ProductTypeID).Contains(Convert.ToString(Filtr.SelectedValue))).ToList();
            if (SortComb.SelectedIndex == 1)
                Result = Result.OrderBy(p => p.Title).ToList();
            if (SortComb.SelectedIndex == 2)
                Result = Result.OrderByDescending(p => p.Title).ToList();
            if (SortComb.SelectedIndex == 3)
                Result = Result.OrderBy(p => p.ProductionWorkshopNumber).ToList();
            if (SortComb.SelectedIndex == 4)
                Result = Result.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
            if (SortComb.SelectedIndex == 5)
                Result = Result.OrderBy(p => p.MinCostForAgent).ToList();
            if (SortComb.SelectedIndex == 6)
                Result = Result.OrderByDescending(p => p.MinCostForAgent).ToList();

            Result = Result.Where(p => p.Title.ToLower().Contains(SearchInput.Text.ToLower()) || p.Description.ToLower().Contains(SearchInput.Text.ToLower())).ToList();


            ListProduct.ItemsSource = Result.ToList();

        }

        private void SortComb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void Filtr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void ListProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void add_new_Click(object sender, RoutedEventArgs e)
        {
            AddNewProduct WIN = new AddNewProduct();
            WIN.Show();
            this.Close();
        }

        private void delet_Click(object sender, RoutedEventArgs e)
        {
            var remove = ListProduct.SelectedItems.Cast<Product>().ToList();
            if (MessageBox.Show($"вы действительно хотите удалить {remove.Count()}", "подверждени", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                NineBase.Getcontext().Product.RemoveRange(remove);
            try
            {
                NineBase.Getcontext().SaveChanges();
                MessageBox.Show("Данные удалены");
                UpdateTub();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ListProduct_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
        }
    }
}
