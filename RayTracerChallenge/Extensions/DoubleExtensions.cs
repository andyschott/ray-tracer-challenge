namespace RayTracerChallenge.Extensions;

internal static class DoubleExtensions
{
    public static bool IsEquivalent(this double left, double right)
    {
        // Simple equality check first (needed to support positive and negative infinity)
        if (left == right)
        {
            return true;
        }
        
        var diff = Math.Abs(left - right);
        return diff < Constants.Epsilon;
    }
}