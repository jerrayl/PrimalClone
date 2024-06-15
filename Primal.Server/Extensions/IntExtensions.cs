namespace Primal.Extensions
{
    public static class IntExtensions
    {
        public static string ToEmptyIfZero(this int variable)
        {
            return variable == 0 ? string.Empty : variable.ToString();
        }

        public static string ToEmptyIfZero(this int variable, string? prefix, string? suffix)
        {
            return variable == 0 ? string.Empty : prefix.ToEmptyIfNull() + variable.ToString() + suffix.ToEmptyIfNull();
        }
    }
}