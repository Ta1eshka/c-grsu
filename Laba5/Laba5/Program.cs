using System;
using NewYearGiftProject.Builders;

namespace NewYearGiftProject
{
    class Program
    {
        static void Main()
        {
            IGiftBuilder builder = new ChildrenGiftBuilder();
            builder.AddChocolates();
            builder.AddCaramels();

            Gift myGift = builder.GetGift();

            Console.WriteLine($"--- Общий вес подарка: {myGift.GetTotalWeight()}г ---");

            Console.WriteLine("\n--- Содержимое подарка ---");
            myGift.PrintSweets();

            Console.WriteLine("\n--- Содержимое после сортировки по весу ---");
            myGift.SortSweets();
            myGift.PrintSweets();

            double minSugar = 7.0;
            double maxSugar = 9.5;
            Console.WriteLine($"\n--- Поиск: конфеты с сахаром от {minSugar}г до {maxSugar}г ---");

            var foundSweets = myGift.FindSweetsBySugarRange(minSugar, maxSugar);
            foreach (var sweet in foundSweets)
            {
                Console.WriteLine(sweet.GetDetails());
            }
        }
    }
}