namespace RayTracerChallenge;

public static class Helpers
{
    public static (double min, double max) CheckAxis(double origin, double direction,
        double axisMin, double axisMax)
    {
        var minNumerator = axisMin - origin;
        var maxNumerator = axisMax - origin;

        double min;
        double max;

        if (Math.Abs(direction) >= Constants.Epsilon)
        {
            min = minNumerator / direction;
            max = maxNumerator / direction;
        }
        else
        {
            min = minNumerator * double.PositiveInfinity;
            max = maxNumerator * double.PositiveInfinity;
        }

        if (min > max)
        {
            (min, max) = (max, min);
        }
        
        return (min, max);
    }
}