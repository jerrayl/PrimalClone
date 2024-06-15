namespace Primal.Business.Helpers
{
    public static class Mappings
    {
        public static int[] AggressionIconMap(int aggressionLevel) => aggressionLevel switch {
            0 => [0,1,2],
            1 => [1,2,3],
            2 => [2,3,4],
            3 => [3,4,5],
            _ => []
        };
    }
}