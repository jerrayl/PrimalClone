namespace Primal.Extensions
{
    public static class BossPartExtensions
    {
        public static string ConvertToString(this (BossPart, int) bossPart)
        {
            return $"{bossPart.Item1} {bossPart.Item2}";
        }
    }
}