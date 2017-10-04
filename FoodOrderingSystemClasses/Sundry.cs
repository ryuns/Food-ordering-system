using System.Collections.Generic;

namespace FoodOrderingSystemClasses
{
    public class Sundry : Food
    {
        public Sundry(string pType, string pName, List<Topping> pTopping, List<decimal> pToppingQty, string pSize, decimal pPrice) : base(pType, pName, pTopping, pToppingQty, pSize, pPrice)
        {
            CalculateTotalToppingCost();
            CalculateGrossProfit();
            IsInStock();
        }
    }
}
