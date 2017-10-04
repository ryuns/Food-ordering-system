using System;
using System.Collections.Generic;
using System.IO;

namespace FoodOrderingSystemClasses
{
    public class FoodManager
    {
        private List<Food> m_Food = new List<Food>();
        private List<Topping> m_Inventory = new List<Topping>();

        public void AddFood(Food pFood)
        {
            m_Food.Add(pFood);
            m_Food.Sort();
        }

        public void AddTopping(Topping pTopping)
        {
            m_Inventory.Add(pTopping);
            m_Inventory.Sort();
        }

        #region FoodSection
        /// <summary>
        /// Gets a list of food based on the inputted food name.
        /// </summary>
        /// <param name="pFoodName">Inputted food name.</param>
        /// <returns></returns>
        public List<Food> GetFoodByName(string pFoodName)
        {
            List<Food> foodList = new List<Food>();
            foreach (Food food in m_Food)
            {
                if (food.GetFoodName().ToLower().Equals(pFoodName.ToLower()))
                {
                    foodList.Add(food);
                }
            }
            if (foodList.Count == 0)
            {
                throw new Exception(string.Format("No food with name {0} exists. ", pFoodName));
            }

            return foodList;
        }

        /// <summary>
        /// Gets a food item based on inputed food name and size.
        /// </summary>
        /// <param name="pFoodName">Inputted food name.</param>
        /// <param name="pFoodSize">Inputted food size.</param>
        /// <returns></returns>
        public Food GetFoodByNameSize(string pFoodName, string pFoodSize)
        {
            for (int i = 0; i < m_Food.Count; i++)
            {
                if (m_Food[i].GetFoodName().ToLower().Equals(pFoodName.ToLower()))
                {
                    if (m_Food[i].GetFoodSize().ToLower().Equals(pFoodSize.ToLower().Replace("-", "")))
                    {
                        return m_Food[i];
                    }
                }
            }
            throw new Exception(string.Format("No food with name ({0}) and size ({1}) exists. ", pFoodName, pFoodSize));
        }

        /// <summary>
        /// Gets a list of food items based on the type of food and size.
        /// </summary>
        /// <param name="pFoodType">Inputted food type.</param>
        /// <param name="pFoodSize">Inputted food size.</param>
        /// <returns></returns>
        public List<Food> GetFoodByTypeSize(string pFoodType, string pFoodSize)
        {
            List<Food> typeSizeList = new List<Food>();
            for (int i = 0; i < m_Food.Count; i++)
            {
                if (m_Food[i].GetFoodType().ToLower().Equals(pFoodType.ToLower()))
                {
                    if (m_Food[i].GetFoodSize().ToLower().Equals(pFoodSize.Replace("-", "").ToLower()))
                    {
                        typeSizeList.Add(m_Food[i]);
                    }
                    else if (pFoodSize.ToLower().Equals("all"))
                    {
                        typeSizeList.Add(m_Food[i]);
                    }
                }
                else if (pFoodType.ToLower().Equals("all"))
                {
                    if (m_Food[i].GetFoodSize().ToLower().Equals(pFoodSize.Replace("-", "").ToLower()))
                    {
                        typeSizeList.Add(m_Food[i]);
                    }
                    else if (pFoodSize.ToLower().Equals("all"))
                    {
                        typeSizeList.Add(m_Food[i]);
                    }
                }
            }
            if (typeSizeList.Count == 0)
            {
                throw new Exception(string.Format("No Food with the type ({0}) and size ({1}) exists.", pFoodType, pFoodSize));
            }
            return typeSizeList;
        }

        /// <summary>
        /// Gets all food menu items.
        /// </summary>
        /// <returns></returns>
        public List<Food> GetAllFood()
        {
            return m_Food;
        }
        #endregion

        #region ToppingSection
        /// <summary>
        /// Gets a topping based on inputted name.
        /// </summary>
        /// <param name="pToppingName">Inputted topping name.</param>
        /// <returns></returns>
        public Topping GetToppingByName(string pToppingName)
        {
            for (int i = 0; i < m_Inventory.Count; i++)
            {
                if (m_Inventory[i].GetToppingName().Replace(" ", "-").ToLower().Equals(pToppingName.Replace(" ", "-").ToLower()))
                {
                    return m_Inventory[i];
                }
            }
            throw new Exception(string.Format("Topping does not exist: {0}. ", pToppingName));
        }

        /// <summary>
        /// Gets a list of toppings based on the topping type.
        /// </summary>
        /// <param name="pToppingType">Inputted topping type.</param>
        /// <returns></returns>
        public List<Topping> GetToppingByType(string pToppingType)
        {
            List<Topping> typeList = new List<Topping>();
            for (int i = 0; i < m_Inventory.Count; i++)
            {
                if (m_Inventory[i].GetToppingType().ToLower().Equals(pToppingType.ToLower()))
                {
                    typeList.Add(m_Inventory[i]);
                }
                else if (pToppingType.ToLower().Equals("all"))
                {
                    typeList.Add(m_Inventory[i]);
                }
            }
            if (typeList.Count == 0)
            {
                throw new Exception(string.Format("No Toppings with the type {0} exists.", pToppingType));
            }
            return typeList;
        }

