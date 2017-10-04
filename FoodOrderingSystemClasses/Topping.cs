using System;
using System.Globalization;

namespace FoodOrderingSystemClasses
{
    public class Topping : IComparable<Topping>
    {
        public enum Type { pizza, burger, sundry };
        private Type m_ToppingType;
        private string m_ToppingName;
        private decimal m_ToppingCost;
        private decimal m_ToppingQuantity;

        public Topping(string pToppingType, string pToppingName, decimal pToppingCost, decimal pToppingQuantity)
        {
            m_ToppingType = GetTypeFromString(pToppingType);
            m_ToppingName = pToppingName;
            m_ToppingCost = pToppingCost;
            m_ToppingQuantity = pToppingQuantity;
        }

        /// <summary>
        /// Compares 2 toppings together to put them in alphabetical order by name.
        /// </summary>
        /// <param name="pTopping">Topping to compare.</param>
        /// <returns></returns>
        public int CompareTo(Topping pTopping)
        {
            return string.Compare(m_ToppingName, pTopping.GetToppingName());
        }

        /// <summary>
        /// Overrides to string ot display catagory, name, cost and quantity.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToTitleCase(string.Format("Category: {0}, Name: {1}, Cost Per Unit: £{2}, Quantity: {3}", m_ToppingType, m_ToppingName, m_ToppingCost, m_ToppingQuantity));
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
        /// Gets the type of the topping item based on inputted type.
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
        /// Updates the toppings quantity.
        /// </summary>
        /// <param name="pQuantity">Inputted topping quantity.</param>
        public void UpdateToppingQuantity(decimal pQuantity)
        {
            if (pQuantity >= 0)
            {
                m_ToppingQuantity = pQuantity;
            }
            else
            {
                throw new Exception("Topping quantity can't be less than 0.");
            }
        }

        #region Accessors
        public string GetToppingType()
        {
            return m_ToppingType.ToString();
        }

        public string GetToppingName()
        {
            return m_ToppingName;
        }

        public decimal GetToppingCost()
        {
            return m_ToppingCost;
        }

        public decimal GetToppingQuantity()
        {
            return m_ToppingQuantity;
        }
        #endregion
    }
}
