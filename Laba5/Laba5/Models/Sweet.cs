using System;
using System.Collections.Generic;
using System.Linq;

namespace NewYearGiftProject.Models
{
    public abstract class Sweet : IComparable<Sweet>
    {
        public string Name { get; protected set; }
        public double Weight { get; protected set; } 
        public double SugarContent { get; protected set; } 

        protected Sweet(string name, double weight, double sugarContent)
        {
            Name = name;
            Weight = weight;
            SugarContent = sugarContent;
        }

        public abstract string GetDetails();

        public int CompareTo(Sweet other)
        {
            if (other == null) return 1;
            return Weight.CompareTo(other.Weight);
        }
    }
}