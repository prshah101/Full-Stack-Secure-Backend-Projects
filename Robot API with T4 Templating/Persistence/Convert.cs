using FastMember;
using Npgsql;
namespace robot_controller_api.Persistence
{
    public static class Convert
    {
        internal static string? ToPascalCase(string snakeCase)
        {
            var parts = snakeCase.Split('_');
            var result = new System.Text.StringBuilder();
            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result.Append(char.ToUpper(part[0]) + part.Substring(1).ToLower());
                }
            }
            return result.ToString();
        }

    }
}