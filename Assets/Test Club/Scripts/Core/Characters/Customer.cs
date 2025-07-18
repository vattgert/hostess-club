using System;
using System.Collections.Generic;


namespace Characters
{
    public enum CustomerType
    {
        Poor,
        Average,
        Rich,
        Celebrity,
        Tycoon,
    }
    public class Customer
    {
        public CustomerType Type { get; set; }
        public int Budget { get; set; }
        public Trait AppearancePreference { get; private set; }
        public Trait PersonalityPreference { get; private set; }

        private static Dictionary<CustomerType, (int, int)> BudgetRange = new Dictionary<CustomerType, (int, int)>()
    {
        { CustomerType.Poor, (1500,1500) },
        { CustomerType.Average, (500, 950) },
        { CustomerType.Rich, (1000, 1500) },
        { CustomerType.Celebrity, (1800, 2300) },
        { CustomerType.Tycoon, (2500, 5000) }
    };

        protected static int GenerateBudgetByType(CustomerType type)
        {
            int step = 10;
            (int LowBudget, int HighBudget) budgetRange = Customer.BudgetRange[type];
            // Basic checks (optional, for robustness)
            if (step <= 0) throw new ArgumentException("Step must be a positive number.");
            if (budgetRange.LowBudget > budgetRange.HighBudget) throw new ArgumentException("End cannot be less than start.");

            // Calculate how many possible 'step' values fit in the range
            // e.g., [start..end] with step = s => number of valid steps = ((end - start) / s) + 1
            int stepsCount = ((budgetRange.HighBudget - budgetRange.LowBudget) / step) + 1;

            // Use Random.Next to pick an integer k in [0..(stepsCount-1)]
            System.Random random = new Random();
            int k = random.Next(0, stepsCount);

            // Final random value is: start + k * step
            return budgetRange.LowBudget + k * step;
        }

        public Customer(CustomerType type)
        {
            Type = type;
            Budget = Customer.GenerateBudgetByType(type);
        }

        public void SetAppearancePreference(Trait trait)
        {
            AppearancePreference = trait;
        }

        public void SetPersonalityPreference(Trait trait)
        {
            PersonalityPreference = trait;
        }

        public override string ToString()
        {
            return "Type: " + Type + "\n"
                + "Budged: " + Budget + "\n"
                + "Appearance preference: " + AppearancePreference + "\n"
                + "Personality preference: " + PersonalityPreference;
        }
    }
}


