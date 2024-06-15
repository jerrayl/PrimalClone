using System;
using System.Linq;

namespace Primal.Extensions
{
    public static class StringExtensions
    {
        public static string ToEmptyIfNull(this string? variable)
        {
            return variable is null ? string.Empty : variable;
        }

        public static string ToEmptyIfNull<T>(this T variable, string? prefix, string? suffix)
        {
            return variable is null ? string.Empty : prefix.ToEmptyIfNull() + variable.ToString() + suffix.ToEmptyIfNull();
        }

        private static readonly Random random = new();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}