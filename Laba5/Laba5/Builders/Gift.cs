namespace NewYearGiftProject.Builders
{
    using Models;
    using Models.Sweets;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Gift
    {
        private readonly List<Sweet> _sweets = new List<Sweet>();

        public void AddSweet(Sweet sweet)
        {
            _sweets.Add(sweet);
        }

        public double GetTotalWeight()
        {
            return _sweets.Sum(s => s.Weight);
        }

        public void SortSweets()
        {
            _sweets.Sort(); 
        }

        public IEnumerable<Sweet> FindSweetsBySugarRange(double minSugar, double maxSugar)
        {
            return _sweets.Where(s => s.SugarContent >= minSugar && s.SugarContent <= maxSugar);
        }

        public void PrintSweets()
        {
            foreach (var sweet in _sweets)
            {
                Console.WriteLine(sweet.GetDetails());
            }
        }
    }

    public interface IGiftBuilder
    {
        void AddChocolates();
        void AddCaramels();
        Gift GetGift();
    }

    public class ChildrenGiftBuilder : IGiftBuilder
    {
        private Gift _gift = new Gift();

        public void AddChocolates()
        {
            _gift.AddSweet(new ChocolateCandy("Аленка", 15.5, 8.2, 45.0));
            _gift.AddSweet(new ChocolateCandy("Мишка на севере", 20.0, 10.5, 50.0));
        }

        public void AddCaramels()
        {
            _gift.AddSweet(new CaramelCandy("Раковые шейки", 10.0, 9.0, true));
            _gift.AddSweet(new CaramelCandy("Барбарис", 7.5, 6.5, false));
            _gift.AddSweet(new CaramelCandy("Дюшес", 8.0, 7.0, false));
        }

        public Gift GetGift()
        {
            return _gift;
        }
    }
}