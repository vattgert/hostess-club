
namespace Characters
{
    public enum Compatibility
    {
        Full,
        Partial,
        Absent
    }

    public class CompatibilityCalcualtor
    {
        public static Compatibility Calculate(Host host, Customer customer)
        {
            bool compatible = host.Personality == customer.PersonalityPreference && host.Appearance == customer.AppearancePreference;
            bool partiallyCompatible = host.Personality == customer.PersonalityPreference || host.Appearance == customer.AppearancePreference;
            if (compatible)
            {
                return Compatibility.Full;
            }
            else if (partiallyCompatible)
            {
                return Compatibility.Partial;
            }
            return Compatibility.Absent;
        } 
    }
}

