using FoodOrderingSystemClasses;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ACW2
{
    /// <summary>
    /// Interaction logic for FoodMenuWindow.xaml
    /// </summary>
    public partial class FoodMenuWindow : Window
    {
        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="pStringInput">Inputted string to convert.</param>
        /// <returns></returns>
        private string ToTitleCase(string pStringInput)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pStringInput.ToLower());
        }

        private FoodManager m_FoodManager;
        private Food m_SelectedFood;

        public FoodMenuWindow(FoodManager pFoodManager)
        {
            InitializeComponent();

            //Passes the menu and inventory to this window.
            m_FoodManager = pFoodManager;

            categoryComboBox.Items.Add("All");
            categoryComboBox.Items.Add("Pizza");
            categoryComboBox.Items.Add("Burger");
            categoryComboBox.Items.Add("Sundry");
            categoryComboBox.SelectedValue = "All";
            sizeComboBox.Items.Add("All");
            sizeComboBox.Items.Add("Regular");
            sizeComboBox.Items.Add("Large");
            sizeComboBox.Items.Add("Extra-Large");
            sizeComboBox.SelectedValue = "All";
        }

        private void UpdateFoodList()
        {
            foodList.Items.Clear();

            if (categoryComboBox.SelectedItem != null && sizeComboBox.SelectedItem != null)
            {
                try
                {
                    //Adds the food items to the food list based on selected type and size.
                    foreach (Food food in m_FoodManager.GetFoodByTypeSize(categoryComboBox.SelectedItem.ToString(), sizeComboBox.SelectedItem.ToString()))
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = food;

                        for (int i = 0; i < food.GetFoodToppings().Count; i++)
                        {
                            //Checks if there is enough topping in stock to create the food, if not then colour in red.
                            if ((food.GetFoodToppings()[i].GetToppingQuantity() - food.GetFoodToppingsQuantity()[i]) < 0)
                            {
                                item.Background = Brushes.PaleVioletRed;
                                break;
                            }
                            else if (food.GetFoodToppings()[i].GetToppingQuantity() > 0 && food.GetFoodToppings()[i].GetToppingQuantity() < 100)
                            {
                                item.Background = Brushes.Yellow;
                            }
                        }

                        foodList.Items.Add(item);
                    }
                    foodList.SelectedIndex = 0;
                }
                catch (Exception exc)
                {
                    categoryComboBox.SelectedValue = "All";
                    sizeComboBox.SelectedValue = "All";
                    foodList.SelectedIndex = 0;
                    MessageBox.Show(exc.Message.ToString());
                }
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFoodList();
        }

        private void sizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFoodList();
        }

        private void foodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ingredientsList.Items.Clear();
            ListBoxItem food = foodList.SelectedItem as ListBoxItem;

            if (food != null)
            {
                m_SelectedFood = food.Content as Food;

                if (m_SelectedFood != null)
                {
                    for (int i = 0; i < m_SelectedFood.GetFoodToppings().Count; i++)
                    {
                        ListBoxItem item = new ListBoxItem();

                        //Adds the toppings name, quantity and price to the list.
                        item.Content = ToTitleCase(string.Format("Topping name: {0}, Topping quantity: {1}, Topping Cost: £{2}", m_SelectedFood.GetFoodToppings()[i].GetToppingName(), m_SelectedFood.GetFoodToppingsQuantity()[i], decimal.Round(m_SelectedFood.GetFoodToppings()[i].GetToppingCost() * m_SelectedFood.GetFoodToppingsQuantity()[i], 2)));

                        //Changes the colours according to topping stock.
                        if (m_FoodManager.GetToppingByName(m_SelectedFood.GetFoodToppings()[i].GetToppingName()).GetToppingQuantity() == 0)
                        {
                            item.Background = Brushes.PaleVioletRed;
                        }
                        else if (m_FoodManager.GetToppingByName(m_SelectedFood.GetFoodToppings()[i].GetToppingName()).GetToppingQuantity() > 0 && m_FoodManager.GetToppingByName(m_SelectedFood.GetFoodToppings()[i].GetToppingName()).GetToppingQuantity() < 100)
                        {
                            item.Background = Brushes.Yellow;
                        }

                        ingredientsList.Items.Add(item);
                    }

                    //Inputs the required information into the correct text boxes.
                    itemPriceText.Text = "£ " + m_SelectedFood.GetFoodPrice();
                    ingredientCostText.Text = "£ " + m_SelectedFood.GetToppingCost();
                    grossProfitText.Text = "£ " + m_SelectedFood.GetGrossProfit();
                }
            }
        }
    }
}