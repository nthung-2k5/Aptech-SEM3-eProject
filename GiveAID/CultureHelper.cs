using System.Globalization;

namespace GiveAID;

public static class CultureHelper
{
    private static readonly CultureInfo UsCulture = CultureInfo.GetCultureInfo("en-US");
    
    public static string ToUsCurrencyString(this decimal amount)
    {
        return amount.ToString("C", UsCulture);
    }
}
