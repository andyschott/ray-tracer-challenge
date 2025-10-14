namespace RayTracerChallenge.Extensions;

static class DoubleExtensions
{
    public static bool IsEquivalent(this double left, double right)
    {
        var diff = Math.Abs(left - right);
        return diff < Constants.Epsilon;
    }
}