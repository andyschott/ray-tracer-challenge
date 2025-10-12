namespace RayTracerChallenge.Extensions;

static class DecimalExtensions
{
    public static bool IsEquivalent(this decimal left, decimal right)
    {
        var diff = Math.Abs(left - right);
        return diff < Constants.Epsilon;
    }
}