        /// <summary>
        /// Updates the inventory text file.
        /// </summary>
        public void UpdateInventory()
        {
            StreamWriter writer = new StreamWriter("inventory.txt");

            foreach (Topping topping in m_Inventory)
            {
                writer.WriteLine("{0}, {1}, {2}, {3}", topping.GetToppingType(), topping.GetToppingName(), topping.GetToppingCost(), topping.GetToppingQuantity());
            }
            writer.Flush();
            writer.Close();
        }
        #endregion

        #region ReadingFiles
        /// <summary>
        /// Reads in the inventory file.
        /// </summary>
        public void ReadInventory()
        {
            Topping newTopping;
            StreamReader textIn;

            //Tests to see if file exists.
            try
            {
                textIn = new StreamReader("inventory.txt");
            }
            catch
            {
                throw new Exception("File Does not exist. ");
            }

            //Reads a file for however many lines there are.
            while (!textIn.EndOfStream)
            {
                string[] line;
                line = textIn.ReadLine().Split(',');

                newTopping = new Topping(line[0].Trim(),
                                         line[1].Trim(),
                                         GetDecFromText(line[2].Trim()),
                                         GetDecFromText(line[3].Trim()));

                AddTopping(newTopping);
            }

            textIn.Close();
        }

        /// <summary>
        /// Reads in the menu file.
        /// </summary>
        public void ReadMenu()
        {
            Food newFood;
            StreamReader textIn;

            //Tests to see if the file exists.
            try
            {
                textIn = new StreamReader("menu.txt");
            }
            catch
            {
                throw new Exception("File Does not exist. ");
            }

            //Reads a file for however many lines there are.
            while (!textIn.EndOfStream)
            {
                string[] line;
                line = textIn.ReadLine().Split(',');

                //Creates a new list of toppings and adds the toppings to the list.
                //Creates a new list of extra toppings and adds the rest of the toppings to this list.
                List<Topping> foodToppings = new List<Topping>();
                List<Topping> extraToppings = new List<Topping>();
                List<decimal> toppingQuantity = new List<decimal>();

                for (int j = 4; j < line.Length; j++)
                {
                    //If j / 2 remainder = 0 then the line is a topping.
                    if (j % 2 == 0)
                    {
                        foodToppings.Add(GetToppingByName(line[j].Trim()));
                    }
                    //else the line is a topping qty.
                    else
                    {
                        toppingQuantity.Add(GetDecFromText(line[j]));
                    }
                }

                //Gets the possible extra toppings for pizzas and burgers.
                foreach (Topping inventoryTopping in m_Inventory)
                {
                    if (inventoryTopping.GetToppingType() == "pizza" && line[0] == "pizza")
                    {
                        //If the food items toppings list doesnt contain a topping then
                        //add that topping to the possible extra topping list.
                        if (!foodToppings.Contains(inventoryTopping))
                        {
                            extraToppings.Add(inventoryTopping);
                        }
                    }
                    if (inventoryTopping.GetToppingType() == "burger" && line[0] == "burger")
                    {
                        //If the topping is salad or cheddar add the item to possible topping list.
                        if (inventoryTopping.GetToppingName() == "salad" || inventoryTopping.GetToppingName() == "cheddar")
                        {
                            if (!foodToppings.Contains(inventoryTopping))
                            {
                                extraToppings.Add(inventoryTopping);
                            }
                        }
                    }
                }

                switch (line[0].ToLower().Trim())
                {
                    //If the type is a pizza, create a pizza.
                    case "pizza":
                        newFood = new Pizza(line[0].Trim(),
                                        line[1].Trim(),
                                        foodToppings,
                                        toppingQuantity,
                                        line[2].Trim().Replace("-", ""),
                                        GetDecFromText(line[3]),
                                        extraToppings);
                        break;

                    //If the type is a burger, create a burger.
                    case "burger":
                        newFood = new Burger(line[0].Trim(),
                                        line[1].Trim(),
                                        foodToppings,
                                        toppingQuantity,
                                        line[2].Trim().Replace("-", ""),
                                        GetDecFromText(line[3]),
                                        extraToppings);
                        break;

                    //If the type is a sundry, create a sundry.
                    case "sundry":
                        newFood = new Sundry(line[0].Trim(),
                                        line[1].Trim(),
                                        foodToppings,
                                        toppingQuantity,
                                        line[2].Trim().Replace("-", ""),
                                        GetDecFromText(line[3]));
                        break;

                    //Otherwise type isnt valid.
                    default:
                        throw new Exception(string.Format("There is no food with the type: {0}. ", line[0]));
                }

                AddFood(newFood);
            }

            textIn.Close();
        }

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
            if (!textAmountTest)
            {
                throw new Exception(string.Format("The amount must be a numberic value: {0}. ", pTextVal));
            }

            return pAmount;
        }
        #endregion
    }
}
