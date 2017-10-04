using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrderingSystemClasses
{
    public class Pizza : Food
    {
        private List<Topping> m_PossibleExtraToppings = new List<Topping>();
        private List<Topping> m_ExtraToppings = new List<Topping>();
        private List<decimal> m_ExtraToppingsQuantity = new List<decimal>();
        private bool m_HasStuffedCrust;

        public Pizza(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQty, string pSize, decimal pPrice, List<Topping> pExtraToppings) : base(pType, pName, pTopping, pToppingQty, pSize, pPrice)
        {
            m_PossibleExtraToppings = pExtraToppings;
            CalculateTotalToppingCost();
            CalculateGrossProfit();
            IsInStock();
        }

        public Pizza(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQty, string pSize, decimal pPrice, List<Topping> pExtraToppings, List<string> pSelectedExtraToppings, bool pIsStuffedCrustChecked) : base(pType, pName, pTopping, pToppingQty, pSize, pPrice)
        {
            m_PossibleExtraToppings = pExtraToppings;
            HasStuffedCrust(pIsStuffedCrustChecked);
            CalculateExtraToppings(pSelectedExtraToppings);
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
        /// Calculates wether the food item is in stock or not.
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
        /// Checks if an inputted extra topping is available.
        /// </summary>
        /// <param name="pTopping">Inputted extra topping.</param>
        /// <returns></returns>
        public bool IsExtraToppingPossible(Topping pTopping)
        {
            switch (m_Size.ToString().ToLower())
            {
                //Checks to see if there is enough quantity based on the size of the pizza.
                case "regular":
                    if ((pTopping.GetToppingQuantity() - (decimal)0.25) < 0)
                    {
                        return false;
                    }
                    break;

                case "large":
                    if ((pTopping.GetToppingQuantity() - (decimal)0.35) < 0)
                    {
                        return false;
                    }
                    break;

                case "extralarge":
                    if ((pTopping.GetToppingQuantity() - (decimal)0.75) < 0)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Gets the selected extra toppings.
        /// </summary>
        /// <param name="pSelectedExtraToppings">Inputted list of extra toppings.</param>
        public void CalculateExtraToppings(List<string> pSelectedExtraToppings)
        {
            if (pSelectedExtraToppings != null)
            {
                //Checks to see if too many toppings are selected.
                if (((m_Toppings.Count - 2) + pSelectedExtraToppings.Count) > 5)
                {
                    throw new Exception("Toppings quantity can not be greater than 5. ");
                }
                else
                {
                    foreach (string selectedTopping in pSelectedExtraToppings)
                    {
                        foreach (Topping topping in m_PossibleExtraToppings)
                        {
                            if (topping.GetToppingName().ToLower().Equals(selectedTopping.ToLower()))
                            {
                                //Adds the correct quantities used based on the size of the pizza
                                //and the stock quantity.
                                if (IsExtraToppingPossible(topping) && m_Size.ToString().ToLower().Equals("regular"))
                                {
                                    m_ExtraToppings.Add(topping);
                                    m_ExtraToppingsQuantity.Add((decimal)0.25);
                                    m_Price += (decimal)0.60;
                                }
                                else if (IsExtraToppingPossible(topping) && m_Size.ToString().ToLower().Equals("large"))
                                {
                                    m_ExtraToppings.Add(topping);
                                    m_ExtraToppingsQuantity.Add((decimal)0.35);
                                    m_Price += (decimal)0.80;
                                }
                                else if (IsExtraToppingPossible(topping) && m_Size.ToString().ToLower().Equals("extralarge"))
                                {
                                    m_ExtraToppings.Add(topping);
                                    m_ExtraToppingsQuantity.Add((decimal)0.75);
                                    m_Price += (decimal)1.50;
                                }
                                else
                                {
                                    throw new Exception(string.Format("Not enough of topping ({0}) in stock. ", topping.GetToppingName()));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if stuffed crust is possible 
        /// </summary>
        /// <returns></returns>
        public bool IsStuffedCrustPossible()
        {
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                switch (m_Toppings[i].GetToppingName().ToLower())
                {
                    //If the quantity plus the quantity * extra quantity needed is less than 0 return false.
                    case "mozzarella":
                        if ((m_Toppings[i].GetToppingQuantity() - decimal.Round(m_ToppingQuantity[i] + (m_ToppingQuantity[i] * (decimal)0.15), 2)) < 0)
                        {
                            return false;
                        }
                        break;

                    case "dough":
                        if ((m_Toppings[i].GetToppingQuantity() - decimal.Round(m_ToppingQuantity[i] + (m_ToppingQuantity[i] * (decimal)0.1), 2)) < 0)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates wether the pizza should have stuffed crust.
        /// </summary>
        /// <param name="pIsStuffedCrustChecked">Inputted bool for stuffed crust.</param>
        public void HasStuffedCrust(bool pIsStuffedCrustChecked)
        {
            if (pIsStuffedCrustChecked)
            {
                //If stuffed crust is possible add the required quantity onto the current quantity.
                if (IsStuffedCrustPossible())
                {
                    for (int i = 0; i < m_Toppings.Count; i++)
                    {
                        switch (m_Toppings[i].GetToppingName().ToLower())
                        {
                            case "mozzarella":
                                m_ToppingQuantity[i] += decimal.Round(m_ToppingQuantity[i] * (decimal)0.15, 2);
                                break;

                            case "dough":
                                m_ToppingQuantity[i] += decimal.Round(m_ToppingQuantity[i] * (decimal)0.1, 2);
                                break;
                        }
                    }

                    //Add the required price increase.
                    switch (m_Size.ToString())
                    {
                        case "regular":
                            m_Price += 1;
                            break;

                        case "large":
                            m_Price += (decimal)1.50;
                            break;

                        case "extralarge":
                            m_Price += (decimal)2.50;
                            break;
                    }

                    m_HasStuffedCrust = true;
                }
                else
                {
                    m_HasStuffedCrust = false;
                    throw new Exception("Sorry there are not enough ingredients for stuffed crust. ");
                }
            }
            else
            {
                m_HasStuffedCrust = false;
            }
        }

        #region Accessors
        public List<Topping> GetPossibleExtraToppings()
        {
            return m_PossibleExtraToppings;
        }

        public List<Topping> GetExtraToppings()
        {
            return m_ExtraToppings;
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

        public List<Topping> GetDoughAndMozzarella()
        {
            List<Topping> toppings = new List<Topping>();
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                switch (m_Toppings[i].GetToppingName().ToLower())
                {
                    case "mozzarella":
                        toppings.Add(m_Toppings[i]);
                        break;

                    case "dough":
                        toppings.Add(m_Toppings[i]);
                        break;
                }
            }
            return toppings;
        }

        public List<decimal> GetDoughAndMozzarellaStuffedQuantities()
        {
            List<decimal> toppingsQuantity = new List<decimal>();
            for (int i = 0; i < m_Toppings.Count; i++)
            {
                switch (m_Toppings[i].GetToppingName().ToLower())
                {
                    case "mozzarella":
                        toppingsQuantity.Add(decimal.Round(m_ToppingQuantity[i] + (m_ToppingQuantity[i] * (decimal)0.15), 2));
                        break;

                    case "dough":
                        toppingsQuantity.Add(decimal.Round(m_ToppingQuantity[i] + (m_ToppingQuantity[i] * (decimal)0.1), 2));
                        break;
                }
            }
            return toppingsQuantity;
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

        public bool GetIfStuffedCrust()
        {
            return m_HasStuffedCrust;
        }
        #endregion
    }
}
