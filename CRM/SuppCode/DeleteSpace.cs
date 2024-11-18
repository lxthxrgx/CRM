using System.Text.RegularExpressions;

public static class DeleteSpace
{
    public static string Deletespace(string p)
    {
        try
        {
            if (p is not null)
            {
                var cleanedData = Regex.Replace(p, @"\s+", " ").Trim();
                return cleanedData;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Deletespace: {ex.Message}");
        }
        return null;
    }
}
