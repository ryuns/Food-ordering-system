using FoodOrderingSystemClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ACW2
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        #region Generic Methods
        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="pStringInput">Inputted string to convert.</param>
        /// <returns></returns>
        private string ToTitleCase(string pStringInput)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pStringInput.ToLower());
        }

        /// <summary>
        /// Generates a food string based on the foods parameters.
        /// </summary>
        /// <param name="pFood">Inputted food item.</param>
        /// <returns></returns>
        private string GenerateFoodString(Food pFood)
        {
            string foodString = "";

            foodString += pFood.GetFoodType() + ", " + pFood.GetFoodName() + ", ";

            if (pFood.GetFoodType() == "pizza")
            {
                Pizza temp = pFood as Pizza;

                //Gets pizza size in the form of inches.
                switch (temp.GetFoodSize())
                {
                    case "regular":
                        foodString += "10\", ";
                        break;

                    case "large":
                        foodString += "12\", ";
                        break;

                    case "extralarge":
                        foodString += "16\", ";
                        break;
                }

                //Gets whether the pizza has stuffed crust.
                if (temp.GetIfStuffedCrust())
                {
                    foodString += "stuffed crust, ";
                }

                //Adds the pizzas extra toppings to the string.
                if (temp.GetExtraToppings().Count != 0)
                {
                    foodString += "extra: ";

                    foreach (Topping topping in temp.GetExtraToppings())
                    {
                        foodString += topping.GetToppingName() + ", ";
                    }
                }
            }
            else if (pFood.GetFoodType() == "burger")
            {
                Burger temp = pFood as Burger;

                //Gets burger size in form of quarter or half pound.
                switch (temp.GetFoodSize())
                {
                    case "regular":
                        foodString += "1/4lb, ";
                        break;

                    case "large":
                        foodString += "1/2lb, ";
                        break;
                }

                //Displays extra toppings on the order if applicable.
                if (temp.GetExtraToppings().Count != 0)
                {
                    foodString += "extra: ";

                    foreach (Topping topping in temp.GetExtraToppings())
                    {
                        foodString += topping.GetToppingName() + ", ";
                    }
                }
            }
            else if (pFood.GetFoodType() == "sundry")
            {
                foodString += pFood.GetFoodSize() + ", ";
            }

            foodString += "£ " + pFood.GetFoodPrice();

            return foodString;
        }
        #endregion

        private OrderManager m_OrderManager;
        private CustomerOrder m_CustomerOrder;
        private FoodManager m_FoodManager;
        private Pizza m_NewPizza;
        private Burger m_NewBurger;
        private Sundry m_NewSundry;

        public OrderWindow(FoodManager pFoodManager, OrderManager pOrderManager)
        {
            InitializeComponent();

            //Passes the food manager and order manager to this window.
            m_FoodManager = pFoodManager;
            m_OrderManager = pOrderManager;

            //Creates a new customer order.
            m_CustomerOrder = new CustomerOrder();

            UpdateFoodList();
        }

        /// <summary>
        /// Updates the food sections based on type and stock levels.
        /// </summary>
        private void UpdateFoodList()
        {
            //Clears the lists and combo boxed ready for items to be added.
            sundryList.Items.Clear();
            pizzaSelectionComboBox.Items.Clear();
            burgerSelectionComboBox.Items.Clear();

            //Creates a list to sort the sundries in alphabetical order.
            List<string> sortedSundries = new List<string>();

            for (int i = 0; i < m_FoodManager.GetAllFood().Count; i++)
            {
                //Runs the is in stock method to check if the food item is in stock at the begining of the window.
                m_FoodManager.GetAllFood()[i].IsInStock();

                //Makes name equal the food name.
                string name = ToTitleCase(m_FoodManager.GetAllFood()[i].GetFoodName());

                //Checks if the food is available, if it is then add the food items to the corresponding sections.
                if (m_CustomerOrder.IsOrderPossible(m_FoodManager.GetAllFood()[i].GetFoodToppings(), m_FoodManager.GetAllFood()[i].GetFoodToppingsQuantity()) && m_FoodManager.GetAllFood()[i].GetStockBool())
                {
                    switch (m_FoodManager.GetAllFood()[i].GetFoodType())
                    {
                        case "sundry":
                            sortedSundries.Add(name);
                            break;

                        case "pizza":
                            //Ensures only one instance of the food name is added to the combo box.
                            if (!pizzaSelectionComboBox.Items.Contains(name))
                            {
                                pizzaSelectionComboBox.Items.Add(name);
                            }
                            break;

                        case "burger":
                            //Ensures only one instance of the food name is added to the combo box.
                            if (!burgerSelectionComboBox.Items.Contains(name))
                            {
                                burgerSelectionComboBox.Items.Add(name);
                            }
                            break;
                    }
                }
            }

            //Sorts the sundry list.
            sortedSundries.Sort();

            //Adds the sundry names to the listbox.
            foreach (string sundryName in sortedSundries)
            {
                sundryList.Items.Add(sundryName);
            }

            //If the lists/combo boxes are empty add a string to say there are none in stock.
            if (sundryList.Items.Count == 0)
            {
                sundryList.Items.Add("Sorry, no sundries are in stock.");
                sundryPriceTextBox.Clear();
            }
            if (pizzaSelectionComboBox.Items.Count == 0)
            {
                pizzaSelectionComboBox.Items.Add("Sorry, no pizzas are in stock.");
                extraPizzaToppingList.Items.Clear();
                pizzaPriceText.Clear();
            }
            if (burgerSelectionComboBox.Items.Count == 0)
            {
                burgerSelectionComboBox.Items.Add("Sorry, no burgers are in stock.");
                burgerPriceTextBox.Clear();
            }

            //Resets all values to "default".
            sundryList.SelectedIndex = 0;
            pizzaSelectionComboBox.SelectedIndex = 0;
            pizzaSizeComboBox.SelectedIndex = 0;
            stuffedCrustCheckBox.IsChecked = false;
            burgerSelectionComboBox.SelectedIndex = 0;
            quarterPoundRadioButton.IsChecked = true;
            saladCheckBox.IsChecked = false;
            cheeseCheckBox.IsChecked = false;
        }

        /// <summary>
        /// Updates the list of current orders.
        /// Generates a string based on the food added ot the order.
        /// </summary>
        private void UpdateOrdersList()
        {
            //Clears the order summary list in prep for adding the orders.
            orderSummaryList.Items.Clear();

            //Generates a formatted string based on the ordered foods selections.
            foreach (Food food in m_CustomerOrder.GetAllFood())
            {
                orderSummaryList.Items.Add(ToTitleCase(GenerateFoodString(food)));
            }

            //Displays current orders prices.
            totalPriceTextBox.Text = "£ " + m_CustomerOrder.GetRevenue();
            grossProffitTextBox.Text = "£ " + m_CustomerOrder.GetGrossProfit();
            ingredientTextBox.Text = "£ " + m_CustomerOrder.GetTotalToppingCost();
        }

        private void removeItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (orderSummaryList.SelectedIndex >= 0)
            {
                //Removes the order item based on the selected summary list index.
                m_CustomerOrder.RemoveItem(orderSummaryList.SelectedIndex);
                UpdateOrdersList();
                UpdateFoodList();
            }
        }

        private void completeOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Adds the order to the order manager and updates the topping quantities.
                m_OrderManager.AddOrder(m_CustomerOrder);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Order could not be completed: " + exc.Message);
            }

            Close();
        }

        #region PizzaSection
        /// <summary>
        /// Updates the current pizza based on options selected.
        /// </summary>
        private void UpdatePizza()
        {
            if (pizzaSelectionComboBox.SelectedItem != null)
            {
                //If there are no pizzas disable all relevant UI elements.
                if (pizzaSelectionComboBox.SelectedValue.ToString().Equals("Sorry, no pizzas are in stock."))
                {
                    pizzaUiElements.IsEnabled = false;
                }
                if (pizzaSizeComboBox.SelectedItem != null)
                {
                    pizzaUiElements.IsEnabled = true;

                    //Gets the base current selected pizza from the menu.
                    Pizza selectedPizza = m_FoodManager.GetFoodByNameSize(pizzaSelectionComboBox.SelectedValue.ToString(), pizzaSizeComboBox.SelectedValue.ToString()) as Pizza;

                    if (selectedPizza != null)
                    {
                        //Checks if stuffed crust can be added to an order, if not then disable the UI element.
                        if (!m_CustomerOrder.IsOrderPossible(selectedPizza.GetDoughAndMozzarella(), selectedPizza.GetDoughAndMozzarellaStuffedQuantities()) || !selectedPizza.IsStuffedCrustPossible())
                        {
                            stuffedCrustCheckBox.IsEnabled = false;
                        }
                        else
                        {
                            stuffedCrustCheckBox.IsEnabled = true;
                        }

                        //If the extra toppings list selected items is 0 then reset the list to the updated toppings.
                        if (extraPizzaToppingList.SelectedItems.Count == 0)
                        {
                            extraPizzaToppingList.Items.Clear();

                            //Creates a sorted list of extra toppings in alphabetical order.
                            List<string> sortedToppings = new List<string>();

                            foreach (Topping topping in selectedPizza.GetPossibleExtraToppings())
                            {
                                //Checks to see if the extra topping is in stock.
                                if (m_CustomerOrder.IsOrderPossible(topping, pizzaSizeComboBox.SelectedValue.ToString().ToLower()) && selectedPizza.IsExtraToppingPossible(topping))
                                {
                                    sortedToppings.Add(ToTitleCase(topping.GetToppingName()));
                                }
                            }

                            //Sorts the toppings list.
                            sortedToppings.Sort();

                            if ((selectedPizza.GetFoodToppings().Count - 2) < 5)
                            {
                                //Adds the extra topping to the list.
                                foreach (string topping in sortedToppings)
                                {
                                    extraPizzaToppingList.Items.Add(topping);
                                }
                            }
                        }

                        //If the selected extra toppings amount is 0 then it resets the extra topping list.
                        //This is so as you're selecting extra topping the list isnt reset and progress is lost.
                        //If the list is expty it disables the list.
                        if (extraPizzaToppingList.Items.Count == 0)
                        {
                            //If default topping quantities are greater than or equal to 5
                            //say you cannot add extra toppings to this specific pizza.
                            if ((selectedPizza.GetFoodToppings().Count - 2) >= 5)
                            {
                                extraPizzaToppingList.Items.Add("Can not add extra toppings");
                                extraPizzaToppingList.Items.Add(string.Format("to a {0} pizza.", ToTitleCase(selectedPizza.GetFoodName())));
                            }
                            //Otherwise its because no toppings are in stock so say
                            //there are no toppings available.
                            else
                            {
                                extraPizzaToppingList.Items.Add("Sorry, there are no");
                                extraPizzaToppingList.Items.Add("extra toppings available.");
                            }
                            extraPizzaToppingList.IsEnabled = false;
                        }
                        else
                        {
                            extraPizzaToppingList.IsEnabled = true;
                        }

                        //If too many toppings are selected, deselect toppings and say how many can be selected on specific pizza.
                        if ((selectedPizza.GetFoodToppings().Count - 2) + extraPizzaToppingList.SelectedItems.Count > 5)
                        {
                            extraPizzaToppingList.SelectedItems.Clear();

                            //If more than 1 topping can be added to the pizza then say toppings, else say topping.
                            if ((5 - (selectedPizza.GetFoodToppings().Count - 2)) > 1)
                            {
                                MessageBox.Show(string.Format("Only {0} extra toppings can be selected on a {1} pizza.", (5 - (selectedPizza.GetFoodToppings().Count - 2)).ToString(), ToTitleCase(selectedPizza.GetFoodName())));
                            }
                            else
                            {
                                MessageBox.Show(string.Format("Only {0} extra topping can be selected on a {1} pizza.", (5 - (selectedPizza.GetFoodToppings().Count - 2)).ToString(), ToTitleCase(selectedPizza.GetFoodName())));
                            }
                        }

                        //Creates a list of extra toppings to be added to the pizza.
                        List<string> extraPizzaToppings = new List<string>();
                        foreach (string extraTopping in extraPizzaToppingList.SelectedItems)
                        {
                            extraPizzaToppings.Add(extraTopping);
                        }

                        //Creates a new pizza based on all selected options (stuffed crust and extra toppings).
                        m_NewPizza = new Pizza(selectedPizza.GetFoodType(), selectedPizza.GetFoodName(), selectedPizza.GetFoodToppings(), new List<decimal>(selectedPizza.GetFoodToppingsQuantity()), selectedPizza.GetFoodSize(), selectedPizza.GetFoodPrice(), selectedPizza.GetPossibleExtraToppings(), extraPizzaToppings, (bool)stuffedCrustCheckBox.IsChecked);

                        pizzaPriceText.Text = "£ " + m_NewPizza.GetFoodPrice();
                    }
                }
            }
        }

        private void pizzaSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clears the sizes.
            pizzaSizeComboBox.Items.Clear();

            if (pizzaSelectionComboBox.SelectedItem != null)
            {
                //Only adds the sizes if there are pizzas in stock. (for formatting)
                if (!pizzaSelectionComboBox.SelectedValue.ToString().Equals("Sorry, no pizzas are in stock."))
                {
                    //Adds only the pizzas that are in stocks sizes to the combo box.
                    foreach (Food food in m_FoodManager.GetFoodByName(pizzaSelectionComboBox.SelectedValue.ToString()))
                    {
                        if (m_CustomerOrder.IsOrderPossible(food.GetFoodToppings(), food.GetFoodToppingsQuantity()) && food.GetStockBool())
                        {
                            if (!food.GetFoodSize().ToLower().Equals("extralarge"))
                            {
                                pizzaSizeComboBox.Items.Add(ToTitleCase(food.GetFoodSize()));
                            }
                            //Changes extralarge to Extra-Large. (for formatting)
                            else
                            {
                                pizzaSizeComboBox.Items.Add("Extra-Large");
                            }
                        }
                    }
                }
            }
            extraPizzaToppingList.SelectedItems.Clear();
            pizzaSizeComboBox.SelectedIndex = 0;
            stuffedCrustCheckBox.IsChecked = false;


            UpdatePizza();
        }

        private void pizzaSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clears the selected extra toppings and stuffed crust.
            extraPizzaToppingList.SelectedItems.Clear();
            stuffedCrustCheckBox.IsChecked = false;
            UpdatePizza();
        }

        private void stuffedCrustCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePizza();
        }

        private void extraPizzaToppingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePizza();
        }

        private void addPizzaOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //If the created pizza is not null then add the pizza to the order.
                if (m_NewPizza != null)
                {
                    m_CustomerOrder.AddFood(m_NewPizza);
                }
                else
                {
                    throw new Exception("No pizza selected. ");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Pizza could not be added to the order: " + exc.Message.ToString());
            }

            //Updates the lists to reflect the new ingredient quantities, new order added.
            UpdateFoodList();
            UpdateOrdersList();
        }
        #endregion

        #region BurgerSection
        /// <summary>
        /// Updates the current burger based on options selected.
        /// </summary>
        private void UpdateBurger()
        {
            if (burgerSelectionComboBox.SelectedItem != null)
            {
                //If there are no burgers disable all relevant UI elements.
                if (burgerSelectionComboBox.SelectedValue.ToString().Equals("Sorry, no burgers are in stock."))
                {
                    burgerUiElements.IsEnabled = false;
                }
                else
                {
                    burgerUiElements.IsEnabled = true;

                    Burger selectedBurger = null;
                    //If quarter pound is selected return regular size burger,
                    //if half pound is selected return large size burger.
                    if (quarterPoundRadioButton.IsChecked == true)
                    {
                        selectedBurger = m_FoodManager.GetFoodByNameSize(burgerSelectionComboBox.SelectedValue.ToString(), "regular") as Burger;
                    }
                    else if (halfPoundRadioButton.IsChecked == true)
                    {
                        selectedBurger = m_FoodManager.GetFoodByNameSize(burgerSelectionComboBox.SelectedValue.ToString(), "large") as Burger;
                    }

                    if (selectedBurger != null)
                    {
                        //gets whether salad and/or cheddar are available.
                        if (m_CustomerOrder.IsOrderPossible(selectedBurger.GetPossibleExtraToppingByName("salad"), (decimal)0.5) && selectedBurger.IsSaladPossible())
                        {
                            saladCheckBox.IsEnabled = true;
                        }
                        else
                        {
                            saladCheckBox.IsEnabled = false;
                        }

                        if (m_CustomerOrder.IsOrderPossible(selectedBurger.GetPossibleExtraToppingByName("cheddar"), (decimal)0.2) && selectedBurger.IsCheesePossible())
                        {
                            cheeseCheckBox.IsEnabled = true;
                        }
                        else
                        {
                            cheeseCheckBox.IsEnabled = false;
                        }

                        //Creates a new burger based on all possible options (cheese and salad).
                        m_NewBurger = new Burger(selectedBurger.GetFoodType(), selectedBurger.GetFoodName(), selectedBurger.GetFoodToppings(), selectedBurger.GetFoodToppingsQuantity(), selectedBurger.GetFoodSize(), selectedBurger.GetFoodPrice(), selectedBurger.GetPossibleExtraToppings(), (bool)saladCheckBox.IsChecked, (bool)cheeseCheckBox.IsChecked);

                        burgerPriceTextBox.Text = "£ " + m_NewBurger.GetFoodPrice();
                    }
                }
            }
        }

        private void burgerSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (burgerSelectionComboBox.SelectedItem != null)
            {
                if (!burgerSelectionComboBox.SelectedValue.ToString().Equals("Sorry, no burgers are in stock."))
                {
                    //Only enables the large size if the toppings are available in sufficient quantities.
                    foreach (Food food in m_FoodManager.GetFoodByName(burgerSelectionComboBox.SelectedValue.ToString()))
                    {
                        if (food.GetFoodSize().ToLower().Equals("large"))
                        {
                            if (m_CustomerOrder.IsOrderPossible(food.GetFoodToppings(), food.GetFoodToppingsQuantity()) && food.GetStockBool())
                            {
                                halfPoundRadioButton.IsEnabled = true;
                            }
                            else
                            {
                                halfPoundRadioButton.IsEnabled = false;
                            }
                        }
                    }
                }
            }
            quarterPoundRadioButton.IsChecked = true;
            saladCheckBox.IsChecked = false;
            cheeseCheckBox.IsChecked = false;

            UpdateBurger();
        }

        private void burgerSizeRadioButtons_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBurger();             
        }

        private void burgerExtrasCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBurger();
        }

        private void addBurgerOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_NewBurger != null)
                {
                    m_CustomerOrder.AddFood(m_NewBurger);
                }
                else
                {
                    throw new Exception("No Burger selected. ");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Burger could not be added to the order: " + exc.Message.ToString());
            }

            UpdateFoodList();
            UpdateOrdersList();
        }
        #endregion

        #region SundrySection
        private void sundryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sundryList.Items.Count != 0)
            {
                //If there are no sundries disable all relevant UI elements.
                if (sundryList.SelectedValue.ToString().Equals("Sorry, no sundries are in stock."))
                {
                    sundryUiElements.IsEnabled = false;
                }
                else
                {
                    sundryUiElements.IsEnabled = true;

                    //Gets the sundry from the menu.
                    Sundry selectedSundry = m_FoodManager.GetFoodByNameSize(sundryList.SelectedItem.ToString(), "regular") as Sundry;

                    if (selectedSundry != null)
                    {
                        m_NewSundry = new Sundry(selectedSundry.GetFoodType(), selectedSundry.GetFoodName(), selectedSundry.GetFoodToppings(), selectedSundry.GetFoodToppingsQuantity(), selectedSundry.GetFoodSize(), selectedSundry.GetFoodPrice());
                    }

                    sundryPriceTextBox.Text = "£ " + m_NewSundry.GetFoodPrice();
                }
            }
        }

        private void addSundryOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_NewSundry != null)
                {
                    m_CustomerOrder.AddFood(m_NewSundry);
                }
                else
                {
                    throw new Exception("No sundry selected. ");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Sundry could not be added to the order: " + exc.Message.ToString());
            }

            UpdateFoodList();
            UpdateOrdersList();
        }
        #endregion
    }
}