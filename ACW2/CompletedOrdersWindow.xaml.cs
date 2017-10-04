using FoodOrderingSystemClasses;
using System.Windows;
using System.Windows.Controls;

namespace ACW2
{
    /// <summary>
    /// Interaction logic for CompletedOrdersWindow.xaml
    /// </summary>
    public partial class CompletedOrdersWindow : Window
    {
        public CompletedOrdersWindow(OrderManager pOrderManager)
        {
            InitializeComponent();

            if (pOrderManager != null)
            {
                //Displays all of the relevent information in the correct places in the window.
                totalNumberOfOrdersTextBox.Text = pOrderManager.GetTotalNumberOfOrders().ToString();
                pizzasSoldTextBox.Text = pOrderManager.GetPizzasSold().ToString();
                burgersSoldTextBox.Text = pOrderManager.GetBurgersSold().ToString();
                sundriesSoldTextBox.Text = pOrderManager.GetSundrysSold().ToString();
                revenueTextBox.Text = "£ " + pOrderManager.GetRevenue().ToString();
                ingredientCostTextBox.Text = "£ " + pOrderManager.GetTotalToppingCost().ToString();
                grossProfitTextBox.Text = "£ " + pOrderManager.GetGrossProfit().ToString();

                ingredientsUsedListBox.ItemsSource = pOrderManager.GetTotalToppingsUsed();
            }
        }

        private void ingredientsUsedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ingredientsUsedListBox.SelectedIndex = -1;
        }
    }
}