using System;

namespace Characters
{
    static class HostessNameAssigner
    {
        // List of 20 female names
        private static readonly string[] femaleNames = new string[]
        {
        "Alice", "Bella", "Charlotte", "Daisy", "Eva",
        "Fiona", "Grace", "Hannah", "Ivy", "Jade",
        "Kayla", "Lily", "Maya", "Nora", "Olivia",
        "Penelope", "Quinn", "Ruby", "Sophia", "Zoe"
        };

        // Use a static Random instance to avoid getting the same seed on rapid calls.
        private static readonly Random rnd = new Random();

        // Function to randomly assign a female name to a hostess
        public static string GetRandomFemaleName()
        {
            int index = rnd.Next(femaleNames.Length);
            return femaleNames[index];
        }
    }

    public class Host
    {
        private static int HOST_MAX_STAMINA = 100;
        private static int EXHAUSTION_THRESHOLD = 10;
        private static int UNAVAILABLE_NEXT_SHIFT_THRESHOLD = 5;
        private static int CHANCE_UNAVAILABLE_NEXT_SHIFT_THRESHOLD = 20;
    
        public string Name { get; }
        public Trait Personality { get; private set; }
        public Trait Appearance { get; private set; }
        public int ChargePerHour { get; }
        public float ChargeInterval { get; }
        public float Stamina { get; private set; }
        public bool AvailableNextShift { get; private set; }

        public Host()
        {
            Name = HostessNameAssigner.GetRandomFemaleName();
            ChargePerHour = 100;
            ChargeInterval = 2f;
            // TODO: In the future, stamina must be counted based on the minimum stamina from the last shift
            Stamina = HOST_MAX_STAMINA;
        }

        public void SetPersonality(Trait trait)
        {
            Personality = trait;
        }

        public void SetAppearance(Trait trait)
        {
            Appearance = trait;
        }

        public void DecreaseStamina(float staminaLoss)
        {
            Stamina -= staminaLoss;
            CalculateNextShiftAvailability();
        }

        public bool Exhausted()
        {
            return Stamina <= EXHAUSTION_THRESHOLD;
        }

        private void CalculateNextShiftAvailability()
        {
            if (Stamina <= UNAVAILABLE_NEXT_SHIFT_THRESHOLD)
            {
                AvailableNextShift = false;
            }
            else if (Stamina <= CHANCE_UNAVAILABLE_NEXT_SHIFT_THRESHOLD)
            {
                Random random = new Random();
                AvailableNextShift = random.NextDouble() > 0.5; // 50% chance
            }
            else
            {
                AvailableNextShift = true;
            }
        }

        public override string ToString()
        {
            return "Name: " + Name + "\n"
                + "Charge: " + ChargePerHour + "\n"
                + "Appearance trait: " + Appearance + "\n"
                + "Personality trait: " + Personality + "\n"
                + "Stamina: " + Stamina;
        }
    }
}

