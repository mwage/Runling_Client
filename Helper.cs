using System.Text;

public static class Helper
{
    public static string GetFriendlyName(this string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            if (char.IsUpper(c))
                sb.Append(" ");
            sb.Append(c);
        }

        if (input.Length > 0 && char.IsUpper(input[0]))
            sb.Remove(0, 1);

        return sb.ToString();
    }
}