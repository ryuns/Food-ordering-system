using System.Collections.Generic;

namespace FoodOrderingSystemClasses
{
    public class CustomerOrder
    {
        private List<Food> m_FoodOrders = new List<Food>();
        private List<Topping> m_ToppingsUsed = new List<Topping>();
        private List<decimal> m_QuantityOfToppingsUsed = new List<decimal>();
        private int m_PizzaOrders;
        private int m_BurgerOrders;
        private int m_SundryOrders;
        private decimal m_TotalToppingCost;
        private decimal m_GrossProfit;
        private decimal m_Revenue;

        public void AddFood(Food pFood)
        {
            m_FoodOrders.Add(pFood);
            CalculateTotalToppingsCost();
            CalculateFoodTypes();
            CalculateFoodToppingsUsed();
            CalculateTotalRevenue();
            m_GrossProfit = m_Revenue - m_TotalToppingCost;
        }

        public void RemoveItem(int pItemToRemove)
        {
            m_FoodOrders.RemoveAt(pItemToRemove);
            CalculateTotalToppingsCost();
            CalculateFoodTypes();
            CalculateFoodToppingsUsed();
            CalculateTotalRevenue();
            m_GrossProfit = m_Revenue - m_TotalToppingCost;
        }

        /// <summary>
        /// Calculates whether the order is possible without physically updating the topping quantity.
        /// </summary>
        /// <param name="pFoodToppings">Inputted list of food toppings.</param>
        /// <param name="pFoodToppingQuantities">Inputted list of food topping quantities.</param>
        /// <returns></returns>
        public bool IsOrderPossible(List<Topping> pFoodToppings, List<decimal> pFoodToppingQuantities)
        {
            for (int i = 0; i < m_ToppingsUsed.Count; i++)
            {
                for (int j = 0; j < pFoodToppings.Count; j++)
                {
                    if (m_ToppingsUsed[i] == pFoodToppings[j])
                    {
                        if ((m_ToppingsUsed[i].GetToppingQuantity() - m_QuantityOfToppingsUsed[i]) - pFoodToppingQuantities[j] < 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates whether the order is possible without physically updating the topping quantity.
        /// </summary>
        /// <param name="pFoodTopping">Inputted food topping.</param>
        /// <param name="pFoodSize">Inputted food size.</param>
        /// <returns></returns>
        public bool IsOrderPossible(Topping pFoodTopping, string pFoodSize)
        {
            for (int i = 0; i < m_ToppingsUsed.Count; i++)
            {
                if (m_ToppingsUsed[i] == pFoodTopping)
                {
                    switch (pFoodSize.ToLower())
                    {
                        case "regular":
                            if ((m_ToppingsUsed[i].GetToppingQuantity() - m_QuantityOfToppingsUsed[i]) - (decimal)0.25 < 0)
                            {
                                return false;
                            }
                            break;

                        case "large":
                            if ((m_ToppingsUsed[i].GetToppingQuantity() - m_QuantityOfToppingsUsed[i]) - (decimal)0.35 < 0)
                            {
                                return false;
                            }
                            break;

                        case "extra-large":
                            if ((m_ToppingsUsed[i].GetToppingQuantity() - m_QuantityOfToppingsUsed[i]) - (decimal)0.75 < 0)
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates whether the order is possible without physically updating the topping quantity.
        /// </summary>
        /// <param name="pFoodTopping">Inputted food topping.</param>
        /// <param name="pFoodToppingQuantity">Inputted food topping quantity.</param>
        /// <returns></returns>
        public bool IsOrderPossible(Topping pFoodTopping, decimal pFoodToppingQuantity)
        {
            for (int i = 0; i < m_ToppingsUsed.Count; i++)
            {
                if (m_ToppingsUsed[i] == pFoodTopping)
                {
                    if ((m_ToppingsUsed[i].GetToppingQuantity() - m_QuantityOfToppingsUsed[i]) - pFoodToppingQuantity < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates the used toppings.
        /// </summary>
        private void CalculateFoodToppingsUsed()
        {
            m_ToppingsUsed = new List<Topping>();
            m_QuantityOfToppingsUsed = new List<decimal>();

            foreach (Food food in m_FoodOrders)
            {
                for (int i = 0; i < food.GetFoodToppings().Count; i++)
                {
                    //If the topping exists then only add the topping quantity.
                    if (m_ToppingsUsed.Contains(food.GetFoodToppings()[i]))
                    {
                        //Gets the index of the used topping and adds the quantity to the corresponding topping quantity.
                        m_QuantityOfToppingsUsed[m_ToppingsUsed.IndexOf(food.GetFoodToppings()[i])] += food.GetFoodToppingsQuantity()[i];
                    }
                    //If the topping doesnt exist in the toppings used list then
                    //add the topping to the list and its corresponding quantity.
                    else
                    {
                        m_ToppingsUsed.Add(food.GetFoodToppings()[i]);
                        m_QuantityOfToppingsUsed.Add(food.GetFoodToppingsQuantity()[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the number of orders for each food type.
        /// </summary>
        private void CalculateFoodTypes()
        {
            m_PizzaOrders = 0;
            m_BurgerOrders = 0;
            m_SundryOrders = 0;

            foreach (Food food in m_FoodOrders)
            {
                switch (food.GetFoodType().ToString())
                {
                    case "pizza":
                        m_PizzaOrders++;
                        break;

                    case "burger":
                        m_BurgerOrders++;
                        break;

                    case "sundry":
                        m_SundryOrders++;
                        break;
                }
            }
        }

        /// <summary>
        /// Calculates the total revenue.
        /// </summary>
        private void CalculateTotalRevenue()
        {
            m_Revenue = 0;
            foreach (Food food in m_FoodOrders)
            {
                m_Revenue += food.GetFoodPrice();
            }
        }

        /// <summary>
        /// Calculates the total toppings cost.
        /// </summary>
        private void CalculateTotalToppingsCost()
        {
            m_TotalToppingCost = 0;
            for (int i = 0; i < m_FoodOrders.Count; i++)
            {
                m_TotalToppingCost += m_FoodOrders[i].GetToppingCost();
            }
        }

        #region Accessors
        public List<Food> GetAllFood()
        {
            return m_FoodOrders;
        }

        public List<Topping> GetToppingsUsed()
        {
            return m_ToppingsUsed;
        }

        public List<decimal> GetToppingsQuantities()
        {
            return m_QuantityOfToppingsUsed;
        }

        public decimal GetRevenue()
        {
            return m_Revenue;
        }

        public decimal GetGrossProfit()
        {
            return m_GrossProfit;
        }

        public decimal GetTotalToppingCost()
        {
            return m_TotalToppingCost;
        }

        public int GetPizzasSold()
        {
            return m_PizzaOrders;
        }

        public int GetBurgersSold()
        {
            return m_BurgerOrders;
        }

        public int GetSundrysSold()
        {
            return m_SundryOrders;
        }
        #endregion
    }
}
