using System.Collections.Generic;
using System.Globalization;

namespace FoodOrderingSystemClasses
{
    public class OrderManager
    {
        private List<CustomerOrder> m_CustomerOrders = new List<CustomerOrder>();

        public void AddOrder(CustomerOrder pCustomerOrder)
        {
            m_CustomerOrders.Add(pCustomerOrder);
            UpdateInventory(pCustomerOrder);
        }

        /// <summary>
        /// Updates the inventory when an order is added to the order manager.
        /// </summary>
        /// <param name="pCustomerOrder">Inputted customer order.</param>
        private void UpdateInventory(CustomerOrder pCustomerOrder)
        {
            foreach (Food food in pCustomerOrder.GetAllFood())
            {
                food.UpdateToppingQuantities();
            }
        }

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="pStringInput">Inputted string to convert.</param>
        /// <returns></returns>
        private string ToTitleCase(string pStringInput)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pStringInput.ToLower());
        }

        #region Accessors
        /// <summary>
        /// Gets all the toppings used in customer orders.
        /// </summary>
        public List<string> GetTotalToppingsUsed()
        {
            List<Topping> toppingList = new List<Topping>();
            List<decimal> toppingQuantityList = new List<decimal>();
            List<string> sortedTotalToppingsList = new List<string>();

            for (int i = 0; i < m_CustomerOrders.Count; i++)
            {
                for (int j = 0; j < m_CustomerOrders[i].GetToppingsUsed().Count; j++)
                {
                    //If the toppings used list contains the topping already then just add the quantity.
                    if (toppingList.Contains(m_CustomerOrders[i].GetToppingsUsed()[j]))
                    {
                        //Gets the index of the topping in the used topping list then uses that index to add the quantity.
                        toppingQuantityList[toppingList.IndexOf(m_CustomerOrders[i].GetToppingsUsed()[j])] += m_CustomerOrders[i].GetToppingsQuantities()[j];
                    }
                    //Otherwise add the topping and quantity.
                    else
                    {
                        toppingList.Add(m_CustomerOrders[i].GetToppingsUsed()[j]);
                        toppingQuantityList.Add(m_CustomerOrders[i].GetToppingsQuantities()[j]);
                    }
                }
            }

            for (int i = 0; i < toppingList.Count; i++)
            {
                sortedTotalToppingsList.Add(ToTitleCase(string.Format("Topping name: {0}, Topping quantity used: {1}", toppingList[i].GetToppingName(), toppingQuantityList[i])));
            }
            sortedTotalToppingsList.Sort();

            return sortedTotalToppingsList;
        }

        public decimal GetRevenue()
        {
            decimal revenue = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                revenue += order.GetRevenue();
            }

            return revenue;
        }

        public decimal GetGrossProfit()
        {
            decimal grossProfit = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                grossProfit += order.GetGrossProfit();
            }

            return grossProfit;
        }

        public decimal GetTotalToppingCost()
        {
            decimal totalToppingCost = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                totalToppingCost += order.GetTotalToppingCost();
            }

            return totalToppingCost;
        }

        public int GetTotalNumberOfOrders()
        {
            return m_CustomerOrders.Count;
        }

        public int GetPizzasSold()
        {
            int pizzasSold = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                pizzasSold += order.GetPizzasSold();
            }

            return pizzasSold;
        }

        public int GetBurgersSold()
        {
            int burgersSold = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                burgersSold += order.GetBurgersSold();
            }

            return burgersSold;
        }

        public int GetSundrysSold()
        {
            int sundriesSold = 0;

            foreach (CustomerOrder order in m_CustomerOrders)
            {
                sundriesSold += order.GetSundrysSold();
            }

            return sundriesSold;
        }
        #endregion
    }
}
