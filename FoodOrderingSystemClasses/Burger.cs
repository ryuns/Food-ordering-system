using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrderingSystemClasses
{
    public class Burger : Food
    {
        private List<Topping> m_PossibleExtraToppings = new List<Topping>();
        private List<Topping> m_ExtraToppings = new List<Topping>();
        private List<decimal> m_ExtraToppingsQuantity = new List<decimal>();

        public Burger(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQuantity, string pSize, decimal pPrice, List<Topping> pExtraToppings) : base(pType, pName, pTopping, pToppingQuantity, pSize, pPrice)
        {
            m_PossibleExtraToppings = pExtraToppings;
            CalculateTotalToppingCost();
            CalculateGrossProfit();
            IsInStock();
        }

        public Burger(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQuantity, string pSize, decimal pPrice, List<Topping> pExtraToppings, bool pIsSaladChecked, bool pIsCheeseChecked) : base(pType, pName, pTopping, pToppingQuantity, pSize, pPrice)
        {
            m_PossibleExtraToppings = pExtraToppings;
            HasSaladOrCheese(pIsSaladChecked, pIsCheeseChecked);
            CalculateTotalToppingCost();
            CalculateGrossProfit();
            IsInStock();
        }

        /// <summary>
        /// Calculates the total cost of all toppings used in a food item.
        /// </summary>
        public override void CalculateTotalToppingCost()
        {
            base.CalculateTotalToppingCost();
            for (int i = 0; i < m_ExtraToppings.Count; i++)
            {
                m_TotalToppingCost += decimal.Round(m_ExtraToppings[i].GetToppingCost() * m_ExtraToppingsQuantity[i], 2);
            }
        }

        /// <summary>
        /// Updates the quantities of toppings based on the quantity the food item uses.
        /// </summary>
        public override void UpdateToppingQuantities()
        {
            base.UpdateToppingQuantities();
            for (int i = 0; i < m_ExtraToppings.Count; i++)
            {
                m_ExtraToppings[i].UpdateToppingQuantity(m_ExtraToppings[i].GetToppingQuantity() - m_ExtraToppingsQuantity[i]);
            }
        }

        /// <summary>
        /// calculates wether the food item is in stock or not.
        /// </summary>
        public override void IsInStock()
        {
            base.IsInStock();
            for (int i = 0; i < m_ExtraToppings.Count; i++)
            {
                if (m_ExtraToppings[i].GetToppingQuantity() - m_ExtraToppingsQuantity[i] >= 0)
                {
                    m_IsInStock = true;
                }
                else
                {
                    m_IsInStock = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Checks whether cheese can be added to a burger.
        /// </summary>
        /// <returns></returns>
        public bool IsCheesePossible()
        {
            Topping topping = GetPossibleExtraToppingByName("cheddar");
            if (topping != null)
            {
                if (topping.GetToppingQuantity() - (decimal)0.2 < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether salad can be added to a burger.
        /// </summary>
        /// <returns></returns>
        public bool IsSaladPossible()
        {
            Topping topping = GetPossibleExtraToppingByName("salad");
            if (topping != null)
            {
                if (topping.GetToppingQuantity() - (decimal)0.5 < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates wether the burger has cheese and/or salad
        /// </summary>
        /// <param name="pIsSaladChecked">Inputted bool value for salad selection.</param>
        /// <param name="pIsCheeseChecked">Inputted bool value for cheese selection.</param>
        public void HasSaladOrCheese(bool pIsSaladChecked, bool pIsCheeseChecked)
        {
            string error = "";
            if (pIsSaladChecked)
            {
                if (IsSaladPossible())
                {
                    //If salad can be added then toppings are added appropriately.
                    m_ExtraToppings.Add(GetPossibleExtraToppingByName("salad"));
                    m_ExtraToppingsQuantity.Add((decimal)0.5);
                    m_Price += (decimal)0.4;
                }
                else
                {
                    error += "Not enough salad. ";
                }
            }
            if (pIsCheeseChecked)
            {
                if (IsCheesePossible())
                {
                    //If cheese can be added then toppings are added appropriately.
                    m_ExtraToppings.Add(GetPossibleExtraToppingByName("cheddar"));
                    m_ExtraToppingsQuantity.Add((decimal)0.2);
                    m_Price += (decimal)0.5;

                }
                else
                {
                    error += "not enough cheese. ";
                }
            }
            if (error != "")
            {
                throw new Exception(error);
            }
        }

        #region Accessors
        public Topping GetPossibleExtraToppingByName(string pToppingName)
        {
            foreach (Topping topping in m_PossibleExtraToppings)
            {
                if (topping.GetToppingName().ToLower().Equals(pToppingName.ToLower()))
                {
                    return topping;
                }
            }
            return null;
        }

        public List<Topping> GetPossibleExtraToppings()
        {
            return m_PossibleExtraToppings;
        }

        public List<Topping> GetExtraToppings()
        {
            return m_ExtraToppings;
        }

        public List<decimal> GetExtraToppingsQuantity()
        {
            return m_ExtraToppingsQuantity;
        }

        public override List<Topping> GetFoodToppings()
        {
            if (m_ExtraToppings.Count == 0)
            {
                return base.GetFoodToppings();
            }
            else
            {
                return base.GetFoodToppings().Concat(m_ExtraToppings).ToList();
            }
        }

        public override List<decimal> GetFoodToppingsQuantity()
        {
            if (m_ExtraToppingsQuantity.Count == 0)
            {
                return base.GetFoodToppingsQuantity();
            }
            else
            {
                return base.GetFoodToppingsQuantity().Concat(m_ExtraToppingsQuantity).ToList();
            }
        }
        #endregion
    }
}