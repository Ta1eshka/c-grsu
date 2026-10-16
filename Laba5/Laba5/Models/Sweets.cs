namespace NewYearGiftProject.Models.Sweets
{
    public class ChocolateCandy : Sweet
    {
        public double CocoaPercentage { get; private set; }

        public ChocolateCandy(string name, double weight, double sugarContent, double cocoaPercentage)
            : base(name, weight, sugarContent)
        {
            CocoaPercentage = cocoaPercentage;
        }

        public override string GetDetails() =>
            $"Шоколадная конфета '{Name}' (Какао: {CocoaPercentage}%, Вес: {Weight}г, Сахар: {SugarContent}г)";
    }

    public class CaramelCandy : Sweet
    {
        public bool HasFilling { get; private set; }

        public CaramelCandy(string name, double weight, double sugarContent, bool hasFilling)
            : base(name, weight, sugarContent)
        {
            HasFilling = hasFilling;
        }

        public override string GetDetails() =>
            $"Карамель '{Name}' (С начинкой: {(HasFilling ? "Да" : "Нет")}, Вес: {Weight}г, Сахар: {SugarContent}г)";
    }
}