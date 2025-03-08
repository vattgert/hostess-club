using System;
using System.Collections.Generic;

public class Host
{
    public static class HostessNameAssigner
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

    public string Name { get; }
    public int ChargePerHour { get; }
    public float TimeWithCustomer { get; }
    public float ChargeInterval { get; }

    public Host()
    {
        this.Name = HostessNameAssigner.GetRandomFemaleName();
        this.ChargePerHour = 100;
        this.ChargeInterval = 2f;
        this.TimeWithCustomer = 10f;
    }
}
