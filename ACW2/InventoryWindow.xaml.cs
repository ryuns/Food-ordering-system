using FoodOrderingSystemClasses;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ACW2
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    ///

    public partial class InventoryWindow : Window
    {
        /// <summary>
        /// Verifies that a string is a valid decimal value.
        /// </summary>
        /// <param name="pTextVal">The string to be parsed.</param>
        /// <returns>The decimal value of the parsed string.</returns>
        private decimal GetDecFromText(string pTextVal)
        {
            decimal pAmount;
            string textAmount = pTextVal;
            bool textAmountTest = decimal.TryParse(textAmount, out pAmount);

            if(pTextVal.Contains(","))
            {
                textAmountTest = false;
            }
            if (!textAmountTest)
            {
                throw new Exception(string.Format("The amount must be a numberic value ({0}). ", pTextVal));
            }

            return pAmount;
        }

        private FoodManager m_FoodManager;
        private Topping m_SelectedTopping;

        public InventoryWindow(FoodManager pFoodManager)
        {
            InitializeComponent();

            //Passes the foodmanager to this window.
            m_FoodManager = pFoodManager;

            categoryComboBox.Items.Add("All");
            categoryComboBox.Items.Add("Pizza");
            categoryComboBox.Items.Add("Burger");
            categoryComboBox.Items.Add("Sundry");
            categoryComboBox.SelectedValue = "All";
        }

        /// <summary>
        /// Adds the toppings to the list box based on category selected.
        /// </summary>
        private void CreateToppingListBox()
        {
            categoryList.Items.Clear();
            //goes through all toppings in the topping type list.
            foreach (Topping topping in m_FoodManager.GetToppingByType(categoryComboBox.SelectedItem.ToString()))
            {
                //Creates a new list box item with the content of the topping.
                ListBoxItem item = new ListBoxItem();
                item.Content = topping;

                //Changes the background colours of the list box item accordingly.
                if (topping.GetToppingQuantity() == 0)
                {
                    item.Background = Brushes.PaleVioletRed;
                }
                else if (topping.GetToppingQuantity() > 0 && topping.GetToppingQuantity() < 100)
                {
                    item.Background = Brushes.Yellow;
                }

                //Adds the item to the list box.
                categoryList.Items.Add(item);
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Creates the toppings list box.
            CreateToppingListBox();
        }

        private void categoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = categoryList.SelectedItem as ListBoxItem;
            if (item != null)
            {
                //Gets the selected topping based on the selected item in the list box.
                m_SelectedTopping = item.Content as Topping;
                if (m_SelectedTopping != null)
                {
                    //Displays the toppings quantity in the text box.
                    stockLevelTextBox.Text = m_SelectedTopping.GetToppingQuantity().ToString();
                }
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //If no topping is selected throws an exception.
                if (m_SelectedTopping == null)
                {
                    throw new Exception("No topping selected.");
                }

                //Updates topping quantity according to the inputted text.
                m_SelectedTopping.UpdateToppingQuantity(GetDecFromText(stockLevelTextBox.Text));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Topping quantity could not be updated: " + exc.Message.ToString());
            }

            //Re creates the list box of toppings.
            CreateToppingListBox();
        }
    }
}