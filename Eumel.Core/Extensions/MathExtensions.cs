namespace Eumel.Dj
{
    public static class MathExtensions
    {
        public static int WithMax(this int source, int max)
        {
            return (source > max) ? max : source;
        }
    }
}