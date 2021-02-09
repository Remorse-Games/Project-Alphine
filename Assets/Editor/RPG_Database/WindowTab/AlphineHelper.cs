
public static class AlphineHelper
{
    public static int NumberMinFilter(ref int value, int defaultValue)
    {
        return value = value < defaultValue ? defaultValue : value;
    }
    
    public static int NumberMaxFilter(ref int value, int defaultValue)
    {
        return value = value > defaultValue ? defaultValue : value;
    }
    
    public static int NumberMinMaxFilter(ref int value, int defaultMinValue, int defaultMaxValue)
    {
        NumberMinFilter(ref value, defaultMinValue);
        return NumberMaxFilter(ref value, defaultMaxValue);
    }
}

