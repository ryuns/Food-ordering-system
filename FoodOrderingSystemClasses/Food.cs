using System;
using System.Collections.Generic;
using System.Globalization;

namespace FoodOrderingSystemClasses
{
    public abstract class Food : IComparable<Food>
    {
        public enum Type { pizza, burger, sundry };
        protected Type m_Type;
        protected string m_Name;
        protected List<Topping> m_Toppings = new List<Topping>();
        protected List<decimal> m_ToppingQuantity = new List<decimal>();
        protected decimal m_TotalToppingCost;
        public enum Size { regular, large, extralarge };
        protected Size m_Size;
        protected decimal m_Price;
        protected decimal m_GrossProfit;
        protected bool m_IsInStock;

        public Food(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQuantity, string pSize, decimal pPrice)
        {
            m_Type = GetTypeFromString(pType);
            m_Name = pName;
            m_Toppings = pTopping;
            m_ToppingQuantity = pToppingQuantity;
            m_Size = GetSizeFromString(pSize);
            m_Price = pPrice;
        }

        /// <summary>
        /// Compares 2 food items together to put them in alphabetical order by name then reverse alphabetical order for size.
        /// </summary>
        /// <param name="pFood">The food item to compare to.</param>
        /// <returns></returns>
        public int CompareTo(Food pFood)
        {
            if (m_Name.Equals(pFood.GetFoodName()))
            {
                if (string.Compare(m_Size.ToString(), pFood.GetFoodSize()) < 0)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return string.Compare(m_Name, pFood.GetFoodName());
        }

        /// <summary>
        /// Overrides to string ot display catagory, name, size and price.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToTitleCase(string.Format("Catagory: {0}, Name: {1}, Size: {2}, Price: £{3}", m_Type, m_Name, m_Size, m_Price));
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

        /// <summary>
        /// Gets the type of the food item based on inputted type.
        /// </summary>
        /// <param name="pType">Type as a string.</param>
        /// <returns></returns>
        private Type GetTypeFromString(string pType)
        {
            switch (pType.ToLower())
            {
                case "pizza":
                    return Type.pizza;

                case "burger":
                    return Type.burger;

                case "sundry":
                    return Type.sundry;

                default:
                    throw new Exception(string.Format("The type was not valid {0}. ", pType));
            }
        }

        /// <summary>
        /// Gets the size of the food item based on inputted size.
        /// </summary>
        /// <param name="pSize">Size as a string.</param>
        /// <returns>Size as an enum.</returns>
        private Size GetSizeFromString(string pSize)
        {
            switch (pSize.ToLower())
            {
                case "regular":
                    return Size.regular;

                case "large":
                    return Size.large;

                case "extralarge":
                    return Size.extralarge;

                default:
                    throw new Exception(string.Format("The size was not valid {0}. ", pSize));
            }
        }

        /// <summary>
        /// Calculates wether the food item is in stock or not.
        /// </summary>
        public virtual void IsInStock()
        {
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                if (m_Toppings[i].GetToppingQuantity() - m_ToppingQuantity[i] >= 0)
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
        /// Calculates the gross profit.
        /// </summary>
        public void CalculateGrossProfit()
        {
            m_GrossProfit = m_Price - m_TotalToppingCost;
        }

        /// <summary>
        /// Calculates the total cost of all toppings used in a food item.
        /// </summary>
        public virtual void CalculateTotalToppingCost()
        {
            m_TotalToppingCost = 0;
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                m_TotalToppingCost += Math.Round(m_Toppings[i].GetToppingCost() * m_ToppingQuantity[i], 2);
            }
        }

        /// <summary>
        /// Updates the quantities of toppings based on the quantity the food item uses.
        /// </summary>
        public virtual void UpdateToppingQuantities()
        {
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                m_Toppings[i].UpdateToppingQuantity(m_Toppings[i].GetToppingQuantity() - m_ToppingQuantity[i]);
            }
        }

        #region Accessors
        public string GetFoodType()
        {
            return m_Type.ToString();
        }

        public string GetFoodName()
        {
            return m_Name;
        }

        public virtual List<Topping> GetFoodToppings()
        {
            return m_Toppings;
        }

        public virtual List<decimal> GetFoodToppingsQuantity()
        {
            return m_ToppingQuantity;
        }

        public string GetFoodSize()
        {
            return m_Size.ToString();
        }

        public decimal GetFoodPrice()
        {
            return m_Price;
        }

        public decimal GetGrossProfit()
        {
            return m_GrossProfit;
        }

        public decimal GetToppingCost()
        {
            return m_TotalToppingCost;
        }

        public bool GetStockBool()
        {
            return m_IsInStock;
        }
        #endregion
    }
}
