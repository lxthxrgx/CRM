using Microsoft.EntityFrameworkCore;
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

public static class Search
{
    public static async Task<List<T>> SearchInNameGroup<T>(DbContext context, Func<T, bool> predicate = null) where T : class
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        var dbSet = context.Set<T>();

        if (predicate != null)
        {
            return await Task.FromResult(dbSet.Where(predicate).ToList());
        }

        return await dbSet.ToListAsync();
    }
}
