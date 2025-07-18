
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ShiftData
{
    public int CumulativeEarnedPerShift { get; private set; } = 0;

    public Dictionary<string, int> EarnedPerHost { get; private set; } = new();

    public int ServedCustomers { get; private set; } = 0;

    public void AddEarning(string hostName, int sum)
    {
        if (EarnedPerHost.ContainsKey(hostName))
        {
            EarnedPerHost[hostName] += sum;
        } else
        {
            EarnedPerHost.Add(hostName, sum);
        }
        CumulativeEarnedPerShift += sum;
    }

    public void AddServedCustomer()
    {
        ServedCustomers += 1;
    }

    public void Clear()
    {
        CumulativeEarnedPerShift = 0;
        EarnedPerHost.Clear();
        ServedCustomers = 0;
    }

    public override string ToString()
    {
        return "Earned per shift: " + CumulativeEarnedPerShift + "\n" + "Served customers: " + ServedCustomers;
    }
}
