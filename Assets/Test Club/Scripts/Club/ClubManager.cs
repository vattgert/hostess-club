using System.Diagnostics;

public class ClubManager
{
    private static ClubManager instance;

    private int renown;
    private int money;

    private ClubManager(int renownOnStart, int moneyOnStart)
    {
        this.renown = renownOnStart;
        this.money = moneyOnStart;
    }

    public static ClubManager GetInstance()
    {
        // If the instance reference has not been set, yet, 
        if (instance == null)
        {
            instance = new ClubManager(0, 5000);
        }
        return instance;
    }

    public int GetCurrentBalance()
    {
        return this.money;
    }

    public void AddIncome(int income)
    {
        if (income >= 0)
        {
            this.money += income;
        } else
        {
            Debug.Fail("For some reason, income is less than 0: " + income);
        }
    }

    public void IncreaseRenown(int increase)
    {
        this.renown += increase;
    }
}
